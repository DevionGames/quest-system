using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.QuestSystem
{
    public class TaskSlot : MonoBehaviour
    {
        [SerializeField]
        protected Text m_TaskProgress;
        [SerializeField]
        protected Text m_TaskDescription;
        [SerializeField]
        protected Text m_RemainingTime;

        [SerializeField]
        protected Color m_DefaultColor = Color.white;
        [SerializeField]
        protected Color m_CompletedColor= Color.green;
        [SerializeField]
        protected Color m_FailedColor = Color.red;

        private QuestTask m_LastTask;

        

        public void Repaint(QuestTask task) {



            if (this.m_TaskProgress != null)
            {
                this.m_TaskProgress.color = this.m_DefaultColor;

                if(task.Status== Status.Completed)
                    this.m_TaskProgress.color = this.m_CompletedColor;

                if (task.Status == Status.Failed)
                    this.m_TaskProgress.color = this.m_FailedColor;

                this.m_TaskProgress.text = task.ProgressMessage;
            }
            if (this.m_TaskDescription != null)
            {
                this.m_TaskDescription.color = this.m_DefaultColor;

                if (task.Status == Status.Completed)
                    this.m_TaskDescription.color = this.m_CompletedColor;

                if (task.Status == Status.Failed)
                    this.m_TaskDescription.color = this.m_FailedColor;

                this.m_TaskDescription.text = task.Description;
            }


            this.m_LastTask = task;
        }

        private void Update()
        {
            if (this.m_RemainingTime != null && this.m_LastTask != null && this.m_LastTask.UseTimeLimit ) {
                float minutes = Mathf.FloorToInt(this.m_LastTask.RemainingTime / 60);
                float seconds = Mathf.FloorToInt(this.m_LastTask.RemainingTime % 60);
                if (this.m_LastTask.RemainingTime <= 0) {
                    this.m_RemainingTime.color = this.m_FailedColor;
                }
                m_RemainingTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
    }
}