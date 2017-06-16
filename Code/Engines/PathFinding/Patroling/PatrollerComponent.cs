using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularFramework.AI {

    public class PatrollerComponent : MonoBehaviour, IPatroller {

        public PatrolData Data;
        public List<Vector3> PatrolPoints { get; set; }
        public List<Node> PatrolPath { get; set; }

        public void Setup(Grid grid) {  
            
        }
    }
}
