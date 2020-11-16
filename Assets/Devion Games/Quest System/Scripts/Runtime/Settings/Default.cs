using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace DevionGames.QuestSystem.Configuration
{
    [System.Serializable]
    public class Default : Settings
    {
        public override string Name
        {
            get
            {
                return "Default";
            }
        }

        public string playerTag = "Player";

        [Header("Debug")]
        public bool debugMessages = true;
    }
}