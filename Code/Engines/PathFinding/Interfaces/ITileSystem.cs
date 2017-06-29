using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.AI {

    public interface ITileSystem : ISetupable {

        Vector3 CellSize { get; }
        int RowCount { get; }
        int ColumnCount { get; }
        int MinGridX { get; }
        int MinGridY { get; }
        int MaxGridX { get; }
        int MaxGridY { get; }

        bool TileExist(int row, int col);
        bool IsTraversable(int row, int col);
        Vector3 GetTileWorldPosition(int row, int col);
        
        
    }

    
    
}
