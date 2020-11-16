using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.QuestSystem
{
    public class QuestSlot : MonoBehaviour
    {
        [SerializeField]
        protected Text m_Title;
        [SerializeField]
        protected Text m_Description;
        [SerializeField]
        protected TaskSlot m_TaskSlotPrefab;
        [EnumFlags]
        [SerializeField]
        protected TaskDisplay m_DisplayTasks= TaskDisplay.Inactive | TaskDisplay.Active | TaskDisplay.Completed | TaskDisplay.Canceled | TaskDisplay.Failed;

        private List<TaskSlot> m_TaskSlotCache=new List<TaskSlot>();

        public virtual void Repaint(Quest quest) {

            if(this.m_Title != null)
                this.m_Title.text =  quest != null?quest.Title:"";

            if (this.m_Description != null)
                this.m_Description.text = quest!= null?quest.Description:"";

            for (int i = 0; i < this.m_TaskSlotCache.Count; i++)
                this.m_TaskSlotCache[i].gameObject.SetActive(false);

            if (quest != null)
            {
                for (int i = 0; i < quest.tasks.Count; i++)
                {
                    if (!this.m_DisplayTasks.HasFlag<TaskDisplay>(TaskDisplay.Inactive) && quest.tasks[i].Status == Status.Inactive || 
                        !this.m_DisplayTasks.HasFlag<TaskDisplay>(TaskDisplay.Active) && quest.tasks[i].Status == Status.Active || 
                        !this.m_DisplayTasks.HasFlag<TaskDisplay>(TaskDisplay.Completed) && quest.tasks[i].Status == Status.Completed || 
                        !this.m_DisplayTasks.HasFlag<TaskDisplay>(TaskDisplay.Failed) && quest.tasks[i].Status == Status.Failed || 
                        !this.m_DisplayTasks.HasFlag<TaskDisplay>(TaskDisplay.Canceled) && quest.tasks[i].Status == Status.Canceled)
                    {
                        continue;
                    }
                    TaskSlot slot = null;
                    if (i < this.m_TaskSlotCache.Count)
                    {
                        slot = this.m_TaskSlotCache[i];
                    }
                    else
                    {
                        slot = Instantiate(this.m_TaskSlotPrefab);
                        slot.transform.SetParent(this.m_TaskSlotPrefab.transform.parent, false);
                        this.m_TaskSlotCache.Add(slot);
                    }
                    slot.gameObject.SetActive(true);
                    slot.Repaint(quest.tasks[i]);
                }
            }
        }

        [System.Flags]
        public enum TaskDisplay
        {
            Inactive=1,
            Active=2,
            Completed=4,
            Failed=8,
            Canceled=16
        }
    }
}