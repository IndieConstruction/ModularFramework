# Finite State Machine [v.1.0](#releases) - [ModularFramework](../../../README.MD)

## Come funziona
TODO 

### Minimal State machine

```c#

    #region minimal state machine

        public List<IDisposable> observables = new List<IDisposable>();

        public enum State { none, unloaded, loading, loaded, started, ended }
        /// <summary>
        /// Current state.
        /// </summary>
        public State CurrentState {
            get { return _currentState; }
            private set {
                if (_currentState != value) {
                    if (!beforeStateChange(value, _currentState))
                        return;
                    State oldState = _currentState;
                    observables.ForEach(o => o.Dispose());
                    observables.Clear();
                    _currentState = value;
                    afterStateChanged(_currentState, oldState);
                    if (OnStateChanged != null)
                        OnStateChanged(_currentState);
                    return;
                }
                _currentState = value;
            }
        }
        private State _currentState = State.none;

        /// <summary>
        /// Operazioni da eseguire prima del cambio stato. Se si vuole impedire il cambio ritornare false.
        /// </summary>
        /// <param name="newState"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        bool beforeStateChange(State newState, State currentState) {
            switch (newState) {
                case State.unloaded:
                    break;
                case State.loading:
                    break;
                case State.loaded:
                    break;
                case State.started:
                    break;
                case State.ended:
                    break;
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Operazioni da eseguire dopo il cambio stato.
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="oldState"></param>
        void afterStateChanged(State currentState, State oldState) {
            switch (currentState) {
                case State.none:
                    break;
                case State.unloaded:
                    observables.Add(Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Space)).Subscribe(_ => {
                        Debug.Log("Space clicked!");
                        CurrentState = State.loading;
                    }));
                    break;
                case State.loading:
                    observables.Add(Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.A)).Subscribe(_ => {
                        Debug.Log("A clicked!");
                    }));
                    observables.Add(Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.S)).Subscribe(_ => {
                        Debug.Log("S clicked!");
                        CurrentState = State.unloaded;
                    }));
                    break;
                case State.loaded:
                    break;
                case State.started:
                    break;
                case State.ended:
                    break;
                default:
                    break;
            }
        }

        public delegate void StateEventHandler(State _state);
        /// <summary>
        /// Happens when state changed.
        /// </summary>
        public event StateEventHandler OnStateChanged;

    #endregion

```

## Releases

### v.1.0
- Minimal state machine.

## Next Releases

### v.1.1
