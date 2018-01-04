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
using ModularFramework.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.Core.QuestSystem {

    public class QuestSystemManager : MonoBehaviour {

        /// <summary>
        /// Crea e istanzia una quest objective view.
        /// </summary>
        public IQuestObjective CreateQuestObjectiveView<T>(ISetupSettings _setupSettings) where T : IQuestObjective {
            //T newCollectQuestObjectiveView = GenericHelper.InstantiateAndSetup<T>(
            //    ObjectivePrefab,
            //    gameObject,
            //    _setupSettings
            //); // Create new CollectQuestObjectiveView instance and call setup with extra settings parameters
            return null;
        }

    }
}