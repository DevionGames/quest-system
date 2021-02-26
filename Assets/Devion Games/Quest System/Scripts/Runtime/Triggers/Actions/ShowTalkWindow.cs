using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevionGames.UIWidgets;
using UnityEngine;

namespace DevionGames.QuestSystem
{
    [Icon("Quest")]
    [ComponentMenu("Quest System/Show Talk Window")]
    [System.Serializable]
    public class ShowTalkWindow : Action
    {
        [SerializeField]
        protected string m_WindowName;
        [SerializeField]
        protected string m_Title = "Talk";
        [SerializeField]
        protected string m_Text = "";

        protected QuestTalkWindow m_TalkQuestWindow;

        public override void OnStart()
        {
            this.m_TalkQuestWindow = WidgetUtility.Find<QuestTalkWindow>(this.m_WindowName);
        }

        public override ActionStatus OnUpdate()
        {

            if (this.m_TalkQuestWindow == null)
            {
                Debug.LogWarning("Missing window " + this.m_WindowName + " in scene!");
                return ActionStatus.Failure;
            }
            this.m_TalkQuestWindow.Show(this.m_Title, this.m_Text);
            return ActionStatus.Success;
        }


    }
}
