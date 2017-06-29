using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using ModularFramework.Data;
using UnityEngine;

namespace ModularFramework.AI {

    public class PatrolComponent : MonoBehaviour, IPathfindComponent {

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

        int patrolPathIndex = 0;

        #region Internal Functions
        #region Data transformation for load and save to/from disk
        /// <summary>
        /// Elabora i dati attuali e li restituisce nel foramto corretto per il salvataggio.
        /// In questo caso le posizioni diventano relative.
        /// </summary>
        /// <returns></returns>
        public IData GetDataForSave() {
            PatrolData returnData = new PatrolData();
            foreach (PatrolPoint pp in Data.PatrolPoints) {
                PatrolPoint newPatrolPoint = new PatrolPoint() {
                    Order = pp.Order,
                    Position = pp.Position - transform.position
                };
                returnData.PatrolPoints.Add(newPatrolPoint);
            }
            // dati senza modifiche
            returnData.Type = Data.Type;
            returnData.Position = transform.position;
            return returnData;
        }

        /// <summary>
        /// Elabora i dati salvati su disco in modo da renderli pronti per essere iniettati negli oggetti di scena.
        /// </summary>
        /// <returns></returns>
        public PatrolData GetDataFromDisk(IData _inputData) {
            PatrolData dataFromDisk = _inputData as PatrolData;
            PatrolData returnData = new PatrolData();
            foreach (PatrolPoint pp in dataFromDisk.PatrolPoints) {
                PatrolPoint newPatrolPoint = new PatrolPoint() {
                    Order = pp.Order,
                    Position = pp.Position + transform.position
                };
                returnData.PatrolPoints.Add(newPatrolPoint);
            }
            returnData.Type = dataFromDisk.Type;
            returnData.Position = transform.position;
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
            if (_patrolPoints.Count <= 0)
                return;
            PatrolPath = TransformPatrolPintsToNodeList(_patrolPoints);
            patrolPathIndex = 0;
        }

        /// <summary>
        /// Data una lista di Transform restituisce una lista di node. 
        /// Usata per generalemente per creare un patrol path.
        /// </summary>
        /// <param name="_targets"></param>
        /// <returns></returns>
        List<Node> TransformPatrolPintsToNodeList(List<PatrolPoint> _targets) {
            List<Node> returnList = new List<Node>();
            foreach (var t in _targets) {
                returnList.Add(grid.NodeFromWorldPoint(t.Position));
            }
            return returnList;
        } 
        #endregion

        #region Events

        /// <summary>
        /// Accade quando è terminata la creazione della griglia su cui si basa il pathfinding.
        /// </summary>
        /// <param name="_grid"></param>
        public void OnPathfindingGridSetupDone(PathfindingGrid _grid) {
            grid = _grid;
            agent = GetComponent<AIAgentComponent>();
        }

        /// <summary>
        /// Accade quando è stato raggiunto un patrol point.
        /// </summary>
        public void OnPatrolPathStepEnded() {
            // TODO: eventuali check per il loop del patrolling... etc
            GoToNextPatrollingNode();
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
                patrolPathIndex = 0;
            } else {
                if (_nodeIndex != -1)
                    patrolPathIndex = _nodeIndex;
                else
                    patrolPathIndex++;
            }
            // TODO: insert patrol loop logic here
            if (patrolPathIndex > PatrolPath.Count - 1 || patrolPathIndex < 0)
                patrolPathIndex = 0;
            switch (Data.Type) {
                case PatrolType.PATHFINDING:
                    agent.FindPathToTarget(PatrolPath[patrolPathIndex]);
                    break;
                case PatrolType.LINEAR:
                    agent.LinearMoveToNode(PatrolPath[patrolPathIndex]);
                    break;
                default:
                    break;
            }
            
        }
        #endregion

    }
}
