﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using EliteAPI.Bindings;
using EliteAPI.Discord;
using EliteAPI.Status;

using Newtonsoft.Json;

namespace EliteAPI
{
    public class EliteDangerousAPI : IEliteDangerousAPI
    {
        //Standard directory
        public static DirectoryInfo StandardDirectory { get => new DirectoryInfo($@"C:\Users\{Environment.UserName}\Saved Games\Frontier Developments\Elite Dangerous"); }

        //Version info.
        public FileVersionInfo Version { get { return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location); } }
        public long MajorVersion { get { return Version.FileMajorPart; } }
        public long MinorVersion { get { return Version.FileMinorPart; } }
        public string BuildVersion { get { return Version.FileVersion; } }

        public bool IsRunning { get; set; }
        public DirectoryInfo JournalDirectory { get; internal set; }
        public bool SkipCatchUp { get; internal set; }
        public Events.EventHandler Events { get; internal set; }
        public Logging.Logger Logger { get; internal set; }

        public ShipStatus Status { get; internal set; }
        public ShipCargo Cargo { get; internal set; }
        public ShipModules Modules { get { return ShipModules.FromFile(new FileInfo(JournalDirectory.FullName + "\\ModulesInfo.json"), this); } }
        public UserBindings Bindings
        {
            get
            {
                try
                {
                    string wantedFile = File.ReadAllText($@"C:\Users\{Environment.UserName}\AppData\Local\Frontier Developments\Elite Dangerous\Options\Bindings\StartPreset.start") + ".binds";
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(wantedFile);
                    return JsonConvert.DeserializeObject<UserBindings>(JsonConvert.SerializeXmlNode(xml));
                }
                catch { return new UserBindings(); }
            }
        }
        public CommanderStatus Commander { get; internal set; }
        public LocationStatus Location { get; internal set; }

        internal StatusWatcher StatusWatcher { get; set; }
        internal CargoWatcher CargoWatcher { get; set; }

        //Services.
        public RichPresenceClient DiscordRichPresence { get; set; }

        public EliteDangerousAPI(DirectoryInfo JournalDirectory, bool SkipCatchUp = true)
        {
            //Set the fields to the parameters.
            this.JournalDirectory = JournalDirectory;
            this.SkipCatchUp = SkipCatchUp;

            //Reset the API.
            Reset();

            try
            {
                //Go through file to set status fields.
                //Select the last edited Journal file.
                FileInfo journalFile = JournalDirectory.GetFiles("Journal.*").OrderByDescending(x => x.LastWriteTime).First();

                //Process the journal file.
                ProcessJournal(journalFile, false);
            }
            catch { }
        }

        public void Reset()
        {
            //Reset services.
            this.Events = new Events.EventHandler();
            this.Logger = new Logging.Logger();
            this.Commander = new CommanderStatus(this);
            this.Location = new LocationStatus(this);
            this.DiscordRichPresence = new RichPresenceClient(this);
            this.StatusWatcher = new StatusWatcher(this);
            this.CargoWatcher = new CargoWatcher(this);
            this.Status = ShipStatus.FromFile(new FileInfo(JournalDirectory + "//Status.json"), this);
        }

        public void Start()
        {
            Logger.LogInfo("Starting EliteAPI.");
            Logger.LogInfo("EliteAPI v" + BuildVersion + ".");

            //Mark the API as running.
            IsRunning = true;

            //We'll process the journal one time first, to catch up.
            //Select the last edited Journal file.

            FileInfo journalFile = null;

            try
            {
                journalFile = JournalDirectory.GetFiles("Journal.*").OrderByDescending(x => x.LastWriteTime).First();
                Logger.LogSuccess("Could find Journal files."); 
            }
            catch(Exception ex) { Logger.LogError("Could not start EliteAPI. Could not find Journal files.", ex); return; }

            //Process the journal file.
            ProcessJournal(journalFile, SkipCatchUp);

            //Go async.
            Task.Run(() =>
            {
                //Run for as long as we're running.
                while (IsRunning)
                {
                    //Select the last edited Journal file.
                    journalFile = JournalDirectory.GetFiles("Journal.*").OrderByDescending(x => x.LastWriteTime).First();

                    //Process the journal file.
                    ProcessJournal(journalFile, false);

                    //Wait half a second to avoid overusing the CPU.
                    Thread.Sleep(500);
                }
            });

            Logger.LogInfo("EliteAPI is ready.");
        }

        private List<string> processedLogs = new List<string>();

        private void ProcessJournal(FileInfo logFile, bool doNotTrigger = true)
        {
            //Create a stream from the log file.
            FileStream fileStream = logFile.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //Create a stream from the file stream.
            StreamReader streamReader = new StreamReader(fileStream);

            //Go through the stream.
            while (!streamReader.EndOfStream)
            {
                //If this string hasn't been processed yet, process it and mark it as processed.
                string json = streamReader.ReadLine();
                if (!processedLogs.Contains(json))
                {
                    if (!doNotTrigger) { Process(json); } //Only process it if it's marked true.
                    processedLogs.Add(json);
                }
		
		dynamic thisEvent = JsonConvert.DeserializeObject<dynamic>(json);
		if (thisEvent.@event != "Shutdown") { processedLogs.Clear(); }
            }
        }

        private void Process(string json)
        {
            dynamic obj = null;
            string eventName = "";

            try
            {
                //Turn the json into an object to find out which event it is.
                obj = JsonConvert.DeserializeObject<dynamic>(json);
                eventName = obj.@event;
            }
            catch(Exception ex) { Logger.LogWarning($"Couldn't process event [{json}].", ex); }

            //Invoke the matching event.
            try { Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Name == $"{eventName}Info").First().GetMethod("Process").Invoke(null, new object[] { json, this }); }
            catch(Exception ex) { Logger.LogError($"Could not invoke event {eventName}, it might not have been (correctly) added yet.", ex); }

            //Invoke the AllEvent.
            try { Events.InvokeAllEvent(obj); }
            catch(Exception ex) { Logger.LogError($"Could not invoke event {eventName}.", ex); }
        }

        public void Stop()
        {
            //Mark the API as not running.
            IsRunning = false;

            Logger.LogInfo("Stopping EliteAPI.");
        }
    }
}
