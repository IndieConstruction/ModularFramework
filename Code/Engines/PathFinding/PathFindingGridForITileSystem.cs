using UnityEngine;
using System;

namespace ModularFramework.AI {

    public class PathFindingGridForITileSystem : Grid {

        ITileSystem level;

        private void OnEnable() {
            level = GetComponent<ITileSystem>();
            if (level != null) {
                level.OnSetupEnd += Level_OnSetupEnd;
            }
        }

        private void Level_OnSetupEnd(ITileSystem tileSystem) {
            Setup();
        }

        private void OnDisable() {
            level.OnSetupEnd -= Level_OnSetupEnd;
        }

        /// <summary>
        /// Setup grid for 
        /// </summary>
        public override void Setup() {
            
            // variables init
            NodeRadius = level.CellSize.x;
            GridDimension = new Dimension2d(level.ColumnCount, level.RowCount);
            // TODO: Usare limiti massimi e minimi per ogni dimensione
            GridOffSet = new Dimension2d(level.ColumnOffset, level.RowOffset);
            //GridCenterOffset = new Vector2(Mathf.RoundToInt(level.ColumnCount * NodeRadius)/2, -Mathf.RoundToInt(level.RowCount * NodeRadius)/2);
            // TODO: leggere dinamicamente
            GridCenterOffset = new Vector2(0, Mathf.RoundToInt(level.RowCount * NodeRadius) / 2);
            // grid init
            grid = new Node[GridDimension.H, GridDimension.W];
            for (int r = 0; r < GridDimension.H; r++) {
                for (int c = 0; c < GridDimension.W; c++) {
                    int nodeType = 1;
                    if (level.TileExist(r,c))
                        if (level.IsTileWalkable(r,c))
                            nodeType = 0;
                        else
                            nodeType = 1;
                    grid[r, c] = new Node(nodeType, new Position2d(r, c), level.GetTileWorldPosition(r, c));// - (Vector3)GridCenterOffset);
                }
            }

            foreach (Node node in grid) {
                extraNodeInfo(node);
            }

            base.Setup();
        }

        void extraNodeInfo(Node _node) {
            Node nNode = GetNeighbour(_node, GridDirection.down);
            if (nNode != null && nNode.NodeType == 0 && _node.NodeType == 1) {
                _node.AddTag("walkable");
            }
        }

    }

}
