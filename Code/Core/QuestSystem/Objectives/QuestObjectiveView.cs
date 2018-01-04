﻿/* --------------------------------------------------------------
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
using ModularFramework.Core.RewardSystem;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.Core.QuestSystem {

    public abstract class QuestObjectiveView<M, C> : BaseView<M, C>, IQuestObjective
                                            where M : QuestObjectiveModel
                                            where C : QuestObjectiveController<M> {

        #region setup

        // Remove this regione if not needed and remove reference to extraSettings in addictionalSetup
        #region Etra Setting extensions

        public class ExtraSettings : Settings {
            // add custom extra settings here...
        }
        /// <summary>
        /// Etra Setting extensions.
        /// </summary>
        protected ExtraSettings extraSettings;

        #endregion

        protected override void addictionalSetup(ISetupSettings _settings) {
            extraSettings = _settings as ExtraSettings;

            // addictional view setup logic here...
        }

        #endregion

        #region IQuestObjective

        /// <summary>
        /// Titolo dell'obbiettivo.
        /// </summary>
        public string Title {
            get { return Model.Title; }
            set { Model.Title = value; }
        }

        /// <summary>
        /// Descrizione dell'obbiettivo.
        /// </summary>
        public string Description {
            get { return Model.Description; }
            set { Model.Description = value; }
        }

        /// <summary>
        /// Collezione di IQuest items che compongono questo obbiettivo.
        /// </summary>
        public List<IQuestItem> ObjectiveItems {
            get { return Model.ObjectiveItems; }
            set { Model.ObjectiveItems = value; }
        }

        /// <summary>
        /// Descrive se questo obbiettivo è obbligatorio per il completamento della quest.
        /// </summary>
        public bool IsMandatory {
            get { return true; }
        }

        /// <summary>
        /// Identifica se l'obbiettivo è completato. Viene impostata automaticamente.
        /// </summary>
        public bool IsComplete {
            get { return Model.IsComplete; }
            set { Model.IsComplete = value; }
        }

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

        /// <summary>
        /// Controlla lo stato di avanzamento dell'obbiettivo e se diventa completato, dichiara il completamento.
        /// </summary>
        public virtual void CheckProgress() {
            if (!IsComplete && ObjectiveItems.FindAll(i => i.IsCollected == false).Count == 0) {
                IsComplete = true;
                if (OnCompleted != null)
                    OnCompleted(this);
            }
        }

        public virtual void OnSetupDone() {

        }

        public virtual void UpdateProgress() {

        }

        #endregion

    }

}