using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ModularFramework.Core.RewardSystem;

namespace ModularFramework.Core.QuestSystem {
    /// <summary>
    /// Quest objective with expect to collect a number of items to be completed. 
    /// </summary>
    /// <seealso cref="ModularFramework.QuestSystem.IQuestObjective" />
    public class CollectObjective : IQuestObjective {

        #region properties

        /// <summary>
        /// Titolo dell'obbiettivo.
        /// </summary>
        public string Title {
            get { return _title; }
            set { _title = value; }
        }
        private string _title;

        /// <summary>
        /// Descrizione dell'obbiettivo.
        /// </summary>
        public string Description {
            get { return _description; }
            set { _description = value; }
        }
        private string _description;

        /// <summary>
        /// Collezione di IQuest items che compongono questo obbiettivo.
        /// </summary>
        public List<IQuestItem> ObjectiveItems {
            get { return _itemsCollected; }
            set { _itemsCollected = value; }
        }
        private List<IQuestItem> _itemsCollected;

        /// <summary>
        /// Descrive se questo obbiettivo è obbligatorio per il completamento della quest.
        /// </summary>
        public bool IsMandatory { get { return true; } }

        /// <summary>
        /// Identifica se l'obbiettivo è completato. Viene impostata automaticamente.
        /// </summary>
        public bool IsComplete {
            get { return _isComplete; }
            set { _isComplete = value; }
        }
        private bool _isComplete;

        /// <summary>
        /// Parent quest auto injected during setup.
        /// </summary>
        public IQuest ParentQuest {
            get { return _parentQuest; }
            set { _parentQuest = value; }
        }
        private IQuest _parentQuest;

        /// <summary>
        /// Lista delle eventuali ricompense legate a questo obbiettivo.
        /// </summary>
        public List<IRewardBehaviour> Rewards { get; set; }

        /// <summary>
        /// Evento richiamato quando 
        /// </summary>
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