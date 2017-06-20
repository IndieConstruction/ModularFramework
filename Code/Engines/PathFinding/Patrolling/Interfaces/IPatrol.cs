using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularFramework.Data;

namespace ModularFramework.AI {

    /// <summary>
    /// Add patrol functionality.
    /// </summary>
    public interface IPatrol : IDataContainer {
        IPatrol PatrollerComponent{ get; }
        //Transform transform { get; }
        //List<Vector3> PatrolPoints { get; set; }
        //List<Node> PatrolPath { get; set; }
        PatrolData GetPatrolData();
    }

}