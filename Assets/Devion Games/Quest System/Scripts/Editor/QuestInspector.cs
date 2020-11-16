using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DevionGames.QuestSystem
{
    [CustomEditor(typeof(Quest),true)]
    public class QuestInspector : Editor
    {
        protected SerializedProperty m_Script;
        protected SerializedProperty m_QuestName;
        protected SerializedProperty m_Title;
        protected SerializedProperty m_Description;

        protected SerializedProperty m_AutoComplete;
        protected SerializedProperty m_RestartFailed;
        protected SerializedProperty m_TaskExecution;


        protected SerializedProperty m_Rewards;
        protected ReorderableList m_RewardList;

        protected SerializedProperty m_Tasks;
        protected ReorderableList m_TaskList;

        private int m_RenameTaskIndex = -1;
        private int m_ClickCount;

        private SerializedProperty m_Conditions;
        private UnityEngine.Object m_Target;
        private IList m_List;
        private string m_FieldName;
        private string m_ElementTypeName;
        private Type m_ElementType;
        private static object m_ObjectToCopy;

        protected virtual void OnEnable()
        {
            if (target == null) return;
            this.m_Script = serializedObject.FindProperty("m_Script");
            this.m_QuestName = serializedObject.FindProperty("m_QuestName");
            this.m_Title = serializedObject.FindProperty("m_Title");
            this.m_Description = serializedObject.FindProperty("m_Description");
            this.m_Rewards = serializedObject.FindProperty("rewards");
            CreateRewardList(serializedObject, this.m_Rewards);

            this.m_AutoComplete = serializedObject.FindProperty("m_AutoComplete");
            this.m_RestartFailed = serializedObject.FindProperty("m_RestartFailed");
            this.m_TaskExecution = serializedObject.FindProperty("m_TaskExecution");

            this.m_Tasks = serializedObject.FindProperty("tasks");
            CreateTaskList(serializedObject, this.m_Tasks);

            this.m_Conditions = serializedObject.FindProperty("conditions");
            this.m_Target = target;
            this.m_List = (target as Quest).conditions;
            this.m_ElementType = Utility.GetElementType(this.m_List.GetType());
            this.m_ElementTypeName = this.m_ElementType.FullName;
            FieldInfo[] fields = this.m_Target.GetType().GetSerializedFields();
            for (int i = 0; i < fields.Length; i++)
            {
                object temp = fields[i].GetValue(this.m_Target);
                if (temp == this.m_List)
                    this.m_FieldName = fields[i].Name;
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(this.m_Script);
            EditorGUI.EndDisabledGroup();

            serializedObject.Update();
            EditorGUILayout.PropertyField(this.m_QuestName);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.m_Title);
            EditorGUILayout.PropertyField(this.m_Description);
            RewardGUI();
            EditorGUILayout.PropertyField(this.m_AutoComplete);
            EditorGUILayout.PropertyField(this.m_RestartFailed);
            EditorGUILayout.PropertyField(this.m_TaskExecution);
            TaskGUI();
            ConditionGUI();
            serializedObject.ApplyModifiedProperties();
        }


        protected void CreateRewardList(SerializedObject serializedObject, SerializedProperty elements)
        {
            this.m_RewardList = new ReorderableList(serializedObject, elements, true, true, true, true);
            this.m_RewardList.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Rewards");
            };

            this.m_RewardList.onAddCallback = (ReorderableList list) => {
                Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(x => typeof(Reward).IsAssignableFrom(x) && !x.IsAbstract && !x.HasAttribute(typeof(ExcludeFromCreation))).ToArray();
                types = types.OrderBy(x => x.BaseType.Name).ToArray();

                if (types.Length > 0)
                {
                    GenericMenu menu = new GenericMenu();
                    for (int i = 0; i < types.Length; i++)
                    {
                        Type type = types[i];
                        menu.AddItem(new GUIContent(types[i].FullName), false, () => { AddReward(type); });
                    }
                    menu.ShowAsContext();
                }
                else
                {
                    Debug.LogWarning("No reward implementations found.");
                }

            };

            this.m_RewardList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                float verticalOffset = (rect.height - EditorGUIUtility.singleLineHeight) * 0.5f;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y = rect.y + verticalOffset;
                SerializedProperty element = this.m_Rewards.GetArrayElementAtIndex(index);
                Type type = element.GetValue().GetType();
                string typeDef = type.FullName;
                GUI.Label(rect, typeDef);
            };
        }

        private void AddReward(Type type)
        {
            object value = System.Activator.CreateInstance(type);
            serializedObject.Update();
            this.m_Rewards.arraySize++;
            this.m_Rewards.GetArrayElementAtIndex(this.m_Rewards.arraySize - 1).managedReferenceValue = value;
            serializedObject.ApplyModifiedProperties();
        }

        private void RewardGUI() {
            EditorGUILayout.Space();
            this.m_RewardList.DoLayoutList();
            EditorGUILayout.Space();

            if (this.m_RewardList.index > -1)
            {
                SerializedProperty element = this.m_Rewards.GetArrayElementAtIndex(this.m_RewardList.index);

                
                foreach (var child in element.EnumerateChildProperties())
                { 
                    EditorGUILayout.PropertyField(child, includeChildren: true);
                }
            }
        }

        protected void CreateTaskList(SerializedObject serializedObject, SerializedProperty elements) {
            this.m_TaskList = new ReorderableList(serializedObject, elements, true, true, true, true);
            this.m_TaskList.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Tasks");
            };

            this.m_TaskList.onAddCallback = (ReorderableList list) => {
                Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(x => typeof(QuestTask).IsAssignableFrom(x) && !x.IsAbstract && !x.HasAttribute(typeof(ExcludeFromCreation))).ToArray();
                types = types.OrderBy(x => x.BaseType.Name).ToArray();
               
                if (types.Length > 1) {
                    GenericMenu menu = new GenericMenu();
                    for (int i = 0; i < types.Length; i++) {
                        Type type = types[i];
                        menu.AddItem(new GUIContent(types[i].FullName), false, () => { AddTask(type); });
                    }
                    menu.ShowAsContext();
                }
                else { 
                    AddTask(typeof(QuestTask));
                }
        
            };

            this.m_TaskList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused)=> {
                float verticalOffset = (rect.height - EditorGUIUtility.singleLineHeight) * 0.5f;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y = rect.y + verticalOffset;
                SerializedProperty element = this.m_Tasks.GetArrayElementAtIndex(index);
                SerializedProperty name = element.FindPropertyRelative("m_Name");

                if (index == this.m_RenameTaskIndex)
                {

                    string before = name.stringValue;
                    GUI.SetNextControlName("RenameTaskField");
                    string after = EditorGUI.TextField(rect, name.stringValue);
                    if (before != after)
                    {
                        name.stringValue = after;
                    }
                }
                else
                {
                    Type type = element.GetValue().GetType();
                    string typeDef = " (" + type.FullName + ")";
                    GUI.Label(rect, name.stringValue+typeDef);
                }
                Event currentEvent = Event.current;
                switch (currentEvent.rawType)
                {
                    case EventType.MouseDown:
                        if (rect.Contains(currentEvent.mousePosition) && index == m_TaskList.index && currentEvent.button == 0 && currentEvent.type == EventType.MouseDown)
                        {
                            this.m_ClickCount += 1;
                        }
                        break;
                    case EventType.KeyUp:
                        if (currentEvent.keyCode == KeyCode.Return && this.m_RenameTaskIndex != -1)
                        {
                            this.m_RenameTaskIndex = -1;
                            currentEvent.Use();
                        }
                        break;
                    case EventType.MouseUp:
                        if (this.m_ClickCount > 0 && rect.Contains(currentEvent.mousePosition) && index == m_TaskList.index && currentEvent.button == 0 && currentEvent.type == EventType.MouseUp)
                        {
                            this.m_RenameTaskIndex = index;
                            this.m_ClickCount = 0;
                            EditorGUI.FocusTextInControl("RenameTaskField");
                            Event.current.Use();

                        }
                        else if (!rect.Contains(Event.current.mousePosition) && Event.current.clickCount > 0 && index == this.m_TaskList.index && this.m_RenameTaskIndex != -1)
                        {
                            this.m_RenameTaskIndex = -1;
                            Event.current.Use();
                        }
                        break;
                }
            };
        }

        private void AddTask(Type type) {
            object value = System.Activator.CreateInstance(type);
            serializedObject.Update();
            this.m_Tasks.arraySize++;
            this.m_Tasks.GetArrayElementAtIndex(this.m_Tasks.arraySize - 1).managedReferenceValue = value;
            serializedObject.ApplyModifiedProperties();
        }

        private void TaskGUI() {
            EditorGUILayout.Space();
            this.m_TaskList.DoLayoutList();
            EditorGUILayout.Space();
            if (this.m_TaskList.index > -1)
            {
                SerializedProperty element = this.m_Tasks.GetArrayElementAtIndex(this.m_TaskList.index);

                SerializedProperty useTimeLimit = element.FindPropertyRelative("m_UseTimeLimit");
                SerializedProperty timeLimit = element.FindPropertyRelative("m_TimeLimit");


                foreach (var child in element.EnumerateChildProperties())
                {
                    if (child.propertyPath == timeLimit.propertyPath)
                    {
                        if (useTimeLimit.boolValue)
                        {
                            EditorGUI.indentLevel += 1;
                            EditorGUILayout.PropertyField(child, includeChildren: true);
                            EditorGUI.indentLevel -= 1;
                        }
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(child, includeChildren: true);
                    }
                }
            }
        }
      
        protected void ConditionGUI()
        {

            GUILayout.Space(10f);
            for (int i = 0; i < this.m_Conditions.arraySize; i++)
            {
                SerializedProperty condition = this.m_Conditions.GetArrayElementAtIndex(i);

                object value = this.m_List[i];
                EditorGUI.BeginChangeCheck();
                if (this.m_Target != null)
                    Undo.RecordObject(this.m_Target, "Quest Condition");

                if (EditorTools.Titlebar(value, ElementContextMenu(this.m_List, i)))
                {
                    EditorGUI.indentLevel += 1;
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField("Script", value != null ? EditorTools.FindMonoScript(value.GetType()) : null, typeof(MonoScript), true);
                    EditorGUI.EndDisabledGroup();
                    if (value == null)
                    {
                        EditorGUILayout.HelpBox("Managed reference values can't be removed or replaced. Only way to fix it is to recreate the renamed or deleted script file or delete and recreate the Item and rereference it in scenes. Unity throws an error: Unknown managed type referenced: [Assembly-CSharp] + Type which has been removed.", MessageType.Error);
                    }
                    if (EditorTools.HasCustomPropertyDrawer(value.GetType()))
                    {
                        EditorGUILayout.PropertyField(condition, true);
                    }
                    else
                    {
                        foreach (var child in condition.EnumerateChildProperties())
                        {
                            //Need to find a better way to disable TargetType on Item, it should be always Player   
                            EditorGUI.BeginDisabledGroup(child.name == "m_Target");
                            EditorGUILayout.PropertyField(
                                child,
                                includeChildren: true
                            );
                            EditorGUI.EndDisabledGroup();
                        }
                    }
                    EditorGUI.indentLevel -= 1;
                }
                if (EditorGUI.EndChangeCheck())
                    EditorUtility.SetDirty(this.m_Target);
            }
            GUILayout.FlexibleSpace();
            DoAddButton();
            GUILayout.Space(10f);

        }

        private void Add(Type type)
        {

            object value = System.Activator.CreateInstance(type);
            serializedObject.Update();
            this.m_Conditions.arraySize++;
            this.m_Conditions.GetArrayElementAtIndex(this.m_Conditions.arraySize - 1).managedReferenceValue = value;
            serializedObject.ApplyModifiedProperties();
        }

        private void CreateScript(string scriptName)
        {

        }

        private void DoAddButton()
        {
            GUIStyle buttonStyle = new GUIStyle("AC Button");
            GUIContent buttonContent = new GUIContent("Add Condition");
            Rect buttonRect = GUILayoutUtility.GetRect(buttonContent, buttonStyle, GUILayout.ExpandWidth(true));
            buttonRect.x = buttonRect.width * 0.5f - buttonStyle.fixedWidth * 0.5f;
            buttonRect.width = buttonStyle.fixedWidth;
            if (GUI.Button(buttonRect, buttonContent, buttonStyle))
            {
                AddObjectWindow.ShowWindow(buttonRect, this.m_ElementType, Add, CreateScript);
            }
        }

        private GenericMenu ElementContextMenu(IList list, int index)
        {

            GenericMenu menu = new GenericMenu();
            if (list[index] == null)
            {
                return menu;
            }
            menu.AddItem(new GUIContent("Reset"), false, delegate {

                object value = System.Activator.CreateInstance(list[index].GetType());
                list[index] = value;
            });
            menu.AddSeparator(string.Empty);
            menu.AddItem(new GUIContent("Remove " + this.m_ElementType.Name), false, delegate { list.RemoveAt(index); EditorUtility.SetDirty(target); });

            if (index > 0)
            {
                menu.AddItem(new GUIContent("Move Up"), false, delegate {
                    object value = list[index];
                    list.RemoveAt(index);
                    list.Insert(index - 1, value);
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Move Up"));
            }

            if (index < list.Count - 1)
            {
                menu.AddItem(new GUIContent("Move Down"), false, delegate
                {
                    object value = list[index];
                    list.RemoveAt(index);
                    list.Insert(index + 1, value);
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Move Down"));
            }

            menu.AddItem(new GUIContent("Copy " + this.m_ElementType.Name), false, delegate {
                object value = list[index];
                m_ObjectToCopy = value;
            });

            if (m_ObjectToCopy != null)
            {
                menu.AddItem(new GUIContent("Paste " + this.m_ElementType.Name + " As New"), false, delegate {
                    object instance = System.Activator.CreateInstance(m_ObjectToCopy.GetType());
                    FieldInfo[] fields = instance.GetType().GetSerializedFields();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        object value = fields[i].GetValue(m_ObjectToCopy);
                        fields[i].SetValue(instance, value);
                    }
                    list.Insert(index + 1, instance);
                });

                if (list[index].GetType() == m_ObjectToCopy.GetType())
                {
                    menu.AddItem(new GUIContent("Paste " + this.m_ElementType.Name + " Values"), false, delegate
                    {
                        object instance = list[index];
                        FieldInfo[] fields = instance.GetType().GetSerializedFields();
                        for (int i = 0; i < fields.Length; i++)
                        {
                            object value = fields[i].GetValue(m_ObjectToCopy);
                            fields[i].SetValue(instance, value);
                        }
                    });
                }
                else
                {
                    menu.AddDisabledItem(new GUIContent("Paste " + this.m_ElementType.Name + " Values"));
                }
            }

            if (list[index] != null)
            {
                MonoScript script = EditorTools.FindMonoScript(list[index].GetType());
                if (script != null)
                {
                    menu.AddSeparator(string.Empty);
                    menu.AddItem(new GUIContent("Edit Script"), false, delegate { AssetDatabase.OpenAsset(script); });
                }
            }
            return menu;
        }
    }
}