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
        private Node[,] _nodes;
        public Node[,] Nodes {
            get { return _nodes; }
            set { _nodes = value; }
        }

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

            // Direct call pathfinding grid setup done for any sub component.
            foreach (var pfComponent in GetComponentsInChildren<IPathfindComponent>()) {
                pfComponent.OnPathfindingGridSetupDone(this);
            }

            IsInitialized = true;
        }

        #region Gizmos

        //public List<Node> Path;
        private void OnDrawGizmosSelected() {
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

            foreach (var node in Nodes) {
                if (node == null)
                    continue;
                // Grid
                if (node.NodeType == 0) {
                    GPointSize = 0.8f;
                    Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.5f);
                    Gizmos.DrawCube(node.WorldPosition, new Vector3(NodeRadius * GPointSize, NodeRadius * GPointSize, NodeRadius * GPointSize));
#if UNITY_EDITOR
                    //Handles.color = Color.blue;
                    //Handles.Label(node.WorldPosition + new Vector2(-0.4f, 0.1f), string.Format("{0}", node.PositionOnGrid), new GUIStyle() { alignment = TextAnchor.LowerRight });
                    //Handles.color = Color.black;
                    //Handles.Label(node.WorldPosition + new Vector2(-0.4f, -0.1f), string.Format("{0}:{1}", node.WorldPosition.x, node.WorldPosition.y), new GUIStyle() { alignment = TextAnchor.LowerRight });
#endif
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
        /// Ritorna il nodo alla posizione richiesta.
        /// Accetta anche indici negativi, verranno riconvertiti in base zero per poter essere letti dall'array bidimensionale basandosi sugli estremi del tileset.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Node GetNode(int x, int y) {
            Position2d zeroBasedPosition = GetPositionZeroBased(x, y);
            // Debug.LogFormat("{0}:{1} ({2}:{3})", zeroBasedPosition.x, zeroBasedPosition.y, x, y);
            Node returnNode = Nodes[zeroBasedPosition.x, zeroBasedPosition.y];
            //Debug.LogFormat("{0}:{1} ({2}:{3}) -> {4}", zeroBasedPosition.x, zeroBasedPosition.y, x, y, returnNode.WorldPosition);
            return returnNode;
        }
        
        /// <summary>
        /// Return node point in specific world position.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Node NodeFromWorldPoint(Vector3 worldPosition) {
            int x = Mathf.RoundToInt((
                    (worldPosition.x - transform.position.x)
                    - NodeRadius / 2)
                / NodeRadius);
            int y = Mathf.RoundToInt((
                    (worldPosition.y - transform.position.y)
                    - NodeRadius / 2)
                / NodeRadius);

            // Debug.LogFormat("Target ({0}:{1} => ({2}))", x, y, worldPosition);
            Node returnNode = GetNode(x, y);
            return returnNode;
        }

        public List<Node> GetNeighbours(Node node) {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    if (x == 0 && y == 0)
                        continue;

                    //Node neighbourNode = GetNode(node.PositionOnGrid.x, node.PositionOnGrid.y);

                    int checkX = node.PositionOnGrid.x + x;
                    int checkY = node.PositionOnGrid.y + y;

                    if (checkX >= GridOffSet.x && checkX < GridDimension.x && checkY >= GridOffSet.y && checkY < GridDimension.y) {
                        neighbours.Add(GetNode(checkX, checkY));
                    }
                }
            }

            return neighbours;
        }

        public enum GridDirection { up, up_right, right, down_right, down, down_left, left, up_left }

        public Node GetNeighbour(Node _node, GridDirection _direction) {
            Node returnNode = null;
            // tengo conto del possibile indice negativo
            Position2d realPosition = _node.PositionOnGrid;
            switch (_direction) {
                case GridDirection.left:
                    if (realPosition.x > 0)
                        returnNode = GetNode(realPosition.x - 1, realPosition.y);
                    break;
                //case GridDirection.up_right:
                //    if (_node.PositionOnGrid.Row < GridDimension.W && _node.PositionOnGrid.Col > 0)
                //        returnNode = grid[_node.PositionOnGrid.Row + 1, _node.PositionOnGrid.Col - 1];
                //    break;
                case GridDirection.down:
                    if (realPosition.y > 0)
                        returnNode = GetNode(realPosition.x, realPosition.y - 1);
                    break;
                //case GridDirection.down_right:
                //    if (_node.PositionOnGrid.Row < GridDimension.W && _node.PositionOnGrid.Col < GridDimension.H)
                //        returnNode = grid[_node.PositionOnGrid.Row + 1, _node.PositionOnGrid.Col + 1];
                //    break;
                case GridDirection.right:
                    if (realPosition.x < GridDimension.x - 1)
                        returnNode = GetNode(realPosition.x + 1, realPosition.y);
                    break;
                //case GridDirection.down_left:
                //    if (_node.PositionOnGrid.Row > 0 && _node.PositionOnGrid.Col < GridDimension.H)
                //        returnNode = grid[_node.PositionOnGrid.Row - 1, _node.PositionOnGrid.Col + 1];
                //    break;
                case GridDirection.up:
                    if (realPosition.y < GridDimension.y - 1)
                        returnNode = GetNode(realPosition.x, realPosition.y + 1);
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

        /// <summary>
        /// Restituisce la posizione della griglia con l'offset della griglia. Consente di avere celle con indice negativo. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected Position2d GetPositionWithOffset(int x, int y) {
            int _x = x - Mathf.Abs(GridOffSet.x);
            int _y = y - Mathf.Abs(GridOffSet.y);
            return new Position2d(_x, _y);
        }

        /// <summary>
        /// Trasforma un eventuale posizione, con anche indici negativi, in una coppia di indici base zero (per la lettura dall'array bidimensionale).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected Position2d GetPositionZeroBased(int x, int y) {
            int _x = x + Mathf.Abs(GridOffSet.x);
            int _y = y + Mathf.Abs(GridOffSet.y);
            return new Position2d(_x, _y);
        }

        #endregion

        #region Events

        public delegate void PathFindingGridEventHandler(Grid _grid);

        public event PathFindingGridEventHandler OnSetupDone;

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
            return string.Format("[{0}:{1}]", x, y); 
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