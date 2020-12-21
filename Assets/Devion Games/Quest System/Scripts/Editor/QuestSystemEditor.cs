using UnityEngine;
using UnityEditor;

namespace DevionGames.QuestSystem
{
	public class QuestSystemEditor : EditorWindow
	{

		private QuestSystemInspector m_QuestSystemInspector;

		public static void ShowWindow()
		{

			QuestSystemEditor[] objArray = Resources.FindObjectsOfTypeAll<QuestSystemEditor>();
			QuestSystemEditor editor = (objArray.Length <= 0 ? ScriptableObject.CreateInstance<QuestSystemEditor>() : objArray[0]);

			editor.hideFlags = HideFlags.HideAndDontSave;
			editor.minSize = new Vector2(690, 300);
			editor.titleContent = new GUIContent("Quest System");

			editor.Show();
		}

		private void OnEnable()
		{
			this.m_QuestSystemInspector = new QuestSystemInspector();
			this.m_QuestSystemInspector.OnEnable();
		}

		private void OnDisable()
		{
			this.m_QuestSystemInspector.OnDisable();
		}

		private void OnDestroy()
		{
			this.m_QuestSystemInspector.OnDestroy();
		}

		private void Update()
		{
			if (EditorWindow.mouseOverWindow == this)
				Repaint();
		}

		private void OnGUI()
		{
			this.m_QuestSystemInspector.OnGUI(position);
		}
	}
}