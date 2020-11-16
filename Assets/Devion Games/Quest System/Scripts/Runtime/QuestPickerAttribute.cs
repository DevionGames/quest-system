using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.QuestSystem
{
    public class QuestPickerAttribute : PropertyAttribute
    {
		public bool utility;

		public QuestPickerAttribute() : this(false) { }

		public QuestPickerAttribute(bool utility)
		{
			this.utility = utility;
		}
	}
}