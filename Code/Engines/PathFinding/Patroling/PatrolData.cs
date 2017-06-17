using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.AI {

    /// <summary>
    /// Data structure needed to define patrol behaviour.
    /// </summary>
    [System.Serializable]
    public class PatrolData {
        /// <summary>
        /// List of patrol points ordered by step sequence.
        /// </summary>
        public List<PatrolPoint> PatrolPoints = new List<PatrolPoint>();
    }

    [System.Serializable]
    public class PatrolPoint {
        public int Order;
        public Vector3 Position = new Vector3();
    }
}
