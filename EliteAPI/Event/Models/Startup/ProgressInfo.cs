using EliteAPI.Event.Models.Abstractions;
using Newtonsoft.Json;

namespace EliteAPI.Event.Models.Startup
{
    /// <summary>
    /// This event is written when the game has started.
    /// </summary>
    /// <seealso cref="RankInfo"/>
    public class ProgressEvent : EventBase
    {
        internal ProgressEvent() { }

        public static ProgressEvent FromJson(string json) => JsonConvert.DeserializeObject<ProgressEvent>(json);

        /// <summary>
        /// The rank process in a percentages within Combat the commander has.
        /// Goes from 0-100.
        /// </summary>
        [JsonProperty("Combat")]
        public short Combat { get; internal set; }

        /// <summary>
        /// The rank process in a percentages within Trade the commander has.
        /// Goes from 0-100.
        /// </summary>
        [JsonProperty("Trade")]
        public short Trade { get; internal set; }

        /// <summary>
        /// The rank process in a percentages within Exploration the commander has.
        /// Goes from 0-100.
        /// </summary>
        [JsonProperty("Explore")]
        public short Explore { get; internal set; }

        /// <summary>
        /// The rank process in a percentages within the Empire the commander has.
        /// Goes from 0-100.
        /// </summary>
        [JsonProperty("Empire")]
        public short Empire { get; internal set; }

        /// <summary>
        /// The rank process in a percentages within the Federation the commander has.
        /// Goes from 0-100.
        /// </summary>
        [JsonProperty("Federation")]
        public short Federation { get; internal set; }

        /// <summary>
        /// The rank process in a percentages within CQC the commander has.
        /// Goes from 0-100.
        /// </summary>
        [JsonProperty("CQC")]
        public short Cqc { get; internal set; }

        
    }
}