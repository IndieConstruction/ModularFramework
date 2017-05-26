using UnityEngine;
using System;
using Rotorz.Tile;

namespace ModularFramework.AI {

    public class PathFindingGridForRotorzLevel : Grid {

        TileSystem level;

        void Start() {
            Setup();
        }

        public override void Setup() {
            level = GetComponent<TileSystem>();
            // variables init
            NodeRadius = level.CellSize.x;
            GridDimension = new Dimension2d(level.ColumnCount, level.RowCount);
            GridCenterOffset = new Vector2(Mathf.RoundToInt(level.ColumnCount * NodeRadius)/2, -Mathf.RoundToInt(level.RowCount * NodeRadius)/2);
            // grid init
            grid = new Node[GridDimension.H, GridDimension.W];
            for (int r = 0; r < GridDimension.H; r++) {
                for (int c = 0; c < GridDimension.W; c++) {
                    TileData tile = level.GetTile(r, c); // is reversed in rotorz??
                    int nodeType = 1;
                    if (tile != null)
                        if (tile.gameObject.tag == "ground")
                            nodeType = 0;
                        else
                            nodeType = 1;
                    grid[r, c] = new Node(nodeType, new Position2d(r, c), level.WorldPositionFromTileIndex(r, c));
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
