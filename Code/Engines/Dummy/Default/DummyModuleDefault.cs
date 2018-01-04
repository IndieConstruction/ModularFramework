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
using UnityEngine;
using System.Collections;
using ModularFramework.Core;

namespace ModularFramework.Modules {

    public class DummyModuleDefault : iDummyModule {

        #region Interface
        public iDummyModule ConcreteModuleImplementation { get; set; }
        public IModuleSettings Settings { get; set; }
        public int LevelId { get; set; }
        public int LevelProgression { get; set; }
        public int LevelGoal { get; set; }
        
        public iDummyModule SetupModule(iDummyModule _concreteModule, IModuleSettings _settings = null) {
            Settings = _settings;
            return this;
        }
        #endregion

        public void LevelStarted() {
            Debug.Log("Default DummyModule LevelStart Functionality");
        }

        public void LevelFinisched() {
            defaultLevelFinishedCelebration();
        }

        void defaultLevelFinishedCelebration() {
            Debug.Log("Wow, finished Default DummyModule!");
        }
    }
}
