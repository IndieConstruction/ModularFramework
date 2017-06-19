using System;
using System.Collections;
using System.Collections.Generic;
using ModularFramework.Data;
using UnityEngine;

namespace ModularFramework.AI {

    public class PatrollerComponent : MonoBehaviour, IPatroller {

        public PatrolData Data;
        public List<Vector3> PatrolPoints { get; set; }
        public List<Node> PatrolPath { get; set; }

        IPatroller IPatroller.PatrollerComponent { get { return this; } }

        public IData GetDataForSave() {
            return Data;
        }

        public PatrolData GetPatrolData() {
            return Data;
        }

        public void Setup(Grid grid) {
            
        }
    }
}
