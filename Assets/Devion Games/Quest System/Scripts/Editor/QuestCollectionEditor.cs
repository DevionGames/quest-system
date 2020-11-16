using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace DevionGames.QuestSystem
{
	[System.Serializable]
	public class QuestCollectionEditor : ScriptableObjectCollectionEditor<Quest>
	{

		public override string ToolbarName
		{
			get
			{
				return "Quests";
			}
		}

		public QuestCollectionEditor(UnityEngine.Object target, List<Quest> items, List<string> searchFilters) : base(target, items)
		{
		}
	}
}