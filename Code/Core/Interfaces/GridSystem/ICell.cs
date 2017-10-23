using UnityEngine;
using DG.Tweening;

namespace ModularFramework.GridSystem {

    public interface ICellModel {
        
    }

    public interface ICellView {
        RectTransform RectTransform { get; }
        IGridView Grid { get; set; }
    }

    #region IGrid interface Extensions

    public static class IGridExtensions {

        public static void MoveTo(this ICellView _this, Position2D _position, bool _allowOutOfBound = false) {
            _this.RectTransform.DOLocalMove(_this.Grid.GetWorldPositionFromGrid(_position, _allowOutOfBound),
                        0.08f
                        ).SetSpeedBased(false);
        }
    }

    #endregion

}