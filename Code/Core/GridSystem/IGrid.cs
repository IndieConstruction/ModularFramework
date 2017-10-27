using UnityEngine;

namespace ModularFramework.GridSystem {

    public interface IGridModel {

    }

    public interface IGridView {
        Vector2 GetWorldPositionFromGrid(Position2D _position, bool _allowOutOfBound);
        IGridModel GetModel { get; }
    }

}