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
using UnityEngine.UI;
using System.Collections.Generic;
using ModularFramework.Core;

namespace ModularFramework.Helpers {

    public static class GenericHelper {

        /// <summary>
        /// Return random float value around _value parameter + or - _variation.
        /// Ex: 
        /// (8, 2) = random from 6 and 10
        /// (1.4f, 0.2f) = random from 1.2 and 1.6
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_variation"></param>
        /// <returns></returns>
        public static float GetValueWithRandomVariation(float _value, float _variation) {
            return Random.Range(_value - _variation, _value + _variation);
        }

        /// <summary>
        /// Gets the value within limits. If value exceeds min or max limits, a respective returned. 
        /// </summary>
        /// <param name="_rawValue">The raw value.</param>
        /// <param name="_minLimit">The minimum limit.</param>
        /// <param name="_maxLimit">The maximum limit.</param>
        /// <returns></returns>
        public static float GetValueWithinLimits(float _rawValue, float _minLimit, float _maxLimit) {
            if (_rawValue > _maxLimit)
                _rawValue = _maxLimit;
            else if (_rawValue < _minLimit)
                _rawValue = _minLimit;
            return _rawValue;
        }

        /// <summary>
        /// Gets the list from enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetListFromEnum<T>() {
            List<T> enumList = new List<T>();
            System.Array enums = System.Enum.GetValues(typeof(T));
            foreach (T e in enums) {
                enumList.Add(e);
            }
            return enumList;
        }

        /// <summary>
        /// Create a new instance of <typeparamref name="T"/> based on <paramref name="_original"/> prefab as child of <paramref name="_parent"/> gameobject.
        /// If new instance creation gone lounch <typeparamref name="T"/> Setup with <paramref name="_setupSettings"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_original"></param>
        /// <param name="_parent"></param>
        /// <param name="_setupSettings"></param>
        /// <returns></returns>
        public static T InstantiateAndSetup<T>(T _original, GameObject _parent, ISetupSettings _setupSettings) where T : ISetuppable {
            GameObject newGOInstance = GameObject.Instantiate<GameObject>(_original as GameObject, _parent.transform);
            T newInstance = newGOInstance.GetComponent<T>();
            if (newInstance == null) {
                Debug.LogWarningFormat("Prefab {0} don't contain component of type {1}", _original, _original.GetType());
                return default(T);
            }
            newInstance.Setup(_setupSettings);
            return newInstance;
        }

        #region Colors

        /// <summary>
        /// Restituisce un oggetto color partendo dall'rgb.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        public static Color ColorFromRGB(float red, float green, float blue) {
            return new Color(red / 255, green / 255, blue / 255);
        }

        /// <summary>
        /// Restituisce un oggetto color partendo dall'rgb + alpha.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        public static Color ColorFromRGB(float red, float green, float blue, float alpha) {
            return new Color(red / 255, green / 255, blue / 255, alpha);
        }

        #endregion

        #region extensions

        #region list

        /// <summary>
        /// Return random element of list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_thisList"></param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this List<T> _thisList) {
            return _thisList[Random.Range(0, _thisList.Count)];
        }

        private static System.Random rng = new System.Random();

        /// <summary>
        /// Shuffles the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        public static void Shuffle<T>(this IList<T> list) {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        #endregion

        #region Unity UI

        /// <summary>
        /// Loads the options from enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this">The this.</param>
        public static void LoadOptionsFromEnum<T>(this Dropdown _this) {
            _this.ClearOptions();
            List<Dropdown.OptionData> optionsToAdd = new List<Dropdown.OptionData>();
            foreach (var val in System.Enum.GetValues(typeof(T))) {
                optionsToAdd.Add(new Dropdown.OptionData() { text = val.ToString() });
            }
            _this.options.AddRange(optionsToAdd);
        }

        #endregion

        #endregion

    }
}