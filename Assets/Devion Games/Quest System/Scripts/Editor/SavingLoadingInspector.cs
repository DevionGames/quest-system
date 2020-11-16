using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DevionGames.QuestSystem.Configuration;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
using System.Linq;

namespace DevionGames.QuestSystem
{
    [CustomEditor(typeof(SavingLoading))]
    public class SavingLoadingInspector : Editor
    {
        private SerializedProperty m_Script;
        private SerializedProperty m_AutoSave;
        private AnimBool m_ShowSave;
        private SerializedProperty m_SavingKey;
        private SerializedProperty m_Provider;

        protected virtual void OnEnable()
        {
            if (target == null) return;
            this.m_Script = serializedObject.FindProperty("m_Script");
            this.m_AutoSave = serializedObject.FindProperty("autoSave");
            this.m_ShowSave = new AnimBool(this.m_AutoSave.boolValue);
            this.m_ShowSave.valueChanged.AddListener(new UnityAction(Repaint));
            this.m_SavingKey = serializedObject.FindProperty("savingKey");
            this.m_Provider = serializedObject.FindProperty("provider");
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(this.m_Script);
            EditorGUI.EndDisabledGroup();

            serializedObject.Update();
            EditorGUILayout.PropertyField(this.m_AutoSave);
            this.m_ShowSave.target = this.m_AutoSave.boolValue;
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowSave.faded))
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(this.m_SavingKey);
                EditorGUILayout.PropertyField(this.m_Provider);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
            EditorGUILayout.EndFadeGroup();
            GUILayout.Space(2f);
            EditorTools.Seperator();

            List<string> keys = PlayerPrefs.GetString("QuestSystemSavedKeys").Split(';').ToList();
            keys.RemoveAll(x => string.IsNullOrEmpty(x));

            if (EditorTools.Foldout("QuestSavedData", new GUIContent("Saved Data " + keys.Count)))
            {
                EditorTools.BeginIndent(1, true);
                if (keys.Count == 0)
                {
                    GUILayout.Label("No data saved on this device!");
                }


                for (int i = 0; i < keys.Count; i++)
                {
                    string key = keys[i];
                  
  
                    GenericMenu keyMenu = new GenericMenu();

                    keyMenu.AddItem(new GUIContent("Delete Key"), false, () => {
                        List<string> allKeys = new List<string>(keys);
                        allKeys.Remove(key);
                        PlayerPrefs.SetString("QuestSystemSavedKeys", string.Join(";", allKeys));
                        PlayerPrefs.DeleteKey(key + ".ActiveQuests");
                        PlayerPrefs.DeleteKey(key + ".CompletedQuests");
                        PlayerPrefs.DeleteKey(key + ".FailedQuests");
                    });

                    if (EditorTools.Foldout(key, new GUIContent(key), keyMenu))
                    {
                        EditorTools.BeginIndent(1, true);
                        string activeQuests = PlayerPrefs.GetString(key + ".ActiveQuests");
                        string completedQuests = PlayerPrefs.GetString(key + ".CompletedQuests");
                        string failedQuests = PlayerPrefs.GetString(key + ".FailedQuests");

                        if (!string.IsNullOrEmpty(activeQuests))
                        {
                            GenericMenu uiMenu = new GenericMenu();
                            uiMenu.AddItem(new GUIContent("Delete Active Quests"), false, () => {
                                PlayerPrefs.DeleteKey(key + ".ActiveQuests");
                            });

                            if (EditorTools.Foldout(key + ".ActiveQuests", new GUIContent("Active Quests"), uiMenu))
                            {
                                EditorTools.BeginIndent(1, true);
                                GUILayout.Label(activeQuests, EditorStyles.wordWrappedLabel);
                                EditorTools.EndIndent();
                            }
                        }

                        if (!string.IsNullOrEmpty(completedQuests))
                        {
                            GenericMenu uiMenu = new GenericMenu();
                            uiMenu.AddItem(new GUIContent("Delete Completed Quests"), false, () => {
                                PlayerPrefs.DeleteKey(key + ".CompletedQuests");
                            });

                            if (EditorTools.Foldout(key + ".CompletedQuests", new GUIContent("Completed Quests"), uiMenu))
                            {
                                EditorTools.BeginIndent(1, true);
                                GUILayout.Label(completedQuests, EditorStyles.wordWrappedLabel);
                                EditorTools.EndIndent();
                            }
                        }

                        if (!string.IsNullOrEmpty(failedQuests))
                        {
                            GenericMenu uiMenu = new GenericMenu();
                            uiMenu.AddItem(new GUIContent("Delete Failed Quests"), false, () => {
                                PlayerPrefs.DeleteKey(key + ".FailedQuests");
                            });

                            if (EditorTools.Foldout(key + ".FailedQuests", new GUIContent("Failed Quests"), uiMenu))
                            {
                                EditorTools.BeginIndent(1, true);
                                GUILayout.Label(failedQuests, EditorStyles.wordWrappedLabel);
                                EditorTools.EndIndent();
                            }
                        }
                        EditorTools.EndIndent();
                    }
                }
                EditorTools.EndIndent();
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}