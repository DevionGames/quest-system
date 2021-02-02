using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.QuestSystem
{
    [System.Serializable]
    public class QuestTask : IJsonSerializable
    {
        [HideInInspector]
        [SerializeField]
        protected string m_Name = "New Task";
        public string Name {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }
        [HeaderLine("General")]
        [TextArea(4, 4)]
        [SerializeField]
        protected string m_Description;
        public string Description
        {
            get { return this.m_Description; }
            set { this.m_Description = value; }
        }

        [SerializeField]
        protected float m_RequiredProgress=1f;
        public float RequiredProgress
        {
            get { return this.m_RequiredProgress; }
        }

        [SerializeField]
        protected string m_ProgressMessage = "{0}/{1}";
        public string ProgressMessage
        {
            get
            {
                return string.Format(this.m_ProgressMessage, this.Progress, this.m_RequiredProgress);
            }
        }

        [HeaderLine("Limits")]
        [SerializeField]
        protected bool m_UseTimeLimit = false;
        public bool UseTimeLimit {
            get { return this.m_UseTimeLimit; }
        }

        [SerializeField]
        protected float m_TimeLimit = 300f;
        public float TimeLimit {
            get { return this.m_TimeLimit; }
        }

        protected float m_RemainingTime;
        public float RemainingTime {
            get { return this.m_RemainingTime; }
        }

        protected bool m_TimerStarted = false;

        [SerializeField]
        protected bool m_Optional = false;
        public bool Optional
        {
            get { return this.m_Optional; }
        }

        [System.NonSerialized]
        public Quest owner;

        protected Status m_Status;
        public Status Status {
            get { return this.m_Status; }
            protected set {
                if (this.m_Status != value)
                {
                    this.m_Status = value;
                    owner.NotifyTaskStatusChange(this);
                }
            }
        }

        protected float m_Progress;
        public float Progress {
            get { return this.m_Progress; }
        }

        public void AddProgress(float progress) {
            Debug.Log(this.RequiredProgress + " " + this.m_Progress + " " + progress);
            SetProgress(this.m_Progress + progress);
        }

        public void SetProgress(float progress) {
            if (this.m_Progress != progress && Status == Status.Active)
            {
                this.m_Progress = progress;
                owner.NotifyTaskProgressChange(this);
                Complete();
            }
           
        }

        public virtual void Activate() {
            this.m_RemainingTime = this.m_TimeLimit;
            Status = Status.Active;
            if (this.UseTimeLimit)
                StartTimer();
        }

        private void StartTimer() {
            UnityTools.StartCoroutine(Timer());
        }

        private IEnumerator Timer() {

            while (this.m_RemainingTime > 0f && Status == Status.Active)
            {
                yield return new WaitForSeconds(1f);
                this.m_RemainingTime -= 1f;
                this.owner.NotifyTaskTimerTick(this);
            }
            if (this.m_RemainingTime <= 0f)
                Status = Status.Failed;
        }

        public virtual bool CanComplete() {
            if (Status != Status.Active) return false;

            if (this.m_Progress < this.m_RequiredProgress) return false;

            return true;
        }

        public virtual void Complete() {
            if (!CanComplete()){
                Status = Status.Active;
                return;
            }
            Status = Status.Completed;
        }

        public virtual void Reset() {
            this.m_Progress = 0f;
            Status = Status.Inactive;
        }

        public virtual void OnQuestCompleted() { }

        public virtual void GetObjectData(Dictionary<string, object> data)
        {
            data.Add("Name", Name);
            data.Add("Status", (int)Status);
            data.Add("Progress",Progress);
            data.Add("RemainingTime", RemainingTime);

        }

        public virtual void SetObjectData(Dictionary<string, object> data)
        {
          
            this.m_Status = (Status)data["Status"];

            this.m_Progress = System.Convert.ToSingle(data["Progress"]);
            this.m_RemainingTime = System.Convert.ToSingle(data["RemainingTime"]);
            if (this.UseTimeLimit)
                StartTimer();
        }
    }

}