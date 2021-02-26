using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.QuestSystem
{
    public class QuestTalkWindow : UIWidget
    {
        [Header("References")]
        [SerializeField]
        protected Text m_Title;
        [SerializeField]
        protected Text m_Text;

        public virtual void Show(string title, string text) {
            if (this.m_Title != null)
                this.m_Title.text = title;
            if (this.m_Text != null)
                this.m_Text.text = text;
            base.Show();
        }
    }
}