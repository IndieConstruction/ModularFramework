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

namespace ModularFramework.Core.QuestSystem {
    public abstract class QuestObjectiveController<T> : BaseController<QuestObjectiveModel> {

        #region Controller Singleton

        private static bool instanced = false;

        new public static T Instance {
            get {
                if (instanced == false) {
                    _instance = Activator.CreateInstance<T>();
                    instanced = true;
                }
                return _instance;
            }
            private set { _instance = value; }
        }
        private static T _instance = default(T);

        #endregion

    }

}