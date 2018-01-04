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
using UnityEngine;
using System;
using System.Collections;
using ModularFramework.Core;

/// <summary>
/// Modules System with strategy pattern implementation.
/// </summary>
namespace ModularFramework.Modules {
    /// <summary>
    /// This is the Context implementation of this Module type with all functionalities provided from the Strategy interface.
    /// </summary>
    public class GameplayModule : IGameplayModule {

        #region IModule implementation
        /// <summary>
        /// Concrete Module Implementation.
        /// </summary>
        public IGameplayModule ConcreteModuleImplementation { get; set; }
        public IModuleSettings Settings { get; set; }

        /// <summary>
        /// Module Setup.
        /// </summary>
        /// <param name="_concreteModule">Concrete module implementation to set as active module behaviour.</param>
        /// <returns></returns>
        public IGameplayModule SetupModule(IGameplayModule _concreteModule, IModuleSettings _settings = null) {
            ConcreteModuleImplementation = _concreteModule.SetupModule(_concreteModule, _settings);
            return ConcreteModuleImplementation;
        }

        #endregion

        #region Properties

        public IGameplayInfo ActualGameplayInfo {
            get { return ConcreteModuleImplementation.ActualGameplayInfo; }
            set { ConcreteModuleImplementation.ActualGameplayInfo = value; }
        }

        #endregion

        #region API

        public void GameplayResult(IGameplayResult _result) {
            ConcreteModuleImplementation.GameplayResult(_result);
        }

        public void GameplayStart(IGameplayInfo _gameplayInfo) {
            ConcreteModuleImplementation.GameplayStart(_gameplayInfo);
        }

        #endregion
    }

    /// <summary>
    /// Strategy interface. 
    /// Provide All the functionalities required for any Concrete implementation of the module.
    /// </summary>
    public interface IGameplayModule : IModule<IGameplayModule> {
        IGameplayInfo ActualGameplayInfo { get; set; }
        void GameplayResult(IGameplayResult _result);
        void GameplayStart(IGameplayInfo _gameplayInfo);
    }

    /// <summary>
    /// Interface for gameplay result data.
    /// </summary>
    public interface IGameplayResult {
        IGameplayInfo GameplayInfo { get; set; }
    }

    /// <summary>
    /// Interface for gameplay info data.
    /// </summary>
    public interface IGameplayInfo {

    }
}