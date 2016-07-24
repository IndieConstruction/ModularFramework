﻿/* --------------------------------------------------------------
*   Indie Contruction : Modular Framework for Unity
*   Copyright(c) 2016 Indie Construction / Paolo Bragonzi
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
using UnityEngine.Assertions;
using System.Collections.Generic;
using ModularFramework.Modules;

namespace ModularFramework.Core {
    public class GameManager : Singleton<GameManager> {

        #region Game Settings
        public GameSettings GameSettings = new GameSettings();
        [Tooltip("To add new SubGame enable 'MultiGame' (if disabled) and add it to list")]
        public bool MultiGame = false;
        public List<GameSettings> Games = new List<GameSettings>();
        #endregion

        [HideInInspector]
        public ModuleManager Modules = new ModuleManager();
        [HideInInspector]
        protected GameSettings ActualSubGame = new GameSettings();
        public Transform UIContainer;
        /// <summary>
        /// Prevent multiple setup.
        /// Setted to true after first setup.
        /// </summary>
        bool setuped = false;

        #region ShortCuts properties

        public UIModule UIModule {
            get { return Modules.UIModule; }
        }

        public PlayerProfileModule PlayerProfile {
            get { return Modules.PlayerProfile; }
        }

        public LocalizationModule Localization {
            get { return Modules.LocalizationModule; }
        }

        #endregion

        #region Game Setup

        /// <summary>
        /// Game entry point.
        /// </summary>
        protected override void GameSetup() {
            if (setuped)
                return;
            base.GameSetup();
            // Modules Setup
            Modules.ModulesSetup();
            Modules.ModuleAutoInstallerOverride(this.gameObject);
            // Set as active the main game
            SetMainActiveGame();
            setuped = true;
        }

        void Start() {
            DontDestroyOnLoad(this);
            Assert.IsTrue(!string.IsNullOrEmpty(GameSettings.GameID), "Main Game ID Can not be null or empty");
        }
        #endregion

        #region MultiGame

        /// <summary>
        /// Return actual active game.
        /// If not multigame, return main game.
        /// </summary>
        /// <returns></returns>
        public GameSettings GetActualGame() {
            if (MultiGame)
                return ActualSubGame;
            else
                return GameSettings;
        }
        /// <summary>
        /// Set as active game, game with id equal to parameter (if exist), otherwise set the main id.
        /// </summary>
        /// <param name="_gameID"></param>
        public void SetActiveGame(string _gameID) {
            if (!MultiGame)
                return;
            GameSettings gameFound = Games.Find(g => g.GameID == _gameID);
            if (gameFound == null)
                Debug.LogErrorFormat("Sub Game with id {0} not found.", _gameID);
            else
                ActualSubGame = gameFound;
        }
        /// <summary>
        /// Set Main Game as active game.
        /// </summary>
        public void SetMainActiveGame() {
            ActualSubGame = GameSettings;
        }

        #endregion

    }
}