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
using ModularFramework.Core;

namespace ModularFramework.Modules {

    public class SceneModuleDefaultInstaller : ModuleInstaller<ISceneModule> {
        public SceneModuleDefaultSettings settings;
        public override ISceneModule InstallModule() {
            var concreteInstance = new SceneModuleDefault() { Settings = settings };
            if (concreteInstance != null)
                ModuleActivationState = ModuleActivationStates.Activated;
            return concreteInstance;
        }
    }
}

/// <summary>
/// Settings for this implementation of SceneModule.
/// </summary>
[Serializable]
public class SceneModuleDefaultSettings : IModuleSettings {
    // Add Here setup settings properties (variables if you want to enable in editor)
}