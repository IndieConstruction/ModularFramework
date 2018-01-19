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

namespace ModularFramework.Core.QuestSystem {

    public interface IQuestObjectiveData {

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
        bool IsComplete { get; set; }

        /// <summary>
        /// Indica se l'obbiettivo è obbligatorio per il completamento della quest o se è facoltativo.
        /// </summary>
        bool IsMandatory { get; }

    }

}
