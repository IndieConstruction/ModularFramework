using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ModularFramework.Core;
// using TMPro;

namespace ModularFramework.Components {

    /// <summary>
    /// That component provide timer for gameplay. 
    /// </summary>
    public class GameplayTimer : Singleton<GameplayTimer> {

        #region Settings
        [Range(10, 300)]
        public float Time;
        // private TextMeshProUGUI timerText;
        #endregion

        #region Runtime variables
        /// <summary>
        /// 
        /// </summary>
        protected int ActualTime {
            get { return actualTime; }
            set {
                if (actualTime != (int)timeRemaining)
                    timeChanged((int)timeRemaining);
                actualTime = (int)timeRemaining;
            }
        }
        private int actualTime;


        private bool isRunning;
        private float timeRemaining;
        protected List<CustomEventData> CustomEvents;
        protected List<CustomEventData> RepeatingEvents;
        #endregion

        #region timer update
        void Update() {
            if (!isRunning)
                return;
            if (timeRemaining > 0f) {
                timeRemaining -= UnityEngine.Time.deltaTime;
            } else {
                if (OnTimeOver != null)
                    OnTimeOver(0);
                timeRemaining = 0;
                isRunning = false;
            }
            DisplayTime();
        }
        #endregion

        #region Timer functionalities
        

        /// <summary>
        /// Start timer with time count by param.
        /// </summary>
        /// <param name="_time"></param>
        public void StartTimer(float _time, List<CustomEventData> _customEvents = null) {
            if (OnStartTimer != null)
                OnStartTimer(_time);
            ResetTimer(_time);
            StartTimer();
            if (_customEvents != null)
                CustomEvents = _customEvents;
        }

        /// <summary>
        /// Start timer with default time count.
        /// </summary>
        public void StartTimer() {
            isRunning = true;
        }

        /// <summary>
        /// Stop timer.
        /// </summary>
        public void StopTimer() {
            isRunning = false;
        }

        /// <summary>
        /// Reset timer with default time count.
        /// </summary>
        public void ResetTimer() {
            timeRemaining = Time;
        }

        /// <summary>
        /// Reset timer with time count by param.
        /// </summary>
        /// <param name="_time"></param>
        public void ResetTimer(float _time) {
            timeRemaining = _time;
        }

        // TODO generalize timer displayer.
        public void DisplayTime() {
            //if (!timerText)
            //    timerText = GetComponent<TextMeshProUGUI>();
            ActualTime = (int)timeRemaining;
            var text = ActualTime.ToString(); //Mathf.Floor(timeRemaining).ToString();
            //timerText.text = text;
        }

        /// <summary>
        /// Force end time.
        /// </summary>
        public void EndTimeRemaning() {
            timeRemaining = 0;
        }
        #endregion

        #region events

        public delegate void TimerEvent(float _time);
        public delegate void CustomTimerEvent(CustomEventData _data);

        public static event TimerEvent OnStartTimer;
        public static event TimerEvent OnTimeOver;
        public static event CustomTimerEvent OnCustomEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_eventData"></param>
        public void RegisterCustomEvent(CustomEventData _eventData) {
            CustomEvents.Add(_eventData);
        }

        public struct CustomEventData {
            public string Name;
            public int Time;
            public bool Repeated;
        }

        /// <summary>
        /// Called every value change of int value of timer.
        /// </summary>
        /// <param name="_time"></param>
        void timeChanged(int _time) {
            foreach (var ev in CustomEvents.FindAll(e => e.Time == _time)) {
                if (OnCustomEvent != null)
                    OnCustomEvent(ev);
            }
        }
        #endregion

        #region INSTRUCTIONS
        /* ======== INSTRUCTIONS ========
        Provide a singleton implementation of timer for gameplay. 
        "Time" public var is amount of gameplay timer.
        At start timer "OnStartTimer" event is called.
        At end of Time amount timer stop and "OnTimeOver" event is called.
        Start a singleton GameplayTimer by call "StartTimer" function:
        ---------------------------------
        GameplayTimer.Instance.StartTimer(25.0f);
        ---------------------------------
        If you want to call some custom events during the timer run state you can add a list of CustomEventData:
        ---------------------------------
        GameplayTimer.Instance.StartTimer(25.0f,
                new List<GameplayTimer.CustomEventData>()
                {
                            new GameplayTimer.CustomEventData() { Name = "SomeTimedEvent", Time = 10
                },
                            new GameplayTimer.CustomEventData() { Name = "SomeTimedEvent2", Time = 22 - 10 }
                },
                            new GameplayTimer.CustomEventData() { Name = "RepeatedEventEvery2Sec", Time = 2, Repeated = true }
                }
        );
        ---------------------------------
        */
        #endregion

    }
}