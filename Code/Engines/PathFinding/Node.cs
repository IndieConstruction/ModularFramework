using UnityEngine;
using System.Collections;
using System;
using System.Linq;

namespace ModularFramework.AI {

    public class Node {
        /// <summary>
        /// Node type. In simplest implementation 0 = notWalkable, 1 = walkable.
        /// </summary>
        public int NodeType;

        /// <summary>
        /// Position on grid.
        /// </summary>
        public Position2d PositionOnGrid;

        /// <summary>
        /// Position of node in world space.
        /// </summary>
        public Vector2 WorldPosition;

        /// <summary>
        /// Parent node link.
        /// </summary>
        public Node Parent;

        /// <summary>
        /// Distance cost from Start point of pathfinding.
        /// </summary>
        public int G_Cost;

        /// <summary>
        /// Dinstance cost from Goal point of pathfinding.
        /// </summary>
        public int H_Cost;

        /// <summary>
        /// Full cost of node;
        /// </summary>
        public int F_Cost {
            get { return G_Cost + H_Cost; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="_isTraversable"></param>
        /// <param name="_position"></param>
        public Node(bool _isTraversable, Position2d _position, Vector3 _worldPosition) {
            if (_isTraversable)
                AddTag("traversable");
            PositionOnGrid = _position;
            WorldPosition = _worldPosition;
        }

        #region Tags
        /// <summary>
        /// Tags:
        /// traversable, walkable
        /// </summary>
        public string[] Tags = new string[] { };

        public void AddTag(string _tag) {
            if (ContainsTag(_tag))
                return;
            Array.Resize(ref Tags, Tags.Length +1);
            Tags[Tags.Length - 1] = _tag;
        }

        public void RemoveTag(string _tag) {
            if (!ContainsTag(_tag))
                return;
            Tags = Tags.Where(val => val != _tag).ToArray();
        }

        public bool ContainsTag(string _tag) {
            return Tags.Contains(_tag);
        }
        #endregion

    }


}