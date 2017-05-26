using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.AI {

    public interface ITileSystem {

        Vector3 CellSize { get; }
        int RowCount { get; }
        int ColumnCount { get; }
        int RowOffset { get; }
        int ColumnOffset { get; }

        bool TileExist(int row, int col);
        bool IsTileWalkable(int row, int col);
        Vector3 GetTileWorldPosition(int row, int col);

        event TileSystemEvent OnSetupEnd;
    }

    public delegate void TileSystemEvent(ITileSystem tileSystem);
    
}
