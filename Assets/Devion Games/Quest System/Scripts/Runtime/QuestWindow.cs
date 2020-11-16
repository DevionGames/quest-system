using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.QuestSystem
{
    /// <summary>
    /// QuestWindow responsible for giving player the quest or completing the quest.
    /// </summary>
    public class QuestWindow : UIWidget
    {
        [Header("References")]
        [SerializeField]
        protected Text m_QuestTitle;
        [SerializeField]
        protected Text m_QuestDescription;
        [SerializeField]
        protected TaskSlot m_TaskSlotPrefab;
        [SerializeField]
        protected RectTransform m_RewardParent;

        [SerializeField]
        protected Button m_AcceptButton;
        [SerializeField]
        protected Button m_DeclineButton;
        [SerializeField]
        protected Button m_CompleteButton;
        [SerializeField]
        protected Button m_CancelButton;

        protected Scrollbar[] m_Scrollbars;

        private List<TaskSlot> m_TaskSlotCache;

        protected override void OnStart()
        {
            base.OnStart();
            this.m_TaskSlotCache = new List<TaskSlot>();
            this.m_Scrollbars = GetComponentsInChildren<Scrollbar>();
        }

        public virtual void Show(Quest quest) {
            base.Show();
            Canvas.ForceUpdateCanvases();
            for (int i = 0; i < this.m_Scrollbars.Length; i++) {
                this.m_Scrollbars[i].value = 1f;
            }
            for (int i = 0; i < this.m_TaskSlotCache.Count; i++)
                this.m_TaskSlotCache[i].gameObject.SetActive(false);

            if (this.m_QuestTitle != null)
                this.m_QuestTitle.text = quest.Title;

            if(this.m_QuestDescription != null)
                this.m_QuestDescription.text = quest.Description;



            for (int i = 0; i < quest.tasks.Count; i++) {
                TaskSlot slot = null;
                if (i < this.m_TaskSlotCache.Count)
                {
                    slot = this.m_TaskSlotCache[i];
                }else {
                    slot = Instantiate(this.m_TaskSlotPrefab);
                    slot.transform.SetParent(this.m_TaskSlotPrefab.transform.parent, false);
                    this.m_TaskSlotCache.Add(slot);
                }
                slot.gameObject.SetActive(true);
                slot.Repaint(quest.tasks[i]);
            }

            int count = this.m_RewardParent.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                 DestroyImmediate(m_RewardParent.GetChild(i).gameObject);
            }

            quest.DisplayReward(this.m_RewardParent);

            if (this.m_AcceptButton != null)
            {
                if (quest.CanActivate())
                {
                    this.m_AcceptButton.gameObject.SetActive(true);
                    this.m_AcceptButton.onClick.RemoveAllListeners();
                    this.m_AcceptButton.onClick.AddListener(() =>
                    {
                        quest.Activate();
                        BaseTrigger.currentUsedTrigger.InUse = false;
                        Close();
                    });
                }
                else
                {

                    this.m_AcceptButton.gameObject.SetActive(false);
                }
            }

            if (this.m_DeclineButton != null)
            {
                if (quest.CanActivate())
                {
                    this.m_DeclineButton.gameObject.SetActive(true);
                    this.m_DeclineButton.onClick.RemoveAllListeners();
                    this.m_DeclineButton.onClick.AddListener(() =>
                    {
                        quest.Decline();
                        BaseTrigger.currentUsedTrigger.InUse = false;
                        Close();
                    });
                }
                else
                {

                    this.m_DeclineButton.gameObject.SetActive(false);
                }
            }

            if (this.m_CompleteButton != null)
            {
                if (quest.CanComplete())
                {
                    this.m_CompleteButton.gameObject.SetActive(true);
                    this.m_CompleteButton.onClick.RemoveAllListeners();
                    this.m_CompleteButton.onClick.AddListener(() =>
                    {
                        quest.Complete();
                        BaseTrigger.currentUsedTrigger.InUse = false;
                        Close();
                    });
                }
                else
                {

                    this.m_CompleteButton.gameObject.SetActive(false);
                }
            }

            if (this.m_CancelButton != null)
            {
                if (quest.CanComplete())
                {
                    this.m_CancelButton.gameObject.SetActive(true);
                    this.m_CancelButton.onClick.RemoveAllListeners();
                    this.m_CancelButton.onClick.AddListener(() =>
                    {
                        quest.Cancel();
                        BaseTrigger.currentUsedTrigger.InUse = false;
                        Close();
                    });
                }
                else
                {

                    this.m_CancelButton.gameObject.SetActive(false);
                }
            }
        }
    }
}