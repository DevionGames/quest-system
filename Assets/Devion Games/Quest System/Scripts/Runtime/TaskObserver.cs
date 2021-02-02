using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DevionGames.QuestSystem
{
    public class TaskObserver : MonoBehaviour
    {
        [QuestPicker(true)]
        [SerializeField]
        protected Quest m_Quest;
        [InspectorLabel("Task")]
        [SerializeField]
        protected string m_TaskName;
        [SerializeField]
        protected float m_Progress = 1f;

        public void NotifyObserver() {
            Quest current = QuestManager.current.GetQuest(this.m_Quest.Name);

            if (current != null && current.Status == Status.Active)
            {
                QuestTask task = current.tasks.FirstOrDefault(x => x.Name == m_TaskName);
                if (task.Status == Status.Active)
                {
                    task.AddProgress(this.m_Progress);
                }
            }
        }
    }
}