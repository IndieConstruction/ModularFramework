/* --------------------------------------------------------------
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
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System.Collections.Generic;
using ModularFramework.Modules;
using System.Linq;

namespace ModularFramework.Core
{
    /// <summary>
    /// Base GameManager class for creating own derived GameManager class.
    /// Override 'GameSetup' function in derived class (and call base.GameSetup inside) and use it a game entry point.
    /// </summary>
    [ScriptExecutionOrder(-8000)]
    public abstract class GameManager : Singleton<GameManager> {

        #region Game Settings

#if UNITY_EDITOR

        private static GameManager _instance;

        public new static GameManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<GameManager>();
                    _instance.GameSetup(); 
                }
                return _instance;
            }
            set { _instance = value; }
        }

#endif

    public GameSettings GameSettings = new GameSettings();
        [Tooltip("To add new SubGame enable 'MultiGame' (if disabled) and add it to list")]
        public bool MultiGame = false;
        public List<GameSettings> Games = new List<GameSettings>();

        #endregion

        public static GameplayEventsManager GameplayEvents;

        [HideInInspector] 
        public ModuleManager Modules = new ModuleManager();
        [HideInInspector]
        protected GameSettings ActualSubGame = new GameSettings();
        /// <summary>
        /// Prevent multiple setup.
        /// Setted to true after first setup.
        /// </summary>

        private bool _setupped = false; 
        /// <summary>
        /// Avoid duplicate setup.
        /// </summary>
        protected bool setuped {
            get { return _setupped; }
            set {
                _setupped = value;
            } 
        }


        #region Managers

        protected List<IManager> managers = new List<IManager>();

        /// <summary>
        /// Aggiunge il manager del tipo indicato nella lista dei manager solo se non è già presente.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_managerToAdd"></param>
        /// <returns></returns>
        public T AddManager<T>(T _managerToAdd) where T : IManager {
            T manager = GetManager<T>();
            if (manager == null) {
                managers.Add(_managerToAdd);
                return _managerToAdd;
            } else {
                Debug.LogFormat("Manager {0} already present.", manager);
                return manager;
            }
        }

        /// <summary>
        /// Restituisce il manager del tipo richiesto, se presente nella lista dei manager, altrimenti restituisce null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T GetManager<T>() where T : IManager {
            T manager = managers.OfType<T>().FirstOrDefault();
            if (manager == null) {
                // Debug.LogErrorFormat("Manager {0} not found in list of managers", manager);
            }
            return manager;
        }

        #endregion

        #region Events

        public delegate void GameEvent(IGameplayInfo _gameplayInfo);

        //public static event GameEvent LevelSet;

        #endregion

        #region Event Handlers
        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
            if (IsDuplicatedInstance)
                return;
            Debug.LogFormat("SceneLoaded {0}",SceneManager.GetActiveScene().name);
            Modules.SceneModule.SceneLoadedBehaviour();
        }
        #endregion

        #region Event Subscription

        // TODO: https://trello.com/c/fxKJ7DsI
        void OnEnable() {
            // SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        void OnDisable() {
            // SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }



        #endregion

        #region ShortCuts properties

        public UIModule UIModule
        {
            get { return Modules.UIModule; }
        }

        public PlayerProfileModule PlayerProfile
        {
            get { return Modules.PlayerProfile; }
        }

        public LocalizationModule Localization
        {
            get { return Modules.LocalizationModule; }
        }

        #endregion

        #region Game Setup

        /// <summary>
        /// Game entry point.
        /// Override this in derived class to use your own code (always call before base.GameSetup).
        /// This function automatically call Modules Setup and module auto installer override.
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

        protected override void Awake() {
            DontDestroyOnLoad(this);
            // Assert.IsTrue(!string.IsNullOrEmpty(GameSettings.GameID), "Main Game ID Can not be null or empty");
            base.Awake();
        }


        #endregion

        #region MultiGame

        /// <summary>
        /// Return actual active game.
        /// If not multigame, return main game.
        /// </summary>
        /// <returns></returns>
        public GameSettings GetActualGame()
        {
            if (MultiGame)
                return ActualSubGame;
            else
                return GameSettings;
        }

        /// <summary>
        /// Set as active game, game with id equal to parameter (if exist), otherwise set the main id.
        /// </summary>
        /// <param name="_gameID"></param>
        public void SetActiveGame(string _gameID)
        {
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
        public void SetMainActiveGame()
        {
            ActualSubGame = GameSettings;
        }

        #endregion

    }
}
