using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace ModularFramework.AI {
    /// <summary>
    /// Rappresenta la grilia dati utilizzata per il pathfinding.
    /// </summary>
    public abstract class Grid : MonoBehaviour {
        public LayerMask UnwalkableMask;
        public Dimension2d GridDimension;
        public Dimension2d GridOffSet;
        public Vector2 GridWorldSize;
        public Vector2 GridCenterOffset;
        public float NodeRadius;
        /// <summary>
        /// Row / Col
        /// </summary>
        public Node[,] grid;

        public bool IsInitialized;

        #region Gizmos
        [Header("Gizmos Settings")]
        public bool PathFindingGizmos = false;
        public bool TagsGizmos = false;
        protected Color GizmoColor;
        #endregion

        #region Pathfinding system settins
        public bool DrawGizmos;
        #endregion

        /// <summary>
        /// Esegue il setup della griglia caricando i nodi con tutte le info senza parametri esterni.
        /// Per eseguire il setup corretto è necessario provvedere a:
        /// - fornire il corretto dimensionamento del NodeRadius
        /// - fornire la dimensione della griglia
        /// </summary>
        public virtual void Setup() {
            GridWorldSize = new Vector2(GridDimension.x * NodeRadius, GridDimension.y * NodeRadius);

            if (OnSetupDone != null)
                OnSetupDone(this);
            IsInitialized = true;
        }

        #region Gizmos

        //public List<Node> Path;
        void OnDrawGizmos() {
            if (!DrawGizmos || !IsInitialized)
                return;
            float GPointSize = 1;

            // Grid bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(
                transform.position + (Vector3)GridCenterOffset, 
                new Vector3(Mathf.RoundToInt(GridDimension.x * NodeRadius),
                            Mathf.RoundToInt(GridDimension.y * NodeRadius), 
                            1)
                );

            foreach (var node in grid) {
                if (node == null)
                    continue;
                // Grid
                if (node.NodeType == 0) {
                    GPointSize = 0.8f;
                    Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.5f);
                    Gizmos.DrawCube(node.WorldPosition, new Vector3(NodeRadius * GPointSize, NodeRadius * GPointSize, NodeRadius * GPointSize));
                } else {
                    GPointSize = 0.8f;
                    Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
                    Gizmos.DrawCube(node.WorldPosition, new Vector3(NodeRadius * GPointSize, NodeRadius * GPointSize, NodeRadius * GPointSize));
                }
                if (TagsGizmos) { 
                    if (node.ContainsTag("walkable")) {
                        GPointSize = 0.5f;
                        Gizmos.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.75f);
                        Gizmos.DrawCube(node.WorldPosition, new Vector3(NodeRadius * GPointSize, NodeRadius * GPointSize, NodeRadius * GPointSize));
                    }
                }

            }


        }

        #endregion

        #region API
        /// <summary>
        /// Return node point in specific world position.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Node NodeFromWorldPoint(Vector3 worldPosition) {
            int row = Mathf.Abs(Mathf.RoundToInt((worldPosition.y + NodeRadius / 2) / NodeRadius));
            int col = Mathf.Abs(Mathf.RoundToInt((worldPosition.x - NodeRadius / 2) / NodeRadius));
            // for visual test in gizmos
            return grid[row, col];
        }

        public List<Node> GetNeighbours(Node node) {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.PositionOnGrid.y + x;
                    int checkY = node.PositionOnGrid.x + y;

                    if (checkX >= 0 && checkX < GridDimension.x && checkY >= 0 && checkY < GridDimension.y) {
                        neighbours.Add(grid[checkY, checkX]);
                    }
                }
            }

            return neighbours;
        }

        public enum GridDirection { up, up_right, right, down_right, down, down_left, left, up_left }
        public Node GetNeighbour(Node _node, GridDirection _direction) {
            Node returnNode = null;
            switch (_direction) {
                case GridDirection.left:
                    if (_node.PositionOnGrid.y > 0)
                        returnNode = grid[_node.PositionOnGrid.x, _node.PositionOnGrid.y - 1];
                    break;
                //case GridDirection.up_right:
                //    if (_node.PositionOnGrid.Row < GridDimension.W && _node.PositionOnGrid.Col > 0)
                //        returnNode = grid[_node.PositionOnGrid.Row + 1, _node.PositionOnGrid.Col - 1];
                //    break;
                case GridDirection.down:
                    if (_node.PositionOnGrid.x < GridDimension.y -1)
                        returnNode = grid[_node.PositionOnGrid.x + 1, _node.PositionOnGrid.y];
                    break;
                //case GridDirection.down_right:
                //    if (_node.PositionOnGrid.Row < GridDimension.W && _node.PositionOnGrid.Col < GridDimension.H)
                //        returnNode = grid[_node.PositionOnGrid.Row + 1, _node.PositionOnGrid.Col + 1];
                //    break;
                case GridDirection.right:
                    if (_node.PositionOnGrid.y < GridDimension.x -1)
                        returnNode = grid[_node.PositionOnGrid.x, _node.PositionOnGrid.y + 1];
                    break;
                //case GridDirection.down_left:
                //    if (_node.PositionOnGrid.Row > 0 && _node.PositionOnGrid.Col < GridDimension.H)
                //        returnNode = grid[_node.PositionOnGrid.Row - 1, _node.PositionOnGrid.Col + 1];
                //    break;
                case GridDirection.up:
                    if (_node.PositionOnGrid.x > 0)
                        returnNode = grid[_node.PositionOnGrid.x - 1, _node.PositionOnGrid.y];
                    break;
                //case GridDirection.up_left:
                //    if (_node.PositionOnGrid.Row > 0 && _node.PositionOnGrid.Col > 0)
                //        returnNode = grid[_node.PositionOnGrid.Row - 1, _node.PositionOnGrid.Col - 1];
                //    break;
                default:
                    break;
            }
            return returnNode;
        }
        #endregion

        #region Events

        public delegate void PathFindingGridEventHandler(Grid _grid);

        public static event PathFindingGridEventHandler OnSetupDone;

        #endregion
    }

    #region structs for grid evn

    public struct Position2d {
        public int x;
        public int y;
        public Position2d(int _x, int _y) {
            x = _x;
            y = _y;
        }
        public override string ToString() {
            return string.Format("(x:{0},y:{1})", x, y); 
        }

    }

    public struct Dimension2d {
        public int x;
        public int y;
        public Dimension2d(int _x, int _y) {
            x = _x;
            y = _y;
        }

    }

    #endregion 

}