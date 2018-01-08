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
    /// Definisce una risorsa per il quest system.
    /// Può essere utilizzato sia come risorsa generale della quest che come risorsa dell'objective.
    /// </summary>
    public interface IQuestResource {

        /// <summary>
        /// Funzione public da richiamare per eseguire un utilizzo di questa risorsa.
        /// Inserire quì le azioni che effettueranno fisicamente l'utilizzo.
        /// </summary>
        void Use();

        /// <summary>
        /// Evento scatenato all'utilizzo della risorsa.
        /// </summary>
        event IQuestResourceEvents.Event OnUse;

        /// <summary>
        /// Evento scatenato all'esaurimento della risorsa.
        /// </summary>
        event IQuestResourceEvents.Event OnOver;

    }

    public static class IQuestResourceEvents {
        /// <summary>
        /// Dichiarazione della tipologia di delegato.
        /// </summary>
        /// <param name="_item">The item.</param>
        public delegate void Event(IQuestResource _item);
    }
}