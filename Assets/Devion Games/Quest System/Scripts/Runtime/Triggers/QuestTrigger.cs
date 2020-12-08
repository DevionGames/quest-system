using DevionGames.UIWidgets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace DevionGames.QuestSystem
{
    public class QuestTrigger : BaseTrigger
    {
        public static QuestWindow currentUsedWindow;

        public override PlayerInfo PlayerInfo => QuestManager.current.PlayerInfo;

        [Header("Selection")]
        [SerializeField]
        protected string m_Title = "Availible Quests";
        [SerializeField]
        protected string m_Text = "Select a quest to continue.";


        protected QuestCollection m_QuestCollection;

        protected override void Start()
        {
            base.Start();
            this.m_QuestCollection = GetComponent<QuestCollection>();

        }

        public override bool CanUse()
        {
            Quest quest = GetNextQuest();
            return base.CanUse() && quest != null;
        }

        public override bool Use()
        {
          
            //Can the trigger be used?
            if (!CanUse())
            {
                return false;
            }
            //Set the trigger in use
            this.InUse = true;
            currentUsedWindow = QuestManager.UI.questWindow;
            //currentUsedWindow.Show(GetNextQuest());
            
            for (int i = 0; i < this.m_QuestCollection.Count; i++) {
                Quest quest = this.m_QuestCollection[i];
                if (quest.CanComplete())
                {
                    currentUsedWindow.Show(quest);
                    return true;
                }
            }

            string[] quests = this.m_QuestCollection.Where(x => x.CanActivate()).Select(y => y.Name).ToArray();
            if (quests.Length > 1)
            {
                DialogBox questSelection = QuestManager.UI.questSelectionWindow;
                Debug.Log(questSelection);
                questSelection.RegisterListener("OnClose", (CallbackEventData eventData) => {
                    InUse = false;
                });

                questSelection.Show(this.m_Title, this.m_Text, (int result) => {
                    currentUsedWindow.Show(this.m_QuestCollection.FirstOrDefault(x => x.Name == quests[result]));
                }, quests);
            }else if(quests.Length == 1) {
                currentUsedWindow.Show(this.m_QuestCollection.FirstOrDefault(x=>x.Name == quests[0]));
            }
            return true;
        }

        private Quest GetNextQuest()
        {
            //First check a quest can be completed.
            for (int i = 0; i < this.m_QuestCollection.Count; i++)
            {
                Quest quest = this.m_QuestCollection[i];
                if (quest.CanComplete())
                {
                    return quest;

                }
            }
            //Check if a quest can be activated.
            for (int i = 0; i < this.m_QuestCollection.Count; i++)
            {
                Quest quest = this.m_QuestCollection[i];
                if (quest.CanActivate())
                {
                    return quest;

                }
            }
            return null;
        }

        protected override void DisplayInUse()
        {
            QuestManager.Notifications.inUse.Show();
        }

        protected override void DisplayOutOfRange()
        {
            QuestManager.Notifications.toFarAway.Show();
        }


        protected override void OnWentOutOfRange()
        {
            if (currentUsedWindow != null)
            {
                currentUsedWindow.Close();
                QuestTrigger.currentUsedWindow = null;
            }
        }
    }
}