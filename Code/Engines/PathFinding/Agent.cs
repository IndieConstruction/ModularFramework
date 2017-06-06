using UnityEngine;
using System.Collections;

namespace ModularFramework.AI {

    public class Agent : MonoBehaviour {

        Grid grid;

        #region Events

        #region subsciptions
        void OnEnable() {
            
        }

        void OnDisable() {
            grid.OnSetupDone -= Grid_OnSetupDone;
        }
        #endregion

        #region delegates
        private void Grid_OnSetupDone(Grid _grid) {
            Debug.Log("GridSetup Done!");
            grid = _grid;
            grid.OnSetupDone += Grid_OnSetupDone;
        }
        #endregion

        #endregion
    }

}
