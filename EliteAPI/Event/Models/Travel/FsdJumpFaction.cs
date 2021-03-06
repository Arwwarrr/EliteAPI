using System.Collections.Generic;
using Newtonsoft.Json;

namespace EliteAPI.Event.Models.Travel
{
    public class FSDJumpFaction
    {
        internal FSDJumpFaction() { }

        [JsonProperty("Name")]
        public string Name { get; internal set; }

        [JsonProperty("FactionState")]
        public string FactionState { get; internal set; } //todo: turn this into an enum

        [JsonProperty("Government")]
        public string Government { get; internal set; }

        [JsonProperty("Influence")]
        public float Influence { get; internal set; }

        [JsonProperty("Happiness")]
        public string Happiness { get; internal set; }

        [JsonProperty("Happiness_Localised", NullValueHandling = NullValueHandling.Ignore)]
        public string HappinessLocalised { get; internal set; }

        [JsonProperty("MyReputation")]
        public float CommanderReputation { get; internal set; }

        [JsonProperty("RecoveringStates", NullValueHandling = NullValueHandling.Ignore)]
        public List<FSDJumpFactionState> RecoveringStates { get; internal set; }

        [JsonProperty("PendingStates", NullValueHandling = NullValueHandling.Ignore)]
        public List<FSDJumpFactionState> PendingStates { get; internal set; }

        [JsonProperty("ActiveStates", NullValueHandling = NullValueHandling.Ignore)]
        public List<FSDJumpActiveState> ActiveStates { get; internal set; }

        public bool SquadronFaction { get; internal set; }
    }
}