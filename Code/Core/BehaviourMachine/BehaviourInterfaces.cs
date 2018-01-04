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
using System.Collections.Generic;

namespace ModularFramework.Core.BM {

    public interface IBehaviour {
        string TypeName { get; set; }
        void PreStart(MonoBehaviour _view);
        void Start();
        void Update();
        void End();
    }

    /// <summary>
    /// Base class for any behaviour.
    /// </summary>
    /// <seealso cref="ModularFramework.Core.BM.IBehaviour" />
    public abstract class BaseBehaviour : IBehaviour {
        public string TypeName { get; set; }
        protected MonoBehaviour view;

        /// <summary>
        /// Start the behaviour and inject the monobehaviour (view) in "view" variable..
        /// </summary>
        /// <typeparam name="T">The type of the 1.</typeparam>
        /// <param name="_view">The view.</param>
        public void PreStart(MonoBehaviour _view) {
            view = _view;
            Start();
        }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void End() { }



    }

    /// <summary>
    /// Base State class for scene.
    /// Create derived class from this to create a scene behaviour class.
    /// Class features:
    /// - T generic type rapresent the type of view. When create derived class from this you can specify the type of view.
    /// - The Start base function automatic call the scene with name defined as parameters on constructor.
    /// </summary>
    /// <seealso cref="ModularFramework.Core.BM.BaseBehaviour" />
    public abstract class SceneBehaviour : BaseBehaviour {

        public string SceneName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneBehaviour"/> class.
        /// Is injected in construction call the name of the scene.
        /// </summary>
        /// <param name="_sceneName">Name of the scene.</param>
        public SceneBehaviour(string _sceneName) {
            SceneName = _sceneName;
        }

        /// <summary>
        /// Start the behaviour and inject the monobehaviour (view) in "view" variable.
        /// this occurs before scene loaded. OnSceneLoadedCompleted is called when the scene has loaded.
        /// </summary>
        /// <param name="_view">The view.</param>
        public override void Start() {
            GameManager.Instance.Modules.SceneModule.LoadSceneWithTransition(SceneName, OnSceneLoadedCompleted);
        }

        /// <summary>
        /// Called when [scene loaded completed].
        /// </summary>
        public virtual void OnSceneLoadedCompleted() {
            Debug.LogFormat("Scene {0} loaded!", SceneName);
        }

    }
}
