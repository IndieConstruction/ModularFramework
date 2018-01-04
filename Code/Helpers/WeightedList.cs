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
using System.Collections.Generic;

namespace ModularFramework.Helpers {

    /// <summary>
    /// Lista pesata.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeightedList<T> {
        List<T> flatList = new List<T>();

        /// <summary>
        /// Crea una lista pesata di elementi T.
        /// </summary>
        /// <param name="elemets"></param>
        public WeightedList(List<WeightedElement<T>> elemets) {
            foreach (var e in elemets) {
                for (int i = 0; i < e.Weight; i++) {
                    flatList.Add(e.Element);
                }
            }
        }

        /// <summary>
        /// Restituisce un elemento random tenendo conto dei pesi dati in fase di creazione della lista.
        /// </summary>
        /// <returns></returns>
        public T GetRandomElement() {
            return flatList.GetRandomElement<T>();
        }
    }

    /// <summary>
    /// Elemento gerico della lista pesata.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeightedElement<T> {

        public T Element;
        public int Weight;

        public WeightedElement(T element, int weight) {
            Element = element;
            Weight = weight;
        }
    }
}