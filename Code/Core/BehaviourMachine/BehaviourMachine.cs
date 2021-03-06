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
using System;
using System.Collections.Generic;
using ModularFramework.Helpers;

namespace ModularFramework.Core.BM {
    /// <summary>
    /// Gestisce un behaviours generico.
    /// </summary>
    public class BehaviourMachine<TView> : IBehaviourMachine<TView> where TView : MonoBehaviour {

        public IBehaviour Current;
        /// <summary>
        /// Cached list of available states for this state machine.
        /// </summary>
        public List<IBehaviour> Behaviours { get; set; }
        /// <summary>
        /// The model view.
        /// </summary>
        TView view;

        public delegate void StateEventHandler(IBehaviour state);

        public StateMachineBehaviour OnStateEnd;

        #region constructors
        /// <summary>
        /// Costruttore. Sarà necessario richiamare la funzione Init prima di utilizzarlo.
        /// </summary>
        /// <param name="_behaviours"></param>
        public BehaviourMachine(List<IBehaviour> _behaviours) {
            Behaviours = _behaviours;
            foreach (IBehaviour behaviour in Behaviours) {
                behaviour.TypeName = behaviour.GetType().FullName;
                TypeCache.GetType(behaviour.TypeName);
            }
            Change(TypeCache.GetType(Behaviours[0].TypeName));
        }

        /// <summary>
        /// Costruttore. Chiamata init automatica.
        /// </summary>
        /// <param name="_behaviours"></param>
        /// <param name="_view"></param>
        public BehaviourMachine(List<IBehaviour> _behaviours, TView _view, bool _init = true) {
            injectView(_view);
            Behaviours = _behaviours;
            foreach (IBehaviour state in Behaviours) {
                state.TypeName = state.GetType().FullName;
                TypeCache.GetType(state.TypeName);
            }
            if(_init)
                Change(TypeCache.GetType(Behaviours[0].TypeName));
        }
        #endregion

        /// <summary>
        /// Inietta la view.
        /// </summary>
        /// <param name="_view"></param>
        void injectView(TView _view)  {
            view = _view;
        }

        /// <summary>
        /// Setta come behaviour attivo il behaviour del tipo passato nel typeparam.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Change<T>() where T : IBehaviour {
            Change(typeof(T));
        }

        /// <summary>
        /// Setta come behaviour attivo il behaviour del tipo passato nel parametro.
        /// </summary>
        /// <param name="_type"></param>
        public void Change(Type _type) {
            if(Current != null)
                Current.End();
            Current = Behaviours.Find(b => TypeCache.GetType(b.TypeName) /* b.GetType() */ == _type);
            Current.PreStart(view);
        }

    }
}