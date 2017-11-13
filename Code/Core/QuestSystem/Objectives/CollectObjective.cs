using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ModularFramework.Core.RewardSystem;

namespace ModularFramework.QuestSystem {
    /// <summary>
    /// Quest objective with expect to collect a number of items to be completed. 
    /// </summary>
    /// <seealso cref="ModularFramework.QuestSystem.IQuestObjective" />
    public class CollectObjective : IQuestObjective {

        #region properties

        private string _title;

        public string Title {
            get { return _title; }
            set { _title = value; }
        }

        private string _description;

        public string Description {
            get { return _description; }
            set { _description = value; }
        }

        private List<IQuestItem> _itemsCollected;

        public List<IQuestItem> ObjectiveItems {
            get { return _itemsCollected; }
            set { _itemsCollected = value; }
        }

        public bool IsMandatory { get { return true; } }

        private bool _isComplete;

        public bool IsComplete {
            get { return _isComplete; }
            set { _isComplete = value; }
        }

        private IQuest _parentQuest;

        public IQuest ParentQuest {
            get { return _parentQuest; }
            set { _parentQuest = value; }
        }

        public List<IRewardBehaviour> Rewards { get; set; }



        public event IQuestObjectiveEvents.Event OnCompleted;
        #endregion

        public CollectObjective(string title, string description, List<IQuestItem> items) {
            Title = title;
            Description = description;
            ObjectiveItems = items;
        }

        public void CheckProgress() {
            if (!IsComplete && ObjectiveItems.FindAll(i => i.IsCollected == false).Count == 0) {
                IsComplete = true;
                if (OnCompleted != null)
                    OnCompleted(this);
            }
        }

        public void OnSetupDone() {

        }

        public void UpdateProgress() {
            
        }


    }

}