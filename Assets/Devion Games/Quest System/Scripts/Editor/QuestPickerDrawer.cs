using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace DevionGames.QuestSystem
{
	[CustomPropertyDrawer(typeof(QuestPickerAttribute), true)]
	public class QuestPickerDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			Quest current = property.GetValue() as Quest;
			position = EditorGUI.PrefixLabel(position, label);
			DoSelection(position, property, label, current);
			EditorGUI.EndProperty();
		}

		protected virtual void DoSelection(Rect buttonRect, SerializedProperty property, GUIContent label, Quest current)
		{

			GUIStyle buttonStyle = EditorStyles.objectField;
			GUIContent buttonContent = new GUIContent(current != null ? current.Name : "Null");
			if (GUI.Button(buttonRect, buttonContent, buttonStyle))
			{
				ObjectPickerWindow.ShowWindow(buttonRect, typeof(QuestDatabase), BuildSelectableObjects(),
					(UnityEngine.Object obj) => {
						property.serializedObject.Update();
						property.objectReferenceValue = obj;
						property.serializedObject.ApplyModifiedProperties();
					},
					() => {
						QuestDatabase db = EditorTools.CreateAsset<QuestDatabase>(true);
					});
			}
		}

		protected virtual List<Quest> GetItems(QuestDatabase database) {
			System.Type type = fieldInfo.FieldType;
			if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType))
			{
				type = Utility.GetElementType(fieldInfo.FieldType);
			}
			return database.items.Where(x => type.IsAssignableFrom(x.GetType())).ToList();
		}

		protected Dictionary<UnityEngine.Object, List<UnityEngine.Object>> BuildSelectableObjects()
		{
			Dictionary<UnityEngine.Object, List<UnityEngine.Object>> selectableObjects = new Dictionary<UnityEngine.Object, List<UnityEngine.Object>>();

			string[] guids = AssetDatabase.FindAssets("t:QuestDatabase");
			for (int i = 0; i < guids.Length; i++)
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);
				UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(QuestDatabase));
				List<UnityEngine.Object> items = GetItems(obj as QuestDatabase).Cast<UnityEngine.Object>().ToList();
				for (int j = 0; j < items.Count; j++)
				{
					items[j].name = (items[j] as INameable).Name;
				}
				selectableObjects.Add(obj, items);
			}
			return selectableObjects;
		}
	}
}
