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

namespace ModularFramework.Core {

    /// <summary>
    /// Classe base per tutti i controller che usano il pattern MVC.
    /// E' un ISetuppable, quindi le classi che ereditano da questa hanno già tutto il sistema eventi dell'ISetuppable.
    /// </summary>
    /// <typeparam name="M"></typeparam>
    public class BaseController<M> {

        public static BaseController<M> Instance { get; set; }



    }

}