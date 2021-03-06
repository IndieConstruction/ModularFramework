﻿using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using ModularFramework.Data;
using UnityEngine;

namespace ModularFramework.AI {

    public class PatrolComponent : MonoBehaviour, IPathfindComponent {

        #region properties and variables

        PathfindingGrid grid;
        AIAgentComponent agent;

        /// <summary>
        /// PatrolData.
        /// </summary>
        public PatrolData Data;
        
        /// <summary>
        /// Lista dei nodi che compongono il patrolling path.
        /// </summary>
        public List<Node> PatrolPath {
            get { return _patrolPath; }
            set { _patrolPath = value; }
        }
        private List<Node> _patrolPath;

        public int PatrolPathIndex {
            get { return _patrolPathIndex; }
            set { _patrolPathIndex = value; }
        }
        private int _patrolPathIndex = 0;

        public Node CurrentPatrolNode {
            get {
                if (PatrolPath != null)
                    return PatrolPath[PatrolPathIndex];
                else
                    return null;
            }
        }
        
        /// <summary>
        /// Rappresenta l'ultimo nodo raggiunto dall'agent.
        /// </summary>
        public Node LastNode {
            get { return _lastNode; }
            set {

                _lastNode = value;
            }
        }
        private Node _lastNode;

        /// <summary>
        /// Contiene tutte le info sull'ultimo step effettuato.
        /// </summary>
        public PatrolStepInfo LastStepInfo {
            get { return _lastStepInfo; }
            set { _lastStepInfo = value; }
        }
        private PatrolStepInfo _lastStepInfo;

        /// <summary>
        /// Se true il senso di percorrenza del path è inverso.
        /// </summary>
        private bool IsReversed = false;

        #endregion

        #region Internal Functions

        #region Data transformation for load and save to/from disk
        /// <summary>
        /// Elabora i dati attuali e li restituisce nel foramto corretto per il salvataggio.
        /// In questo caso le posizioni diventano relative.
        /// </summary>
        /// <returns></returns>
        public IData GetDataForSave() {
            PatrolData returnData = new PatrolData();
            returnData = Data;
            returnData.Position = transform.position;
            List<PatrolPoint> bkPatrolPoints = new List<PatrolPoint>(Data.PatrolPoints);
            returnData.PatrolPoints = new List<PatrolPoint>();
            // modifica dei PatrolPoints in modo da fargli avere posizione relativa.
            foreach (PatrolPoint pp in bkPatrolPoints) {
                PatrolPoint newPatrolPoint = new PatrolPoint() {
                    Order = pp.Order,
                    Position = pp.Position - transform.position
                };
                returnData.PatrolPoints.Add(newPatrolPoint);
            }
            
            return returnData;
        }

        /// <summary>
        /// Elabora i dati salvati su disco in modo da renderli pronti per essere iniettati negli oggetti di scena.
        /// </summary>
        /// <returns></returns>
        public PatrolData GetDataFromDisk(IData _inputData) {
            PatrolData returnData = _inputData as PatrolData;
            // modifica dei PatrolPoints in modo da fargli avere posizione relativa.
            List<PatrolPoint> bkPatrolPointToDisk = new List<PatrolPoint>(returnData.PatrolPoints);
            returnData.Position = transform.position;
            returnData.PatrolPoints = new List<PatrolPoint>();
            foreach (PatrolPoint pp in bkPatrolPointToDisk) {
                PatrolPoint newPatrolPoint = new PatrolPoint() {
                    Order = pp.Order,
                    Position = pp.Position + transform.position
                };
                returnData.PatrolPoints.Add(newPatrolPoint);
            }
            return returnData;
        }
        #endregion

        /// <summary>
        /// Usa come fonte "patrolData.PatrolPoints" per riempire PathTargets dopo le dovute modifiche.
        /// </summary>
        /// <param name="patrolData"></param>
        public void SetupPatrolPoints(IData data) {
            Data = GetDataFromDisk(data);
        }

        /// <summary>
        /// Set patrol path from list of patrolPoints.
        /// </summary>
        /// <param name="_patrolPoints"></param>
        /// <param name="startAfterCreation"></param>
        void createPatrolPath(List<PatrolPoint> _patrolPoints) {
            if (_patrolPoints.Count <= 0) {
                PatrolPath = new List<Node>();
                return;
            }
            PatrolPath = TransformPatrolPointsToNodeList(_patrolPoints);
            PatrolPathIndex = 0;
        }

        /// <summary>
        /// Data una lista di Transform restituisce una lista di node. 
        /// Usata per generalemente per creare un patrol path.
        /// </summary>
        /// <param name="_targets"></param>
        /// <returns></returns>
        List<Node> TransformPatrolPointsToNodeList(List<PatrolPoint> _targets) {
            List<Node> returnList = new List<Node>();
            foreach (var t in _targets) {
                returnList.Add(grid.NodeFromWorldPoint(t.Position));
            }
            return returnList;
        }
        #endregion

        #region State machine
        public enum State { NOT_READY, READY, MOVE_TO_NODE, WAITING_FOR_INSTRUCTIONS }

        public State CurrentState {
            get { return _currentState; }
            set {
                if (_currentState != value) {
                    State oldState = _currentState;
                    _currentState = value;
                    OnStatusChanged(_currentState, oldState);
                }
            }
        }

        private State _currentState = State.NOT_READY;

        void OnStatusChanged(State newState, State oldState) {
            switch (newState) {
                case State.NOT_READY:
                    break;
                case State.READY:
                    break;
                case State.MOVE_TO_NODE:
                    break;
                case State.WAITING_FOR_INSTRUCTIONS:
                    break;
                default:
                    break;
            }
            if (OnStateChanged != null)
                OnStateChanged(newState, oldState);
        }

        public delegate void StateChanged(State newState, State oldState);
        /// <summary>
        /// Happens when state changed.
        /// </summary>
        public event StateChanged OnStateChanged;

        #endregion

        #region Events

        /// <summary>
        /// Accade quando è terminata la creazione della griglia su cui si basa il pathfinding.
        /// </summary>
        /// <param name="_grid"></param>
        public void OnPathfindingGridSetupDone(PathfindingGrid _grid) {
            grid = _grid;
            agent = GetComponent<AIAgentComponent>();
            CurrentState = State.READY;
        }

        /// <summary>
        /// Accade quando è stato raggiunto un patrol point.
        /// </summary>
        public void OnPatrolPathStepEnded() {
            LastStepInfo = new PatrolStepInfo() {
                CurrentNode = CurrentPatrolNode,
                LastNodeReached = LastNode,
                IsLastNodeOfPath = PatrolPathIndex == PatrolPath.Count - 1 ? true : false,
            };
            CurrentState = State.WAITING_FOR_INSTRUCTIONS;
        }

        #endregion

        #region API

        /// <summary>
        /// Fa partire il patrolling.
        /// </summary>
        public void StartPatrolling() {
            GoToNextPatrollingNode(0);
        }

        /// <summary>
        /// Prosegue con il prossimo nodo del patrolNode.
        /// </summary>
        /// <param name="_nodeIndex"></param>
        public void GoToNextPatrollingNode(int _nodeIndex = -1) {
            // Primo accesso, non c'è ancora il patrolling path.
            if (PatrolPath == null) {
                createPatrolPath(Data.PatrolPoints);
                if (PatrolPath.Count < 2) {
                    // Ci sono meno di 2 patrol points non è possibile fare il patroling
                    CurrentState = State.WAITING_FOR_INSTRUCTIONS;
                    return;
                }
                PatrolPathIndex = 0;
            } else {
                if (_nodeIndex != -1) {
                    PatrolPathIndex = _nodeIndex;
                } else {
                    // Salvo l'ultimo nodo raggiunto e aggiorno i dati ...
                    LastNode = PatrolPath[PatrolPathIndex];
                    PatrolPathIndex++;
                }
            }

            // faccio un modo che l'index non vada mai fuori range e se succede lo assegno al limite più vicino.
            if (PatrolPathIndex > PatrolPath.Count - 1 || PatrolPathIndex < 0)
                PatrolPathIndex = 0;
            switch (Data.Type) {
                case PatrolType.PATHFINDING:
                    CurrentState = State.MOVE_TO_NODE;
                    agent.FindPathToTargetAndMove(PatrolPath[PatrolPathIndex]);
                    break;
                case PatrolType.LINEAR:
                    CurrentState = State.MOVE_TO_NODE;
                    agent.MoveLinearyToNode(PatrolPath[PatrolPathIndex]);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Inverte il senso di patrolling.
        /// TODO: non funziona correttamente... 
        /// </summary>
        public void Reverse() {
            if (PatrolPath == null)
                return;
            agent.StopAnyMovement();
            PatrolPath.Reverse();
            if (CurrentState == State.MOVE_TO_NODE) {
                PatrolPathIndex++;
                GoToNextPatrollingNode();
            }
        }

        #endregion

        #region structures

        /// <summary>
        /// Questa struttura contiene le informazioni dello step appena terminato.
        /// </summary>
        public struct PatrolStepInfo {
            public Node CurrentNode;
            public Node LastNodeReached;
            public bool IsLastNodeOfPath;
        }

        #endregion
    }
}
