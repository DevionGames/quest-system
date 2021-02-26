using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.QuestSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Quest")]
    [ComponentMenu("Quest System/UpdateQuestIndicators")]
    [System.Serializable]
    public class UpdateQuestIndicators : Action
    {

        public override ActionStatus OnUpdate()
        {
            QuestIndicator[] indicators = GameObject.FindObjectsOfType<QuestIndicator>();
            for (int i = 0; i < indicators.Length; i++) {
                indicators[i].UpdateQuestIndicator();
            }
            return ActionStatus.Success;
        }
    }
}