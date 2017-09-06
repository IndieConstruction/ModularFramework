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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularFramework.Helpers;

namespace ModularFramework.Core {

    public class MonoBehaviour_MFExtended : MonoBehaviour {

        public T AddComponentAndSetup<T>(ISetupSettings _settings) where T : MonoBehaviour, ISetuppable {
            T newComponent =  gameObject.AddComponent<T>();
            newComponent.Setup(_settings);
            return newComponent;
        }

        /// <summary>
        /// Create new instance of <typeparamref name="T"/> object as child of "this" and setup it with <paramref name="_setupSettings"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_original"></param>
        /// <param name="_parent"></param>
        /// <param name="_worldPositionStays"></param>
        public T InstantiateAndSetup<T>(T _original, ISetupSettings _setupSettings) where T : ISetuppable {
            T newInstance = GenericHelper.InstantiateAndSetup<T>(_original, gameObject, _setupSettings);
            return newInstance;
        }
    }
}