using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DevionGames.QuestSystem.Configuration
{
    [System.Serializable]
    public class QuestSettingsEditor : ScriptableObjectCollectionEditor<Settings>
    {
        public override string ToolbarName
        {
            get
            {
                return "Settings";
            }
        }

        protected override bool CanAdd
        {
            get
            {
                return false;
            }
        }

        protected override bool CanRemove
        {
            get
            {
                return false;
            }
        }

        public QuestSettingsEditor(UnityEngine.Object target, List<Settings> items) : base(target, items)
        {
            this.target = target;
            this.items = items;
           

            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => typeof(Settings).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToArray();

            foreach (Type type in types)
            {
                if (Items.Where(x => x.GetType() == type).FirstOrDefault() == null)
                {
                    CreateItem(type);
                }
            }
        }

        protected override string ButtonLabel(int index, Settings item)
        {
            return "  " + GetSidebarLabel(item);
        }
    }
}