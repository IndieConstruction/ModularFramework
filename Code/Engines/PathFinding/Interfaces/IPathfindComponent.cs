using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.AI {
    /// <summary>
    /// Interfaccia comune a tutti i componeti che lavorano con il sistema di pathfinding.
    /// </summary>
    public interface IPathfindComponent {

        /// <summary>
        /// Viene chiamata al termine del setup della grid del pathfinding.
        /// </summary>
        /// <param name="_grid"></param>
        void OnPathfindingGridSetupDone(Grid _grid);
    }
}