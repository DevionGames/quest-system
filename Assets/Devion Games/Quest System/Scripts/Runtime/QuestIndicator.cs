using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DevionGames.QuestSystem
{
    public class QuestIndicator : MonoBehaviour
    {
        public GameObject availableQuest;
        public GameObject completeableQuest;
        public GameObject questTask;

        public List<QuestTaskSet> taskSets;

        private QuestCollection m_QuestCollection;

        

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);

            this.m_QuestCollection = GetComponent<QuestCollection>();
            QuestManager.current.OnQuestStatusChanged += OnQuestStatusChanged;
            QuestManager.current.OnTaskProgressChanged += OnTaskChanged;
            QuestManager.current.OnTaskStatusChanged += OnTaskChanged;
            UpdateQuestIndicator();
           
        }

        private void OnTaskChanged(Quest quest, QuestTask task)
        {
            UpdateQuestIndicator();
        }

        private void OnQuestStatusChanged(Quest quest)
        {
            UpdateQuestIndicator();
        }

        private void UpdateQuestIndicator()
        {
            availableQuest.SetActive(false);
            completeableQuest.SetActive(false);
            questTask.SetActive(false);


            for (int i = 0; i < taskSets.Count; i++)
            {
                Quest quest = QuestManager.current.ActiveQuests.FirstOrDefault(x => x.Name == taskSets[i].quest.Name);
                if (quest != null)
                {
                    QuestTask task = quest.tasks.FirstOrDefault(x => x.Name == taskSets[i].task);
                    if (task != null && task.Status == Status.Active)
                    {
                        questTask.SetActive(true);
                    }
                }
            }

            if (this.m_QuestCollection != null)
            {
                Quest combleteable = this.m_QuestCollection.FirstOrDefault(x => x.CanComplete());
                if (combleteable != null)
                {
                    completeableQuest.SetActive(true);
                    return;
                }

                Quest available = this.m_QuestCollection.FirstOrDefault(x => x.CanActivate());
                if (available != null)
                {
                    availableQuest.SetActive(true);
                }
            }

        }

        [System.Serializable]
        public class QuestTaskSet{
            [QuestPicker(true)]
            public Quest quest;
            public string task;
        }
    }
}