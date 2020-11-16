using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames.QuestSystem
{
    public static class QuestSystemMenu
    {
        [MenuItem("Tools/Devion Games/Quest System/Editor", false, 0)]
        private static void OpenEditor()
        {
            QuestSystemEditor.ShowWindow();
        }

		[MenuItem("Tools/Devion Games/Quest System/Create Quest Manager", false, 1)]
		private static void CreateQuestManager()
		{
			GameObject go = new GameObject("Quest Manager");
			go.AddComponent<QuestManager>();
			Selection.activeGameObject = go;
		}

		[MenuItem("Tools/Devion Dames/Quest System/Create Quest Manager", true)]
		static bool ValidateCreateQuestManager()
		{
			return GameObject.FindObjectOfType<QuestManager>() == null;
		}
	}
}