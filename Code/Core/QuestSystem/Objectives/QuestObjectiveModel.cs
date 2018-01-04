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
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModularFramework.Core.QuestSystem {
    [Serializable]
    public abstract class QuestObjectiveModel : BaseModel {

        #region serializable variables

        /// <summary>
        /// Titolo dell'obbiettivo.
        /// </summary>
        public string Title;

        /// <summary>
        /// Descrizione dell'obbiettivo.
        /// </summary>
        public string Description;

        /// <summary>
        /// Collezione di IQuest items che compongono questo obbiettivo.
        /// </summary>
        public List<IQuestItem> ObjectiveItems;

        /// <summary>
        /// Descrive se questo obbiettivo è obbligatorio per il completamento della quest.
        /// </summary>
        public bool IsMandatory;

        /// <summary>
        /// True se è completato.
        /// </summary>
        public bool IsComplete { get; set; }

        #endregion

        #region non serializable properties

        // QuestObjectiveModel non serializable properties here...

        #endregion

    }

}