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
using DG.Tweening;

namespace ModularFramework.Components {
    /// <summary>
    /// Follow the gameobject on selected axes with selected delay.
    /// </summary>
    public class GOFollower : MonoBehaviour {

        public GameObject GOToFollow;
        public float FollowDelay;
        public bool X, Y, Z;

        Vector3 initialPosition;
        Tween tw;

        void Start() {
            initialPosition = transform.position;
            
        }

        void Update() {

            tw.Kill();
            tw = transform.DOMove(new Vector3(
                X ? GOToFollow.transform.position.x : initialPosition.x,
                Y ? GOToFollow.transform.position.y : initialPosition.y,
                Z ? GOToFollow.transform.position.z : initialPosition.z
                ), FollowDelay);
        }

        void OnDestroy() {
            tw.Kill();
        }

    }
}
