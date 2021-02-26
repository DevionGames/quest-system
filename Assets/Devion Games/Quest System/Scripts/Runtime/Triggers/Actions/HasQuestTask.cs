using UnityEngine;

namespace DevionGames.QuestSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Quest")]
    [ComponentMenu("Quest System/Has Quest Task")]
    [System.Serializable]
    public class HasQuestTask : Action, ICondition
    {
        [QuestPicker(true)]
        [SerializeField]
        protected Quest m_Quest;
        [SerializeField]
        protected string m_Task="";
        [SerializeField]
        protected Status m_Status = Status.Completed;

        public override ActionStatus OnUpdate()
        {
            Quest instance;
            if (QuestManager.current.HasQuest(this.m_Quest, out instance)) {
                return instance.tasks.Find(x => x.Name == this.m_Task && x.Status == this.m_Status) != null ? ActionStatus.Success : ActionStatus.Failure;
            }
            return ActionStatus.Failure;
        }
    }
}