using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.AI {

    /// <summary>
    /// Add patrol functionality.
    /// </summary>
    public interface IPatroller : IAgent {
        Transform transform { get; }
        //PatrolData Data { get; set; }
        List<Vector3> PatrolPoints { get; set; }
        List<Node> PatrolPath { get; set; }
    }

}