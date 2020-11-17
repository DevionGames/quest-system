using DevionGames.UIWidgets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DevionGames.QuestSystem
{
    public class QuestJournal : UIWidget
    {
        [Header("References")]
        [SerializeField]
        protected Dropdown m_Filter;
        [SerializeField]
        protected Button m_SelectButton;
        [SerializeField]
        protected QuestSlot m_QuestSlot;


        [SerializeField]
        protected Button m_CancelButton;

        private Dictionary<Quest, Button> m_QuestButtonMap;

        private Quest m_SelectedQuest;

        protected override void OnStart()
        {
            base.OnStart();
            if (this.m_Filter != null)
            {
                this.m_Filter.AddOptions(new List<string>() { "All", "Active", "Completed","Canceled","Failed" });
                this.m_Filter.onValueChanged.AddListener(Filter);
            }
            this.m_QuestButtonMap = new Dictionary<Quest, Button>();
            UnityTools.StartCoroutine(Reload());
            QuestManager.current.OnQuestStatusChanged += OnQuestStatusChanged;
            QuestManager.current.OnTaskStatusChanged += OnTaskStatusChanged;
            QuestManager.current.OnTaskProgressChanged += OnTaskProgressChanged;
            Repaint();
        }

        private IEnumerator Reload() {
            yield return new WaitForSeconds(1f);
            List<Quest> quests = QuestManager.current.AllQuests;
            for (int i = 0; i < quests.Count; i++) {
                CreateQuestButton(quests[i]);
            }
            Filter(this.m_Filter.value);
            Repaint();
        }

        public override void Show()
        {
            base.Show();
            Filter(this.m_Filter.value);
        }

        private void OnTaskProgressChanged(Quest quest, QuestTask task)
        {
            if (this.m_SelectedQuest == quest)
                Repaint();
        }

        private void OnTaskStatusChanged(Quest quest, QuestTask task)
        {
            if(this.m_SelectedQuest == quest)
                Repaint();
        }

        private void OnQuestStatusChanged(Quest quest)
        {

            Filter(this.m_Filter.value);
            if (quest.Status == Status.Active && !this.m_QuestButtonMap.ContainsKey(quest)) {
                CreateQuestButton(quest);
            }
            if (this.m_SelectedQuest == quest)
                Repaint();
        }

        private void CreateQuestButton(Quest quest) {
            if (!m_QuestButtonMap.ContainsKey(quest))
            {
                Button button = Instantiate(this.m_SelectButton);
                button.gameObject.SetActive(true);
                Text text = button.GetComponentInChildren<Text>();
                text.text = quest.Title;
                button.transform.SetParent(this.m_SelectButton.transform.parent, false);
                button.onClick.AddListener(() => { this.m_SelectedQuest = quest; Repaint(); });
                this.m_QuestButtonMap.Add(quest, button);
            }
        }

        public void Filter(int index)
        {
            this.m_SelectedQuest = null;
            Repaint();
            foreach( KeyValuePair<Quest, Button> kvp in this.m_QuestButtonMap){
                switch (index)
                {
                    case 0://All
                        kvp.Value.gameObject.SetActive(kvp.Key.Status != Status.Inactive);
                        break;
                    case 1://Active
                            kvp.Value.gameObject.SetActive(kvp.Key.Status == Status.Active);
                        break;
                    case 2://Completed
                        kvp.Value.gameObject.SetActive(kvp.Key.Status == Status.Completed);
                        break;
                    case 3:
                        kvp.Value.gameObject.SetActive(kvp.Key.Status == Status.Canceled);
                        break;
                    case 4:
                        kvp.Value.gameObject.SetActive(kvp.Key.Status == Status.Failed);
                        break;
                }
            }
        }

        private void Repaint() {
            this.m_QuestSlot.Repaint(this.m_SelectedQuest);

            if (this.m_CancelButton != null) {
                this.m_CancelButton.gameObject.SetActive(this.m_SelectedQuest != null && this.m_SelectedQuest.Status== Status.Active);
                if (this.m_SelectedQuest != null)
                {
                    this.m_CancelButton.onClick.RemoveAllListeners();
                    this.m_CancelButton.onClick.AddListener(() => {
                        QuestManager.UI.dialogBox.Show(QuestManager.Notifications.cancelQuest,(int result)=> {
                            switch (result) {
                                case 0:
                                    this.m_SelectedQuest.Cancel();
                                    break;
                                case 1:
                                    break;
                            }
                        } ,"Yes", "No");
    
                    });
                }
            }

        }
    }
}