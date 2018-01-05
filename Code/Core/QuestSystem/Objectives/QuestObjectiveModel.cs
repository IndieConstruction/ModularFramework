/* --------------------------------------------------------------
*   Indie Contruction : Modular Framework for Unity
*   Copyright(c) 2018 Indie Construction / Paolo Bragonzi
*   All rights reserved. 
*   For any information refer to http://www.indieconstruction.com
*   
*   This library is free software; you can redistribute it and/or
*   modify it under the terms of the GNU Lesser General Public
*   License as published by the Free Software Foundation; either
*   version 3.0 of the License, or(at your option) any later version.
*   
*   This library is distributed in the hope that it will be useful,
*   but WITHOUT ANY WARRANTY; without even the implied warranty of
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
*   Lesser General Public License for more details.
*   
*   You should have received a copy of the GNU Lesser General Public
*   License along with this library.
* -------------------------------------------------------------- */
using UnityEngine;
using ModularFramework.Core.RewardSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModularFramework.Core.QuestSystem {
    [Serializable]
    public abstract class QuestObjectiveModel : BaseModel, IQuestObjective {

        #region serializable variables

        /// <summary>
        /// Titolo dell'obbiettivo.
        /// </summary>
        public string Title {
            get { return _title; }
            set { _title = value; }
        }
        [SerializeField] private string _title;

        /// <summary>
        /// Descrizione dell'obbiettivo.
        /// </summary>
        public string Description {
            get { return _description; }
            set { _description = value; }
        }
        [SerializeField] private string _description;


        /// <summary>
        /// Descrive se questo obbiettivo è obbligatorio per il completamento della quest.
        /// </summary>
        public bool IsMandatory {
            get { return _isMandatory; }
            set { _isMandatory = value; }
        }
        [SerializeField] private bool _isMandatory;


        /// <summary>
        /// Collezione di IQuest items che compongono questo obbiettivo.
        /// </summary>
        public List<IQuestItem> ObjectiveItems {
            get { return _objectiveItems; }
            set { _objectiveItems = value; }
        }
        [SerializeField] private List<IQuestItem> _objectiveItems;


        #endregion

        #region non serializable properties

        // QuestObjectiveModel non serializable properties here...
        
        /// <summary>
        /// Parent Quest.
        /// </summary>
        public IQuest ParentQuest {
            get { return _parentQuest; }
            set { _parentQuest = value; }
        }
        private IQuest _parentQuest;

        /// <summary>
        /// True se è completato.
        /// </summary>
        public bool IsComplete {
            get { return _isComplete; }
            private set { _isComplete = value; }
        }
        private bool _isComplete;

        /// <summary>
        /// Lista dei rewards che si otterranno al completamento della quest.
        /// </summary>
        public List<IRewardBehaviour> Rewards {
            get { return _rewards; }
            set { _rewards = value; }
        }
        private List<IRewardBehaviour> _rewards;

        public event IQuestObjectiveEvents.Event OnCompleted;

        /// <summary>
        /// Richiamata automaticamente al termine del setup.
        /// Se non sovrascritta non esegue nulla di aggiuntivo al setup.
        /// </summary>
        public virtual void OnSetupDone() { }

        /// <summary>
        /// Usare per eseguire le logica di update dell'obbiettivo.
        /// Se non sovrascritta non esegue nulla di aggiuntivo al progress.
        /// Al contrario la funzione interna si occupa di gestire l'item appena completato.
        /// Richiamata automaticamente ad ogni IQuestItem completato.
        /// </summary>
        public virtual void UpdateProgress() { }

        /// <summary>
        /// Usare per eseguire la logica dicontrollo dello stato dell'obbiettivo. 
        /// Se non sovrascritta esegue il controllo che non sia già completato l'obbiettivo e che non ci siano items ancora da raccogliere. In caso contrario esegue la funzione "Complete()".
        /// In caso di Override, in caso di check positivo, richiamare Complete().
        /// Richiamata automaticamente ad ogni IQuestItem completato. 
        /// </summary>
        public virtual void CheckProgress() {
            if (!IsComplete && ObjectiveItems.FindAll(i => i.IsCollected == false).Count == 0) {
                Complete();
            }
        }

        /// <summary>
        /// Funzione da richiamare per dichiarare completato l'obbiettivo.
        /// </summary>
        protected void Complete() {
            IsComplete = true;
            if (OnCompleted != null)
                OnCompleted(this);
        }

        #endregion

    }

}