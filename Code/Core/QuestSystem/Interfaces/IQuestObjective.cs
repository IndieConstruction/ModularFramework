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
using System.Collections.Generic;
using ModularFramework.Core.RewardSystem;

namespace ModularFramework.Core.QuestSystem {

    public interface IQuestObjective {
        /// <summary>
        /// Parent quest auto injected during setup.
        /// </summary>
        IQuest ParentQuest { get; set; }

        /// <summary>
        /// Titolo dell'obbiettivo.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Descrizione dell'obbiettivo.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Collezione di IQuestItems da completare per completare l'obbiettivo.
        /// </summary>
        List<IQuestItem> ObjectiveItems { get; set; }

        /// <summary>
        /// Lista dei rewards riscattabili una volta completata il quest objective.
        /// </summary>
        List<IRewardBehaviour> Rewards { get; set; }

        /// <summary>
        /// Identifica se l'obbiettivo è completato. Viene impostata automaticamente.
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// Indica se l'obbiettivo è obbligatorio per il completamento della quest o se è facoltativo.
        /// </summary>
        bool IsMandatory { get; }

        /// <summary>
        /// Evento che viene invocato quando tutti i IQuest Items sono completati e quindi l'obbiettivo è da considerarsi completato.
        /// </summary>
        event IQuestObjectiveEvents.Event OnCompleted;

        /// <summary>
        /// Richiamata automaticamente al termine del setup.
        /// </summary>
        void OnSetupDone();

        /// <summary>
        /// Usare per eseguire le logica di update dell'obbiettivo.
        /// Richiamata automaticamente ad ogni IQuestItem completato.
        /// </summary>
        void UpdateProgress();

        /// <summary>
        /// Usare per eseguire la logica dicontrollo dello stato dell'obbiettivo.
        /// Richiamata automaticamente ad ogni IQuestItem completato. 
        /// </summary>
        void CheckProgress();
        
    }

    public static class IQuestObjectiveExtensions {
        /// <summary>
        /// Setups the specified quest. Auto called from ParentQuest and subscribe it to OnObjectiveCompleted event.
        /// </summary>
        /// <param name="_this">The this.</param>
        /// <param name="_quest">The quest.</param>
        public static void Setup(this IQuestObjective _this, IQuest _quest) {
            _this.ParentQuest = _quest;
            _this.OnCompleted += _quest.OnObjectiveCompleted;
            foreach (IQuestItem questItem in _this.ObjectiveItems) {
                questItem.OnCompleted += _this.OnItemCompleted;
            }
            _this.OnSetupDone();
        }

        /// <summary>
        /// Viene richiamata automaticamente (iscrizione durante il setup) ogni volta che un IQuestItem viene completato.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="_item"></param>
        static void OnItemCompleted(this IQuestObjective _this, IQuestItem _item) {
            _item.OnCompleted -= _this.OnItemCompleted;
            _item.IsCollected = true;
            _this.UpdateProgress();
            _this.CheckProgress();
        }

        // Cercare di rendere overridable...
        //public static void OnSetupDone(this IQuestObjective _this) {
        //}

    }

    public class IQuestObjectiveEvents {
        public delegate void Event(IQuestObjective _objective);
    }

}
