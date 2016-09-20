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

        public delegate void RepeatingActionDelegate();
        protected RepeatingActionDelegate actionDelegate;

        public RepeatingAction(float _amountTime, string _id, RepeatingActionDelegate _delegate, bool _play = false) {
            timeCounter = 0;
            id = _id;
            beat = _amountTime;
            actionDelegate = _delegate;
            if (_play)
                Play();
        }

        /// <summary>
        /// Active repeat behaviour.
        /// </summary>
        public void Play() {
            updateObserver = Observable.EveryUpdate().Subscribe(x => {
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
                actionDelegate();
                timeCounter = 0;
            }
        }
    }
}
