using UnityEngine;
using System;
using System.Collections;
using UniRx;

namespace ModularFramework.Components {

    public class RepeatingAction {

        /// <summary>
        /// Observe any update if not null.
        /// </summary>
        IDisposable updateObserver;
        protected float beat;
        protected string id;
        protected float timeCounter;

        public RepeatingAction(float _amountTime, string _id, bool _play = false) {
            timeCounter = 0;
            id = _id;
            beat = _amountTime;
            if (_play)
                Play();
        }

        /// <summary>
        /// Active repeat behaviour.
        /// </summary>
        public void Play() {
            updateObserver = Observable.EveryUpdate().Subscribe(x => {
                // place here update logic
                Update();
                
            });
        }

        /// <summary>
        /// Stop repeat behaviour.
        /// </summary>
        public void Stop() {
            updateObserver.Dispose();
        }

        public void Update() {
            timeCounter += Time.deltaTime;
            if(timeCounter >= beat) { 
                Debug.LogFormat("{0} {1} : {2}", id, beat, timeCounter);
                timeCounter = 0;
            }
        }
    }
}
