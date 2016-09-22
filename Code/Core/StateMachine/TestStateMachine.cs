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
using System.Collections.Generic;
using ModularFramework.Core;

namespace ModularFramework.Test {

    public class TestStateMachine : MonoBehaviour {

        StateMachine bm;

        // Use this for initialization
        void Start() {
            bm = new StateMachine(new List<IState> {
            new TestStateDummy1(),
            new TestStateDummy2(),
            new TestStateDummy3(),
            new TestStateDummy4(),
        });
        }

        // Update is called once per frame
        void Update() {
            for (int i = 0; i < 4; i++) {
                bm.Change(bm.States[Random.Range(0, 4)].GetType());
            }
        }
    }

    public class TestStateDummy1 : BaseState {
        int count = 0;
        public override void Start(MonoBehaviour _view) {
            base.Start(_view);
            Debug.Log("B: " + GetType());
        }

        public override void End() {
            base.End();
            Debug.LogFormat("+ Count {0}", count++);
        }
    }

    public class TestStateDummy2 : BaseState {
        int count = 0;
        public override void Start(MonoBehaviour _view) {
            base.Start(_view);
            Debug.Log("B: " + GetType());
            // initTest
            count = 0;
        }

        public override void End() {
            base.End();
            Debug.LogFormat("= Count {0}", count++);
        }
    }

    public class TestStateDummy3 : BaseState {
        public override void Start(MonoBehaviour _view) {
            base.Start(_view);
            Debug.Log("B: " + GetType());
        }
    }

    public class TestStateDummy4 : BaseState {
        public override void Start(MonoBehaviour _view) {
            base.Start(_view);
            Debug.Log("B: " + GetType());
        }
    }
}
