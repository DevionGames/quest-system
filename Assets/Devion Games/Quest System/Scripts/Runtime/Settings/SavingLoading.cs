using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.QuestSystem.Configuration
{
    [System.Serializable]
    public class SavingLoading : Settings
    {
        public override string Name
        {
            get
            {
                return "Saving & Loading";
            }
        }

        public string savingKey = "Player";
        public bool autoSave = true;
        public SavingProvider provider = SavingProvider.PlayerPrefs;

        public enum SavingProvider
        {
            PlayerPrefs,
        }
    }
}