using UnityEngine;
using System.Collections;

namespace ModularFramework.AI {

    public class Agent : MonoBehaviour {

        #region Events

        #region subsciptions
        void OnEnable() {
            Grid.OnSetupDone += Grid_OnSetupDone;
        }

        void OnDisable() {
            Grid.OnSetupDone -= Grid_OnSetupDone;
        }
        #endregion

        #region delegates
        private void Grid_OnSetupDone(Grid _grid) {
            Debug.Log("GridSetup Done!");
        }
        #endregion

        #endregion
    }

}
