﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using DG.Tweening;
using System.Linq;
using ModularFramework.Data;
using System;

namespace ModularFramework.AI {

    public class AIAgent : MonoBehaviour, IAgent, IDataContainer {

        #region Properties

        /// <summary>
        /// Patrol points che determinano il percorso di patrolling.
        /// </summary>
        public List<PatrolPoint> PathTargets = new List<PatrolPoint>();
        
        /// <summary>
        /// Grid.
        /// </summary>
        Grid grid;

        /// <summary>
        /// Lista dei nodi che compongono il patrolling path.
        /// </summary>
        public List<Node> PatrolPath = new List<Node>();
        int patrolPathIndex = 0;
        
        /// <summary>
        /// Lista degli step (nodi) del path patrol attuale.
        /// </summary>
        List<Node> ActivePath = null;

        [Header("AI Settings")]
        
        #region AI Settings
        [Header("AI Settings")]

        public bool Loop = true;
        public bool Reversable = true;
        
        public bool AutoFindPathTargets = false;
        public AIType AIAptitude = AIType.FullMover;
        #endregion

        #endregion

        #region Data

        public AIAgentData Data = new AIAgentData();

        /// <summary>
        /// Restituisce i dati, se necessario elaborati, per il salvataggio.
        /// </summary>
        /// <returns></returns>
        public virtual IData GetDataForSave() {
            return Data;
        }


        /// <summary>
        /// IData Type.
        /// </summary>
        [Serializable]
        public class AIAgentData : IData {
            public string AgentID;
            public float MoveSpeed = 0.5f;
            public bool IsPatroller = true;

        }

        #endregion

        #region Pluggable Abilities

        #region Patroling
        IPatroller patrolController;
        #endregion

        #endregion

        #region Events

        #region subsciptions
        void OnEnable() {
            grid = GetComponentInParent<PathFindingGridForITileSystem>();
            if (grid != null)
                grid.OnSetupDone += Grid_OnSetupDone;
            else
                Debug.LogWarningFormat("Component Grid not found in parent hierarchy!");
        }

        void OnDisable() {
            grid.OnSetupDone -= Grid_OnSetupDone;
        }
        #endregion

        #region delegates
        /// <summary>
        /// It occurs when grid setup done.
        /// </summary>
        /// <param name="_grid"></param>
        private void Grid_OnSetupDone(Grid _grid) {



            // Todo: il target verrà selezionato a seconda della situazione.
            //Target = FindObjectOfType<PlayerController>().transform;
            //GoToTarget(Target);
            //createPatrolPath(PathTargets);
        }

        /// <summary>
        /// It occurs when a target node has been reached.
        /// </summary>
        private void OnPathEnded() {
            // PATROLING
            if (Data.IsPatroller) { 
                patrolPathIndex++;
                // TODO: insert patrol loop logic here
                if (patrolPathIndex >= PatrolPath.Count)
                    patrolPathIndex = 0;
                GoToNodeTarget(PatrolPath[patrolPathIndex]);
            }
        }
        #endregion

        #endregion

        #region Init
        void Awake() {
            gizmoColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1.0f);
        }

        public void Setup(Grid _grid) {
            grid = _grid;

            // Monitoring every PathTargets changes
            this.ObserveEveryValueChanged(th => th.PathTargets.Count).Subscribe(_ => {
                if (grid == null)
                    return;
                if (PathTargets.Count > 1)
                    createPatrolPath(PathTargets);
            });

            patrolController = GetComponent<IPatroller>();
            if(patrolController != null)
                SetupPatrolPoints(patrolController.GetPatrolData());

        }
        #endregion

        #region AI API

        /// <summary>
        /// Usa come fonte patrolData.PatrolPoints per riempire PathTargets dopo le dovute modifiche.
        /// </summary>
        /// <param name="patrolData"></param>
        public void SetupPatrolPoints(PatrolData patrolData) {
            // AutoFind Nodes
            foreach (PatrolPoint position in patrolData.PatrolPoints) {
                PathTargets.Add(position);
            }
        }

        public PathFindingSettings AISettings { get { return GetAISettingsFromAIType(AIAptitude); } }

        public enum AIType {
            /// <summary>
            /// no moves
            /// </summary>
            None = 0,
            /// <summary>
            /// move everywhere
            /// </summary>
            FullMover = 1,
            /// <summary>
            /// locked to ground (TODO)
            /// </summary>
            LockedToGround = 2,
        }

        /// <summary>
        /// Imposta il pathfinding attuale per raggiungere il target e inizia il movimento.
        /// </summary>
        /// <param name="_target"></param>
        public void GoToTarget(Transform _target) {
            ActivePath = FindPath(transform.position, _target.position, AISettings);
            if (ActivePath == null || ActivePath.Count == 0) {
                // OnPathEnded();
                Debug.LogFormat("Path for target {0} from {1} not found!", _target.position, transform.position);
                return;
            }
            moveOnPath();
        }

        /// <summary>
        /// Imposta il pathfinding attuale per raggiungere il target e inizia il movimento.
        /// </summary>
        /// <param name="_node"></param>
        void GoToNodeTarget(Node _node) {
            ActivePath = FindPath(transform.position, _node.WorldPosition, AISettings);

            if (ActivePath == null || ActivePath.Count == 0) {
                // OnPathEnded();
                Debug.LogFormat("Path for target {0} from {1} not found!", _node.WorldPosition, transform.position);
                return;
            }
            moveOnPath();
        }

        #endregion

        #region internal functions

        #region PathFinding

        /// <summary>
        /// GetAISettingsFromAIType
        /// </summary>
        /// <param name="_AIType"></param>
        /// <returns></returns>
        PathFindingSettings GetAISettingsFromAIType(AIType _AIType) {
            switch (_AIType) {
                case AIType.FullMover:
                    return PathFindingSettings.FullMover;
                case AIType.LockedToGround:
                    return PathFindingSettings.LockedToGround;
                default:
                    return PathFindingSettings.FullMover;
            }
        }

        /// <summary>
        /// Find path from start position to end posision i accord to pathfindingSettings.
        /// If not found path return empty node list.
        /// </summary>
        /// <param name="_startPos"></param>
        /// <param name="_targetPos"></param>
        /// <param name="_settings"></param>
        /// <returns></returns>
        List<Node> FindPath(Vector3 _startPos, Vector3 _targetPos, PathFindingSettings _settings) {
            Node startNode = grid.NodeFromWorldPoint(new Vector3(_startPos.x, _startPos.y, 0));
            Node targetNode = grid.NodeFromWorldPoint(_targetPos);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            // Ciclo la collezione open fino a quando ha elementi e scelgo quello con il costo minore per avanzare con la ricerca
            while (openSet.Count > 0) {
                Node node = openSet[0];
                for (int i = 1; i < openSet.Count; i++) {
                    if (openSet[i].F_Cost < node.F_Cost || openSet[i].F_Cost == node.F_Cost) {
                        if (openSet[i].H_Cost < node.H_Cost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);
                // se il nodo attuale è uguale al target, ho raggiunto il target, ho terminato.
                if (node == targetNode) {
                    return RetracePath(startNode, targetNode);
                }

                List<Node> nNodes = grid.GetNeighbours(node);
                // ---------------------------------
                // --------- Settings Eval ---------
                if (!_settings.CanJump) {
                    nNodes = nNodes.Where(n => n.ContainsTag("walkable")).ToList<Node>();
                }
                // ---------------------------------

                foreach (Node neighbour in nNodes) {
                    if (!neighbour.ContainsTag("traversable") || closedSet.Contains(neighbour)) {
                    //if (closedSet.Contains(neighbour)) {
                            continue;
                    }

                    int newCostToNeighbour = node.G_Cost + GetDistance(node, neighbour, _settings);
                    if (newCostToNeighbour < neighbour.G_Cost || !openSet.Contains(neighbour)) {
                        neighbour.G_Cost = newCostToNeighbour;
                        neighbour.H_Cost = GetDistance(neighbour, targetNode, _settings);
                        neighbour.Parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            return new List<Node>();
        }

        /// <summary>
        /// Get distance from 2 nodes, calculating shorted path using movement weights (diagonals and not diagonals).
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        int GetDistance(Node nodeA, Node nodeB, PathFindingSettings _pathFindingSettings) {
            int dstX = Mathf.Abs(nodeA.PositionOnGrid.y - nodeB.PositionOnGrid.y);
            int dstY = Mathf.Abs(nodeA.PositionOnGrid.x - nodeB.PositionOnGrid.x);

            if (dstX > dstY)
                return _pathFindingSettings.DiagonalMoveCost * dstY + _pathFindingSettings.StraightMoveCost * (dstX - dstY);
            return _pathFindingSettings.DiagonalMoveCost * dstX + _pathFindingSettings.StraightMoveCost * (dstY - dstX);
        }

        List<Node> RetracePath(Node startNode, Node endNode) {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode) {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();

            return path;
        }
        #endregion

        #region Movements

        /// <summary>
        /// Set patrol path and start it if requested by startAfterCreation.
        /// </summary>
        /// <param name="_targets"></param>
        /// <param name="startAfterCreation"></param>
        void createPatrolPath(List<PatrolPoint> _targets, bool startAfterCreation = true) {
            if (PathTargets == null || PathTargets.Count <= 0) 
                return;
            PatrolPath = TransformTargetsToNodeList(_targets);
            patrolPathIndex = 0;
            if (startAfterCreation)
                GoToNodeTarget(PatrolPath[patrolPathIndex]);
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

        /// <summary>
        /// Percorre tutto il path attuale fino al termine.
        /// </summary>
        void moveOnPath() {
            if (ActivePath.Count <= 0) {
                OnPathEnded();
                return;
            }
            moveToNode(ActivePath[0]);
        }

        /// <summary>
        /// Effettua l'effettivo movimento dello step attuale del path.
        /// </summary>
        /// <param name="_node"></param>
        void moveToNode(Node _node) {
            Sequence PathSequence = DOTween.Sequence();
            PathSequence.Append(transform.DOMove(_node.WorldPosition, Data.MoveSpeed).OnComplete(delegate() {
                ActivePath.Remove(_node);
                moveOnPath();
            }).SetEase(Ease.Linear));
        }

        #endregion
        
        #endregion

        #region Gizmos
        Color gizmoColor;

        void OnDrawGizmos() {
            if (!grid || !grid.PathFindingGizmos)
                return;
            float GPointSize = 1;
            if (ActivePath != null) {
                foreach (Node node in ActivePath) {
                    if (ActivePath.Contains(node)) {
                        GPointSize = 0.25f;
                        Gizmos.color = gizmoColor;
                        Gizmos.DrawSphere(node.WorldPosition, grid.NodeRadius * GPointSize);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected() {
            if (!grid || !grid.PathFindingGizmos)
                return;

            // Test to delete

            List<Node> targetNodes = new List<Node>();
            foreach (var target in PathTargets) {
                Node nodeToAdd = grid.NodeFromWorldPoint(target.Position);
                targetNodes.Add(nodeToAdd);
            }
            float GPointSize = 1f;
            Gizmos.color = Color.black;
            if (ActivePath != null) {
                foreach (Node node in targetNodes) {
                    Gizmos.color = gizmoColor;
                    Gizmos.DrawSphere(node.WorldPosition, grid.NodeRadius * GPointSize);
                }
            }
            // -----            
        }

        #endregion
    }

    /// <summary>
    /// Settings for pathfinding that changes p. behaviour.
    /// </summary>
    public class PathFindingSettings {
        public int StraightMoveCost = 10;
        public int DiagonalMoveCost = 14;
        public bool CanJump = true;
        public bool CanFly = true;
        public bool CanFallDown = true;

        #region Presets

        public static PathFindingSettings FullMover = new PathFindingSettings() {
            CanJump = true,
            CanFly = true,
            CanFallDown = true,
        };
        public static PathFindingSettings LockedToGround = new PathFindingSettings() {
            CanJump = false,
            CanFly = false,
            CanFallDown = false,
        };

        public static PathFindingSettings Simple = new PathFindingSettings() {
            StraightMoveCost = 10,
            DiagonalMoveCost = 14,
        };

        public static PathFindingSettings AvoidDiagonals = new PathFindingSettings() {
            StraightMoveCost = 10,
            DiagonalMoveCost = 21,
        };

        public static PathFindingSettings DenyDiagonals = new PathFindingSettings() {
            StraightMoveCost = 10,
            DiagonalMoveCost = 100,
        };

        #endregion
    }


}