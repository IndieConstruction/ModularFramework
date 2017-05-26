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
using System;
using System.Collections.Generic;

namespace ModularFramework.Helpers {
    /// <summary>
    /// Cache any class type eliminating the overhead by using reflection.
    /// Use TypeCache.GetType instead of System.GetType.
    /// </summary>
    public static class TypeCache {

        private static readonly Dictionary<string, Type> typeCache = new Dictionary<string, Type>();

        /// <summary>
        /// Get Type reflaction information using cached value at first request.
        /// </summary>
        /// <param name="_typeName"></param>
        /// <returns></returns>
        public static Type GetType(string _typeName) {
            Type type = null;
            if (typeCache.TryGetValue(_typeName, out type)) {
                return type;
            }

            type = Type.GetType(_typeName);
            if (type != null) {
                typeCache.Add(_typeName, type);
            }
            return type;
        }

    }
}
