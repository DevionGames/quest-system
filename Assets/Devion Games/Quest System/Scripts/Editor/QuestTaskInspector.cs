using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames.QuestSystem
{

    [CustomEditor(typeof(QuestTask))]
    public class QuestTaskInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Label("Hi");
        }
    }
}