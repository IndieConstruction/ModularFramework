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
using ModularFramework.Core;

/// <summary>
/// TODO: Spostare una volta funzionante al 100% in ModularFramework.core
/// </summary>
namespace ModularFramework.Core {

    public abstract class BaseView<M,C> : MonoBehaviour, ISetuppable where M : IModel {

        protected M model;
        protected C controller;

        #region ISetuppable

        public class Settings : ISetupSettings {
            public M model;
            public C controller;
        }

        /// <summary>
        /// Evento invocato al termine di tutte le operazioni di setup.
        /// </summary>
        public event ISetuppableEvents.Event OnSetupCompleted;

        /// <summary>
        /// Esegue il setup esegue caching del model contenuto in <paramref name="_settings"/>.
        /// </summary>
        /// <param name="_settings"></param>
        /// <returns></returns>
        public ISetuppable Setup(ISetupSettings _settings) {
            Settings s = _settings as Settings;
            model = s.model;
            addictionalSetup(_settings);
            dataBindings(_settings);
            SetupEnded();
            return this;
        }

        /// <summary>
        /// Chiamato al termine del setup base e dell'AddictionalSetup.
        /// </summary>
        public void SetupEnded() {
            if (OnSetupCompleted != null)
                OnSetupCompleted(this);
        }

        /// <summary>
        /// Override di questa funzione per aggiungere azioni da eseguire dopo il setup base che salva il model.
        /// </summary>
        /// <param name="_settings"></param>
        protected virtual void addictionalSetup(ISetupSettings _settings) { }

        /// <summary>
        /// Override di questa funzione per
        /// </summary>
        /// <param name="_settings"></param>
        protected virtual void dataBindings(ISetupSettings _settings) { }

        #endregion
    }
}
