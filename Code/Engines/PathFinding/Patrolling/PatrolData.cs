﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ModularFramework.Data;

namespace ModularFramework.AI {

    /// <summary>
    /// Data structure needed to define patrol behaviour.
    /// </summary>
    [Serializable]
    public class PatrolData : IData {
        /// <summary>
        /// List of patrol points ordered by step sequence.
        /// </summary>
        public List<PatrolPoint> PatrolPoints = new List<PatrolPoint>();
        public PatrolType Type = PatrolType.PATHFINDING;
        public Vector3 Position;
    }

    public enum PatrolType {
        PATHFINDING = 10,
        LINEAR = 20,
    }

    [System.Serializable]
    public class PatrolPoint {
        public int Order;
        public Vector3 Position = new Vector3();
    }
}
