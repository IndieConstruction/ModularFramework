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
using System.Collections.Generic;
using ModularFramework.Helpers;

namespace ModularFramework.Core.SM {
    /// <summary>
    /// Gestisce un behaviours generico.
    /// </summary>
    public class StateMachine : IStateMachine {

        public IState Actual;
        /// <summary>
        /// Cached list of available states for this state machine.
        /// </summary>
        public List<IState> States { get; set; }
        /// <summary>
        /// The model view.
        /// </summary>
        MonoBehaviour view;

        #region constructors
        /// <summary>
        /// Costruttore. Sarà necessario richiamare la funzione Init prima di utilizzarlo.
        /// </summary>
        /// <param name="_states"></param>
        public StateMachine(List<IState> _states) {
            States = _states;
            foreach (IState state in States) {
                state.TypeName = state.GetType().FullName;
                TypeCache.GetType(state.TypeName);
            }
            Change(TypeCache.GetType(States[0].TypeName));
        }

        /// <summary>
        /// Costruttore. Chiamata init automatica.
        /// </summary>
        /// <param name="_behaviours"></param>
        /// <param name="_view"></param>
        public StateMachine(List<IState> _behaviours, MonoBehaviour _view) : this(_behaviours) {
            Init(_view);
        }
        #endregion

        /// <summary>
        /// Inietta la view.
        /// </summary>
        /// <param name="_view"></param>
        public void Init(MonoBehaviour _view)  {
            view = _view;
        }

        /// <summary>
        /// Setta come behaviour attivo il behaviour del tipo passato nel typeparam.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Change<T>() where T : IState {
            Change(typeof(T));
        }

        /// <summary>
        /// Setta come behaviour attivo il behaviour del tipo passato nel parametro.
        /// </summary>
        /// <param name="_type"></param>
        public void Change(Type _type) {
            if(Actual != null)
                Actual.End();
            Actual = States.Find(b => TypeCache.GetType(b.TypeName) /* b.GetType() */ == _type);
            Actual.Start(view);
        }

    }
}