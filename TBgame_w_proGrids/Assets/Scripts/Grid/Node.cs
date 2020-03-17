using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class Node
    {
        public int x, y, z; // the actual x y z of the tile, not relative to the world

        public bool isAir;
        public bool isWalkable;
        public Vector3 worldPos; // Tile's Position in relation to the world
        //public Vector3 floorPos;
        public GridObject obstacle; // object or obstacle occupying the node
        public GameObject tileViz; // vizualization for tile, in case we need later
        public GridCharacter character; // the character occupying the node

        // A* costs
        public float hCost;
        public float gCost;
        public float fCost
        {
            get
            {
                return hCost + gCost;
            }
        }

        public Node parentNode; 
    }
}