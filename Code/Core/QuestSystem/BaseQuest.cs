using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularFramework.Core.RewardSystem;


namespace ModularFramework.Core.QuestSystem {
    /// <summary>
    /// Base class for any type of quest:
    /// - 
    /// </summary>
    /// <seealso cref="ModularFramework.QuestSystem.IQuest" />
    public class BaseQuest : IQuest {

        private string _id;
        /// <summary>
        /// ID of quest.
        /// </summary>
        public string ID {
            get { return _id; }
            protected set { _id = value; }
        }

        private string _title;
        /// <summary>
        /// Title of Quest.
        /// </summary>
        public string Title {
            get { return _title; }
            set { _title = value; }
        }

        private List<IQuestObjective> _objectives;
        /// <summary>
        /// List of objectives to complete this quest.
        /// </summary>
        public List<IQuestObjective> Objectives {
            get { return _objectives; }
            protected set { _objectives = value; }
        }

        private List<IRewardBehaviour> _rewards;
        /// <summary>
        /// List of rewards for quest completion.
        /// </summary>
        public List<IRewardBehaviour> Rewards {
            get { return _rewards; }
            set { _rewards = value; }
        }

        public event IQuestEvents.Event OnCompleted;

        public BaseQuest(string id, string title, List<IQuestObjective> objectives, List<IRewardBehaviour> rewards) {
            ID = id;
            Title = title;
            SetupQuest(objectives);
            Rewards = rewards;
        }

        /// <summary>
        /// Setups the quest.
        /// </summary>
        /// <param name="objectives">The objectives.</param>
        public virtual void SetupQuest(List<IQuestObjective> objectives) {
            Objectives = objectives;
            foreach (var objective in Objectives) {
                objective.Setup(this);
            }
        }

        /// <summary>
        /// Called when a objective is completed. This behaviour unsubscribe from event subscription (then call this after the override logic).
        /// </summary>
        /// <param name="_objective">The objective.</param>
        public virtual void OnObjectiveCompleted(IQuestObjective objective) {
            objective.OnCompleted -= OnObjectiveCompleted;
            Debug.LogFormat("Objective {0} completed! ", objective.Title);
            if (Objectives.FindAll(o => o.IsComplete == false || o.IsMandatory == true).Count == 0) {
                // Quest completed
                RedeemAllRewards();
                if (OnCompleted != null)
                    OnCompleted(this);
            }
        }

        /// <summary>
        /// Riscatta tutti i rewards assegnati alla quest.
        /// </summary>
        private void RedeemAllRewards() {
            foreach (IRewardBehaviour reward in Rewards) {
                reward.Redeem();
            }
        }

    }
}
