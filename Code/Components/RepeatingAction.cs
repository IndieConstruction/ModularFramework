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
using System;
using System.Collections;
using UniRx;

namespace ModularFramework.Components {
    /// <summary>
    /// Create infinite (at the moment) repeated action defined in the caller delegate.
    /// Remember to call "Stop" function to dispose the behaviour.
    /// TODO:
    /// - add ability to set limited number of iteration.
    /// - add ability to pause behaviour saving the data and restart it from saved position.
    /// </summary>
    public class RepeatingAction {

        #region runtime stuff
        /// <summary>
        /// The data.
        /// </summary>
        Data data;

        /// <summary>
        /// Delegate function.
        /// </summary>
        public delegate void RepeatingActionDelegate(Data _data);
        protected RepeatingActionDelegate actionDelegate;

        /// <summary>
        /// Observe any update if not null.
        /// </summary>
        IDisposable updateObserver;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatingAction"/> class.
        /// </summary>
        /// <param name="_data">The data.</param>
        /// <param name="_delegate">The delegate.</param>
        /// <param name="_play">if set to <c>true</c> [play].</param>
        public RepeatingAction(Data _data, RepeatingActionDelegate _delegate, bool _play = false) 
            : this(_data.beat, _data.id, _delegate, _play) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatingAction"/> class.
        /// </summary>
        /// <param name="_amountTime">The amount time.</param>
        /// <param name="_id">The identifier.</param>
        /// <param name="_delegate">The delegate.</param>
        /// <param name="_play">if set to <c>true</c> [play].</param>
        public RepeatingAction(float _amountTime, string _id, RepeatingActionDelegate _delegate, bool _play = false) {
            data = new Data();
            data.timeCounter = 0;
            data.id = _id;
            data.beat = _amountTime;
            actionDelegate = _delegate;
            if (_play)
                Play();
        }
        #endregion

        #region life cycle
        /// <summary>
        /// Active repeat behaviour.
        /// </summary>
        public void Play() {
            updateObserver = Observable.EveryUpdate().Subscribe(x => {
                Update();
            });
        }

        /// <summary>
        /// Stop repeat behaviour.
        /// </summary>
        public void Stop() {
            updateObserver.Dispose();
        }

        public void Update() {
            data.timeCounter += Time.deltaTime;
            if(data.timeCounter >= data.beat) {
                actionDelegate(data);
                data.timeCounter = 0;
            }
        }
        #endregion

        #region data structures
        [Serializable]
        public struct Data {
            /// <summary>
            /// The beat frequency.
            /// </summary>
            public float beat;
            /// <summary>
            /// The RepeatingAction identifier.
            /// </summary>
            public string id;
            /// <summary>
            /// The time counter.
            /// </summary>
            public float timeCounter;
        }
        #endregion
    }
}
