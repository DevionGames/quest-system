using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace DevionGames.QuestSystem
{
	public class QuestSystemEditor : EditorWindow
	{

		public static void ShowWindow()
		{

			QuestSystemEditor[] objArray = Resources.FindObjectsOfTypeAll<QuestSystemEditor>();
			QuestSystemEditor editor = (objArray.Length <= 0 ? ScriptableObject.CreateInstance<QuestSystemEditor>() : objArray[0]);

			editor.hideFlags = HideFlags.HideAndDontSave;
			editor.minSize = new Vector2(690, 300);
			editor.titleContent = new GUIContent("Quest System");
			editor.SelectDatabase();
		}

		public static QuestSystemEditor instance;

		private QuestDatabase database;
		private static QuestDatabase db;
		public static QuestDatabase Database
		{
			get
			{
				if (QuestSystemEditor.instance != null)
				{
					db = QuestSystemEditor.instance.database;
				}
				return db;
			}
			set
			{
				db = value;
				if (QuestSystemEditor.instance != null)
				{
					QuestSystemEditor.instance.database = value;
				}
			}
		}

		private List<ICollectionEditor> childEditors;

		[SerializeField]
		private int toolbarIndex;

		private string[] toolbarNames
		{
			get
			{
				string[] items = new string[childEditors.Count];
				for (int i = 0; i < childEditors.Count; i++)
				{
					items[i] = childEditors[i].ToolbarName;
				}
				return items;
			}

		}

		private void OnEnable()
		{
			instance = this;

			if (database == null)
			{
				SelectDatabase();
			}
			ResetChildEditors();
		}

		private void OnDisable()
		{
			if (childEditors != null)
			{
				for (int i = 0; i < childEditors.Count; i++)
				{
					childEditors[i].OnDisable();
				}
			}
		}

		private void OnDestroy()
		{
			if (childEditors != null)
			{
				for (int i = 0; i < childEditors.Count; i++)
				{
					childEditors[i].OnDestroy();
				}
			}
			instance = null;
		}


		private void Update()
        {
			Repaint();
        }

        private void OnGUI()
		{
			if (childEditors != null)
			{
				EditorGUILayout.Space();
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarNames, GUILayout.MinWidth(200));
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				childEditors[toolbarIndex].OnGUI(new Rect(0f, 30f, position.width, position.height - 30f));
			}
		}

		private void SelectDatabase()
		{
			string searchString = "Search...";
			QuestDatabase[] databases = EditorTools.FindAssets<QuestDatabase>();

			UtilityInstanceWindow.ShowWindow("Select Database", delegate () {
				searchString = EditorTools.SearchField(searchString);

				for (int i = 0; i < databases.Length; i++)
				{
					if (!string.IsNullOrEmpty(searchString) && !searchString.Equals("Search...") && !databases[i].name.Contains(searchString))
					{
						continue;
					}
					GUIStyle style = new GUIStyle("button");
					style.wordWrap = true;
					if (GUILayout.Button(AssetDatabase.GetAssetPath(databases[i]), style))
					{
						database = databases[i];
						ResetChildEditors();
						Show();
						UtilityInstanceWindow.CloseWindow();
					}
				}
				GUILayout.FlexibleSpace();
				Color color = GUI.backgroundColor;
				GUI.backgroundColor = Color.green;
				if (GUILayout.Button("Create"))
				{
					QuestDatabase db = EditorTools.CreateAsset<QuestDatabase>(true);
					if (db != null)
					{
						ArrayUtility.Add<QuestDatabase>(ref databases, db);
					}
				}
				GUI.backgroundColor = color;
			});

		}

		private void ResetChildEditors()
		{
			if (database != null)
			{
				childEditors = new List<ICollectionEditor>();
				childEditors.Add(new QuestCollectionEditor(database, database.items, new List<string>()));
				childEditors.Add(new Configuration.QuestSettingsEditor(database, database.settings));
				for (int i = 0; i < childEditors.Count; i++)
				{
					childEditors[i].OnEnable();
				}
			}
		}
	}
}