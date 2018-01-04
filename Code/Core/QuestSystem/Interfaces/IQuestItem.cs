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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.Core.QuestSystem {

    /// <summary>
    /// Interface for any quest item.
    /// </summary>
    public interface IQuestItem {

        /// <summary>
        /// Non è necessario settare questa proprietà, *verrà settata in automatico una volta completato*.
        /// </summary>
        bool IsCollected { get; set; }
        
        /// <summary>
        /// Funzione che definisce le operazioni necessarie per "completare" l'elemento.
        /// </summary>
        /// <param name="questItemUse"></param>
        void Complete(IQuestItemUse questItemUse);

        /// <summary>
        /// Evento da richiamare per notificare che è terminata la funzione dell'item.
        /// L'interfaccia obbliga la dichiarazione dell'evento, nella classe implementante devono essere specificate le modalità con cui avviene questo evento.
        /// </summary>
        event IQuestItemEvents.Event OnCompleted;

    }

    public static class IQuestItemExtensions {
        
    }

    public static class IQuestItemEvents {
        /// <summary>
        /// Dichiarazione della tipologia di delegato.
        /// </summary>
        /// <param name="_item">The item.</param>
        public delegate void Event(IQuestItem _item);
    }
}