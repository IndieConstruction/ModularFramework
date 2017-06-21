using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using DG.Tweening;
using ModularFramework.Data;
using System;

namespace ModularFramework.AI {

    /// <summary>
    /// Classe base per intelligenza artificiale.
    /// - Settings per comportamenti di intelligenza artificila.
    /// - Pathfiding.
    /// -- Create grid based on Tilemap stage.
    /// </summary>
    public class AIAgentComponent : MonoBehaviour, IPathfindComponent {

        #region Properties
        
        /// <summary>
        /// Grid.
        /// </summary>
        PathfindingGrid grid;
        PatrolComponent patrolComponent;

        int pathStepIndex = 0;
        
        /// <summary>
        /// Lista degli step (nodi) del path finding attuale.
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
        
        #endregion

        #region Events

        /// <summary>
        /// It occurs when pathfinding grid setup done.
        /// </summary>
        /// <param name="_grid"></param>
        public void OnPathfindingGridSetupDone(PathfindingGrid _grid) {
            patrolComponent = GetComponent<PatrolComponent>();
        }

        #endregion

        #region Init
        void Awake() {
            gizmoColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1.0f);
        }

        public void Setup(PathfindingGrid _grid) {
            grid = _grid;
        }
        #endregion

        #region AI API
        
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
        public void FindPathToTarget(Transform _target) {
            FindPathToTarget(grid.NodeFromWorldPoint(_target.position));
        }

        /// <summary>
        /// Imposta il pathfinding attuale per raggiungere il target e inizia il movimento.
        /// </summary>
        /// <param name="_node"></param>
        public void FindPathToTarget(Node _node) {
            ActivePath = FindPath(transform.position, _node.WorldPosition, AISettings);
            pathStepIndex = 0;
            Debug.LogFormat("... Path from {2}[{3}] to target {0}[{1}].", _node.WorldPosition, _node.PositionOnGrid, transform.position, grid.NodeFromWorldPoint(transform.position).PositionOnGrid);
            if (ActivePath == null) {
                // OnPathEnded();
                Debug.LogFormat("Path from {2}[{3}] to target {0}[{1}] not found!", _node.WorldPosition, _node.PositionOnGrid, transform.position, grid.NodeFromWorldPoint(transform.position).PositionOnGrid);
                return;
            }
            if (ActivePath.Count < 2) {
                Debug.LogFormat("Path count < 2 elements... from {2}[{3}] to {0}[{1}].", _node.WorldPosition, _node.PositionOnGrid, transform.position, grid.NodeFromWorldPoint(transform.position).PositionOnGrid);
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
        /// Inizia a percorrere il path di nodi verso il target.
        /// </summary>
        void moveOnPath() {
            if (pathStepIndex > ActivePath.Count - 1) {
                patrolComponent.OnPatrolPathStepEnded();
                return;
            }
            // Altrimenti proseguo nel percorrere il path
            moveToNode(ActivePath[pathStepIndex]);
        }

        /// <summary>
        /// Effettua l'effettivo movimento dello step attuale del path.
        /// </summary>
        /// <param name="_node"></param>
        void moveToNode(Node _node) {
            Sequence PathSequence = DOTween.Sequence();
            PathSequence.Append(transform.DOMove(_node.WorldPosition, Data.MoveSpeed).OnComplete(delegate() {
                pathStepIndex++;
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
        }

        #endregion
    }

    #region Enemy Data Structure
    /// <summary>
    /// IData Type.
    /// </summary>
    [Serializable]
    public class AIAgentData : IData {
        public string AgentID;
        public float MoveSpeed = 0.5f;
        public bool IsPatroller = true;

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
    #endregion

}