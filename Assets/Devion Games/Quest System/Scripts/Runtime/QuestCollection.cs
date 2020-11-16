using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DevionGames.QuestSystem
{
    public class QuestCollection : MonoBehaviour, IEnumerable<Quest>
    {
        [QuestPicker(true)]
        [SerializeField]
        protected List<Quest> m_Quests = new List<Quest>();
      
        [HideInInspector]
        public UnityEvent onChange;

        private bool m_Initialized;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (this.m_Initialized) { return; }
            for (int i = 0; i < this.m_Quests.Count; i++)
            {
                Quest instance;
                if (!QuestManager.current.HasQuest(this.m_Quests[i], out instance))
                {
                    instance = Instantiate(this.m_Quests[i]);
                }
                this.m_Quests[i] = instance;

                for (int j = 0; j < instance.tasks.Count; j++) {
                    instance.tasks[j].owner = this.m_Quests[i];
                }
            }
            this.m_Initialized = true;
        }

        public Quest this[int index]
        {
            get { return this.m_Quests[index]; }
            set
            {
                Insert(index, value);
                if (onChange != null)
                    onChange.Invoke();
            }
        }

        public int Count
        {
            get
            {
                return this.m_Quests.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return m_Quests.Count == 0;
            }
        }

        public IEnumerator<Quest> GetEnumerator()
        {
            return this.m_Quests.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(Quest[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Add(items[i]);
            }
        }

        public void Add(Quest item)
        {
            this.m_Quests.Add(item);
            if (onChange != null)
                onChange.Invoke();

        }

        public bool Remove(Quest item)
        {
            bool result = this.m_Quests.Remove(item);
            if (onChange != null && result)
                onChange.Invoke();

            return result;
        }

        public void Insert(int index, Quest child)
        {
            this.m_Quests.Insert(index, child);
            if (onChange != null)
                onChange.Invoke();
        }

        public void RemoveAt(int index)
        {
            this.m_Quests.RemoveAt(index);
            if (onChange != null)
                onChange.Invoke();
        }

        public void Clear()
        {
            this.m_Quests.Clear();
            if (onChange != null)
                onChange.Invoke();
        }

        public void GetObjectData(Dictionary<string, object> data)
        {
           
        }

        public void SetObjectData(Dictionary<string, object> data)
        {
        
        }
    }
}