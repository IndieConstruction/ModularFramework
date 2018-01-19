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
using ModularFramework.Core.RewardSystem;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.Core.QuestSystem {

    /// <summary>
    /// Classe base per tutte le view di un quest objective.
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="C"></typeparam>
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

        public new IQuestObjectiveData Model { get; set; }

        public event IQuestObjectiveEvents.Event OnCompleted;

        #endregion

        protected override void addictionalSetup(ISetupSettings _settings) {
            extraSettings = _settings as ExtraSettings;

            // addictional view setup logic here...
        }

        /// <summary>
        /// Eseguire l'override di questa funzione per specificare come riempire la collezione degli objective items.
        /// Se non si esegue l'override verrà utilizzata la collezione del model ObjectiveItems attuale.
        /// </summary>
        /// <returns></returns>
        public virtual List<IQuestItem> SetQuestItems() {
            return Model.ObjectiveItems;
        }

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
            if (!Model.IsComplete && Model.ObjectiveItems.FindAll(i => i.IsCollected == false).Count == 0) {
                Complete();
            }
        }

        /// <summary>
        /// Funzione da richiamare per dichiarare completato l'obbiettivo.
        /// </summary>
        protected void Complete() {
            Model.IsComplete = true;
            if (OnCompleted != null)
                OnCompleted(this);
        }

        #endregion

    }

}