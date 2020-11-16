using UnityEngine;

namespace DevionGames.QuestSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Quest")]
    [ComponentMenu("Quest System/Has Quest")]
    public class HasQuest : Action, ICondition
    {
        [QuestPicker(true)]
        [SerializeField]
        protected Quest requiredQuest;
        [SerializeField]
        protected Status m_Status = Status.Completed;

        public override ActionStatus OnUpdate()
        {

            return QuestManager.current.HasQuest(requiredQuest, this.m_Status)?ActionStatus.Success: ActionStatus.Failure;
        }
    }
}