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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.Core.QuestSystem {
    [Serializable]
    public class QuestItem : MonoBehaviour, IQuestItem {

        /// <summary>
        /// Identifica
        /// </summary>
        public string TypeID {
            get { return _typeID; }
            set { _typeID = value; }
        }
        [SerializeField]
        protected string _typeID;

        public bool IsCollected { get; set; }

        public event IQuestItemEvents.Event OnCompleted;

        public virtual void Complete(IQuestItemUse questItemUse = null) {
            if(questItemUse != null)
                questItemUse.UseAction();
            IsCollected = true;
            if (OnCompleted != null)
                OnCompleted(this);
        }
    }

}