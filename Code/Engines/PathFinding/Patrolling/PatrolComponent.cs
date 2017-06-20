using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using ModularFramework.Data;
using UnityEngine;

namespace ModularFramework.AI {

    public class PatrolComponent : MonoBehaviour, IPathfindComponent {

        PathfindingGrid grid;
        AIAgentComponent aiAgent;

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
        private List<Node> _patrolPath = new List<Node>();


        int patrolPathIndex = 0;

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
            return returnData;
        }

        /// <summary>
        /// Elabora i dati salvati su disco in modo da renderli pronti per essere iniettati negli oggetti di scena.
        /// </summary>
        /// <returns></returns>
        public PatrolData GetDataFromDisk(IData _inputData) {
            PatrolData returnData = new PatrolData();
            foreach (PatrolPoint pp in (_inputData as PatrolData).PatrolPoints) {
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
        /// Usa come fonte patrolData.PatrolPoints per riempire PathTargets dopo le dovute modifiche.
        /// </summary>
        /// <param name="patrolData"></param>
        public void SetupPatrolPoints(IData data) {
            Data = GetDataFromDisk(data);

            foreach (PatrolPoint pp in Data.PatrolPoints) {
                PatrolPath.Add(grid.NodeFromWorldPoint(pp.Position));
            }
        }

        /// <summary>
        /// Set patrol path and start it if requested by startAfterCreation.
        /// </summary>
        /// <param name="_targets"></param>
        /// <param name="startAfterCreation"></param>
        void createPatrolPath(List<PatrolPoint> _targets, bool startAfterCreation = true) {
            if (_targets.Count <= 0)
                return;
            PatrolPath = TransformTargetsToNodeList(_targets);
            patrolPathIndex = 0;
            if (startAfterCreation)
                aiAgent.GoToNodeTarget(PatrolPath[patrolPathIndex]);
        }

        /// <summary>
        /// Data una lista di Transform restituisce una lista di node. 
        /// Usata per generalemente per creare un patrol path.
        /// </summary>
        /// <param name="_targets"></param>
        /// <returns></returns>
        List<Node> TransformTargetsToNodeList(List<PatrolPoint> _targets) {
            List<Node> returnList = new List<Node>();
            foreach (var t in _targets) {
                returnList.Add(grid.NodeFromWorldPoint(t.Position));
            }
            return returnList;
        }

        #region Events

        private void OnEnable() {
            // Monitoring every PatrolPath changes
            this.ObserveEveryValueChanged(th => th.PatrolPath.Count).Subscribe(_ => {
                if (grid == null)
                    return;
                if (PatrolPath.Count > 1)
                    createPatrolPath(Data.PatrolPoints);
            });
        }

        public void OnPathfindingGridSetupDone(PathfindingGrid _grid) {
            grid = _grid;
            aiAgent = GetComponent<AIAgentComponent>();
        }

        /// <summary>
        /// Accade quando è stato raggiunto un patrol point.
        /// </summary>
        public void OnPatrolPathStepEnded() {
            // PATROLING
            patrolPathIndex++;
            // TODO: insert patrol loop logic here
            if (patrolPathIndex >= PatrolPath.Count)
                patrolPathIndex = 0;
            aiAgent.GoToNodeTarget(PatrolPath[patrolPathIndex]);
        }



        #endregion
    }
}
