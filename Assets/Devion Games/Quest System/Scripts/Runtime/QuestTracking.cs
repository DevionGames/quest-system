using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.QuestSystem
{
    public class QuestTracking : MonoBehaviour
    {
        [SerializeField]
        protected QuestSlot m_QuestSlot;

        private Dictionary<Quest, QuestSlot> m_QuestSlotMap;

        protected virtual void Start() {
            this.m_QuestSlotMap = new Dictionary<Quest, QuestSlot>();
            UnityTools.StartCoroutine(Reload());
            QuestManager.current.OnQuestStatusChanged += OnQuestStatusChanged;
            QuestManager.current.OnTaskStatusChanged += OnTaskStatusChanged;
            QuestManager.current.OnTaskProgressChanged += OnTaskStatusChanged;
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(1f);
            List<Quest> quests = QuestManager.current.ActiveQuests;
            for (int i = 0; i < quests.Count; i++)
            {
                QuestSlot slot = GetOrCreateSlot(quests[i]);
                slot.Repaint(quests[i]);
            }
           
        }


        private void OnTaskStatusChanged(Quest quest, QuestTask task)
        {
            QuestSlot slot = GetOrCreateSlot(quest);   
            slot.Repaint(quest);
        }

        private void OnQuestStatusChanged(Quest quest)
        {
            
            if (quest.Status == Status.Active)
            {
                QuestSlot slot = GetOrCreateSlot(quest);
                slot.Repaint(quest);
            }else {
                DestroySlot(quest);

            }

        }

        private void DestroySlot(Quest quest) {
            if (this.m_QuestSlotMap.ContainsKey(quest)) {
                Destroy(this.m_QuestSlotMap[quest].gameObject);
                this.m_QuestSlotMap.Remove(quest);
            }
        }

        private QuestSlot GetOrCreateSlot(Quest quest) {
            if (this.m_QuestSlotMap.ContainsKey(quest))
                return this.m_QuestSlotMap[quest];

            if (this.m_QuestSlot != null)
            {
                GameObject go = (GameObject)Instantiate(this.m_QuestSlot.gameObject);
                go.SetActive(true);
                go.transform.SetParent(this.m_QuestSlot.transform.parent, false);
                QuestSlot slot = go.GetComponent<QuestSlot>();
                this.m_QuestSlotMap.Add(quest, slot);
                return slot;
            }
            Debug.LogWarning("Please ensure that the slot prefab is set in the inspector.");
            return null;
        }
    }
}