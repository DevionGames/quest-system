using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevionGames.UIWidgets;
using UnityEngine;

namespace DevionGames.QuestSystem
{
    [Icon("Quest")]
    [ComponentMenu("Quest System/Show Window")]
    public class AddProgress : Action
    {
        [QuestPicker(true)]
        [SerializeField]
        protected Quest quest;
        [InspectorLabel("Task")]
        [SerializeField]
        protected string m_TaskName;
        [SerializeField]
        protected float m_Progress = 1f;

        public override ActionStatus OnUpdate()
        {
            Quest current = QuestManager.current.GetQuest(quest.Name);

            if (current != null && current.Status== Status.Active)
            {
                QuestTask task = current.tasks.FirstOrDefault(x => x.Name == m_TaskName);
                if (task.Status == Status.Active)
                {
                    task.AddProgress(this.m_Progress);
                    return ActionStatus.Success;
                }
            }
            Debug.Log("Fail");
            return ActionStatus.Failure;
        }


    }
}
