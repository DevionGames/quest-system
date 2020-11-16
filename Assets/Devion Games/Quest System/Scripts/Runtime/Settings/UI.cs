using System.Collections;
using System.Collections.Generic;
using DevionGames.UIWidgets;
using UnityEngine;
using UnityEngine.Assertions;

namespace DevionGames.QuestSystem.Configuration
{
    [System.Serializable]
    public class UI : Settings
    {
        public override string Name
        {
            get
            {
                return "UI";
            }
        }

        [InspectorLabel("Notification", "Name of Notification widget.")]
        public string notificationName = "Notification";
        [InspectorLabel("Dialog Box", "Name of the dialog box widget.")]
        public string dialogBoxName = "Dialog Box";
        [InspectorLabel("Quest Window", "Name of the quest window widget.")]
        public string questWindowName = "Quest";

        private Notification m_Notification;
        public Notification notification
        {
            get
            {
                if (this.m_Notification == null)
                {
                    this.m_Notification = WidgetUtility.Find<Notification>(this.notificationName);
                }
                Assert.IsNotNull(this.m_Notification, "Notification widget with name " + this.notificationName + " is not present in scene.");
                return this.m_Notification;
            }
        }

        private DialogBox m_DialogBox;
        public DialogBox dialogBox
        {
            get
            {
                if (this.m_DialogBox == null)
                {
                    this.m_DialogBox = WidgetUtility.Find<DialogBox>(this.dialogBoxName);
                }
                Assert.IsNotNull(this.m_DialogBox, "DialogBox widget with name " + this.dialogBoxName + " is not present in scene.");
                return this.m_DialogBox;
            }
        }

        private QuestWindow m_QuestWindow;
        public QuestWindow questWindow
        {
            get
            {
                if (this.m_QuestWindow == null)
                {
                    this.m_QuestWindow = WidgetUtility.Find<QuestWindow>(questWindowName);
                }
                Assert.IsNotNull(this.m_QuestWindow, "QuestWindow widget with name " + this.questWindowName + " is not present in scene.");
                return this.m_QuestWindow;
            }
        }
    }
}