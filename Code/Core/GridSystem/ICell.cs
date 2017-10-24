using UnityEngine;
using DG.Tweening;

namespace ModularFramework.GridSystem {

    public interface ICellModel {
        Position2D Position { get; set; }
    }

    public interface ICellView {
        Position2D Position { get; set; }
        Vector2 LocalPosition { get; }
        RectTransform RectTransform { get; }
        IGridView Grid { get; set; }
    }

    #region IGrid interface Extensions

    public static class IGridExtensions {

    }

    #endregion

}