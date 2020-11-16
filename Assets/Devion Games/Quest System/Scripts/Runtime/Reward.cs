using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.QuestSystem
{
    [System.Serializable]
    public abstract class Reward
    {
        [SerializeField]
        protected GameObject m_DisplayRewardPrefab;

        public abstract bool GiveReward();
        public virtual void DisplayReward(RectTransform parent, int order) {
            GameObject go = GameObject.Instantiate(this.m_DisplayRewardPrefab);
            go.transform.SetParent(parent, false);
            go.transform.SetSiblingIndex(order);
            DisplayReward(go);
        }

        public virtual void DisplayReward(GameObject reward) { }

    }
}