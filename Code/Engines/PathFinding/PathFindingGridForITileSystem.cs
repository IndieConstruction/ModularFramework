using UnityEngine;
using System;

namespace ModularFramework.AI {

    /// <summary>
    /// Grid created for pathfinding based on Tilemap level.
    /// This component must be added to same gameobject with ITileSystem
    /// Features:
    /// - Auto search and subscribe to ITileSystem's OnSetupEnded event (ITileSystem component on same gameobject is needed)
    /// - Build non visual grid with a list of Node for pathfinding
    /// - ExtraNodeInfo to any node adding tags (...TBC)
    /// 
    /// </summary>
    public class PathFindingGridForITileSystem : PathfindingGrid {

        ITileSystem level;

        private void OnEnable() {
            level = GetComponent<ITileSystem>();

            if (level != null) {
                level.OnSetupEnded += Level_OnSetupEnd;
            } else {
                Debug.LogWarningFormat("Component implementing ITileSystem interface not found on gameobject {0}", gameObject.name);
            }
                    
        }

        private void Level_OnSetupEnd(ISetupable tileSystem) {
            Setup();
            AIAgentSetup();
        }

        private void OnDisable() {
            level.OnSetupEnded -= Level_OnSetupEnd;
        }

        /// <summary>
        /// Setup grid for 
        /// </summary>
        public override void Setup() {
            
            // variables init
            NodeRadius = level.CellSize.x;
            GridDimension = new Dimension2d(Mathf.Abs(level.MinGridX) + Mathf.Abs(level.MaxGridX) + 1, Mathf.Abs(level.MinGridY) + Mathf.Abs(level.MaxGridY) + 1);
            GridOffSet = new Dimension2d(level.MinGridX, level.MinGridY);
            //GridCenterOffset = new Vector2(Mathf.RoundToInt(level.ColumnCount * NodeRadius)/2, -Mathf.RoundToInt(level.RowCount * NodeRadius)/2);
            // TODO: leggere dinamicamente
            GridCenterOffset = new Vector2(0, Mathf.RoundToInt(GridDimension.y * NodeRadius) / 2);
            // grid init
            Nodes = new Node[GridDimension.x, GridDimension.y];
            int counter = 0;
            for (int x = 0; x < GridDimension.x; x++) {
                counter++;
                for (int y = 0; y < GridDimension.y; y++) {
                    Position2d normalizedPosition = GetPositionWithOffset(x, y);
                    if (level.TileExist(normalizedPosition.x, normalizedPosition.y)) {
                        Nodes[x, y] = new Node(
                            level.IsTraversable(normalizedPosition.x, normalizedPosition.y),
                            new Position2d(normalizedPosition.x, normalizedPosition.y),
                            level.GetTileWorldPosition(normalizedPosition.x, normalizedPosition.y)
                            );
                    } else {
                        Debug.LogFormat("N ({0},{1}) ({2},{3}) not exist", x,y,normalizedPosition.x, normalizedPosition.y);
                    }
                }
            }

            foreach (Node node in Nodes) {
                extraNodeInfo(node);
            }

            base.Setup();
        }

        void extraNodeInfo(Node _node) {
            // TODO: fix le assi invertite provocano AOR 
            // https://trello.com/c/Fk8oPOxe
            Node nNode = GetNeighbour(_node, GridDirection.down);
            if (nNode != null && nNode.NodeType == 1 && _node.NodeType == 0) {
                _node.AddTag("walkable");
            }
        }

        void AIAgentSetup() {
            foreach (AIAgentComponent agent in GetComponentsInChildren<AIAgentComponent>()) {
                agent.Setup(this);
            }
        }

        #region debug

        private void Update() {
            if (Input.GetKeyDown(KeyCode.LeftAlt)){
                Setup();
            }
        }

        #endregion

    }

}
