using UnityEngine;
using System;

namespace ModularFramework.AI {

    public class PathFindingGridForITileSystem : Grid {

        ITileSystem level;

        private void OnEnable() {
            level = GetComponent<ITileSystem>();
            if (level != null) {
                level.OnSetupEnded += Level_OnSetupEnd;
            }
        }

        private void Level_OnSetupEnd(ISetupable tileSystem) {
            Setup();
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
            GridDimension = new Dimension2d(Mathf.Abs(level.MinGridX) + Mathf.Abs(level.MaxGridX), Mathf.Abs(level.MinGridY) + Mathf.Abs(level.MaxGridY));
            // TODO: Usare limiti massimi e minimi per ogni dimensione
            GridOffSet = new Dimension2d(level.MaxGridX - level.MinGridX, level.MaxGridY - level.MinGridY);
            //GridCenterOffset = new Vector2(Mathf.RoundToInt(level.ColumnCount * NodeRadius)/2, -Mathf.RoundToInt(level.RowCount * NodeRadius)/2);
            // TODO: leggere dinamicamente
            GridCenterOffset = new Vector2(0, Mathf.RoundToInt(GridDimension.y * NodeRadius) / 2);
            // grid init
            grid = new Node[GridDimension.x, GridDimension.y];
            for (int x = 0; x < GridDimension.x; x++) {
                for (int y = 0; y < GridDimension.y; y++) {
                    Position2d normalizedPosition = GetPositionWithOffset(x, y);
                    if (normalizedPosition.x == -9 || normalizedPosition.x == -8) {
                        //Debug.Log("");
                    }
                    int nodeType = 1;
                    if (level.TileExist(normalizedPosition.x, normalizedPosition.y)) {
                        if (level.IsTraversable(normalizedPosition.x, normalizedPosition.y))
                            nodeType = 0;
                        else
                            nodeType = 1;

                        grid[x, y] = new Node(
                            nodeType,
                            new Position2d(normalizedPosition.x, normalizedPosition.y),
                            level.GetTileWorldPosition(x, y) 
                            - new Vector3(Mathf.RoundToInt(GridOffSet.x * NodeRadius) / 2, 0, 0)
                            );
                    } else {
                        Debug.LogFormat("N ({0},{1}) ({2},{3}) not exist", x,y,normalizedPosition.x, normalizedPosition.y);
                    }
                }
            }

            // TODO: fix le assi invertite provocano AOR 
            //foreach (Node node in grid) {
            //    extraNodeInfo(node);
            //}

            base.Setup();
        }

        Position2d GetPositionWithOffset(int x, int y) {
            return new Position2d(x + level.MinGridX, y + level.MinGridY);
        }

        void extraNodeInfo(Node _node) {
            Node nNode = GetNeighbour(_node, GridDirection.down);
            if (nNode != null && nNode.NodeType == 0 && _node.NodeType == 1) {
                _node.AddTag("walkable");
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
