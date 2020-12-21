using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace DevionGames.QuestSystem
{
    [CustomEditor(typeof(QuestCollection), true)]
    public class QuestCollectionInspector : Editor
    {
        private SerializedProperty script;

        private SerializedProperty m_Quests;
        private ReorderableList m_QuestList;

        private void OnEnable()
        {
            this.script = serializedObject.FindProperty("m_Script");
            this.m_Quests = serializedObject.FindProperty("m_Quests");

            CreateItemList(serializedObject, this.m_Quests);
        }

        private void CreateItemList(SerializedObject serializedObject, SerializedProperty elements)
        {
            this.m_QuestList = new ReorderableList(serializedObject, elements, true, true, true, true);
            this.m_QuestList.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Quests");
            };

            this.m_QuestList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                float verticalOffset = (rect.height - EditorGUIUtility.singleLineHeight) * 0.5f;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y = rect.y + verticalOffset;
                SerializedProperty element = elements.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, GUIContent.none, true);
            };

            this.m_QuestList.onRemoveCallback = (ReorderableList list) =>
            {
                list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue = null;
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
            };
        }


        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(script);
            EditorGUI.EndDisabledGroup();
            serializedObject.Update();
            GUILayout.Space(3f);
            this.m_QuestList.DoLayoutList();
            EditorGUILayout.Space();
         
            serializedObject.ApplyModifiedProperties();
        }
    }
}