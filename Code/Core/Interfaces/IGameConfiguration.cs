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
namespace ModularFramework.Core {

    /// <summary>
    /// Common interface to implement game configuration file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGameConfiguration<T> where T : GameManager {
        /// <summary>
        /// Reference to base game manager.
        /// </summary>
        T GM { get; }

        /// <summary>
        /// Function containing all the necessary code for game configuration.
        /*
        <![CDATA[
        #region LevelManager
        LevelManagerConfig levelManagerConfig = new LevelManagerConfig() {
            // Add config parameters here:
            LevelTemplatesResourcesPath = "Levels/Templates/",
            LevelStageTemplatesResourcesPath = "Levels/StagePrefabs/",
        };
        LevelManager levelManager = new LevelManager().Setup(levelManagerConfig) as LevelManager;
        GM.AddManager<LevelManager>(levelManager);
        #endregion
        ]]> 
        */
        /// </summary>
        /// <param name="_gameManager"></param>
        void ConfigureGame(T _gameManager);

    }
}