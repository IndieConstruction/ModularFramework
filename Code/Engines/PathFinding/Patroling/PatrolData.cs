using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.AI {

    /// <summary>
    /// Data structure needed to define patrol behaviour.
    /// </summary>
    [System.Serializable]
    public class PatrolData {

        public List<PatrolPoint> PatrolPoints = new List<PatrolPoint>();
        //public PatrolPoint PatrolPoint;

    }
    [System.Serializable]
    public class PatrolPoint {
        public int Order;
        public Vector3 Position = new Vector3();
    }
}
