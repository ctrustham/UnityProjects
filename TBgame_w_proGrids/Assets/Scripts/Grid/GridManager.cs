using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class GridManager : MonoBehaviour
    {
        #region Variables
        Node[,,] grid;
        [SerializeField] //??
        float xzScale = 0.5f; // to change the grid size/scale on the z-x axis
        [SerializeField]
        float yScale = 1;
        Vector3 minPos;
        int maxX, maxY, maxZ;

        public bool visualizeCollisions;

        List<Vector3> nodeViz = new List<Vector3>(); // to show grid
        public Vector3 extends = new Vector3(.5f, .5f, .5f); // how far each tile extends for purposes of colision detection ( the dimensions of the node's cube)

        int pos_x, pos_y, pos_z;


        #endregion

        public GameObject unit; // will change
        public GameObject tileViz;

        public GameObject tileContainer; //container for created tiles

        public void Init()
        {
            tileContainer = new GameObject ("tileContainer");
            ReadLevel();
/*
            Node n = getNode(unit.transform.position);
            if(n != null)
            {
                unit.transform.position = n.worldPos;
            }
           */
        }

        void ReadLevel()
        {
            GridPosition[] gridPos = GameObject.FindObjectsOfType<GridPosition>();

            float minX = float.MaxValue; //use opposite values
            float maxX = float.MinValue;

            float minZ = minX;
            float maxZ = maxX;

            float minY = minX;
            float maxY = maxX;

            for(int i = 0; i < gridPos.Length; i++)
            {
                //create corners for bounds of grid
                Transform t = gridPos[i].transform;
                #region Read positions
                if(t.position.x < minX)
                {
                    minX = t.position.x;
                }
                if (t.position.x > maxX)
                {
                    maxX = t.position.x;
                }
                if (t.position.z < minZ)
                {
                    minZ = t.position.z;
                }
                if (t.position.z > maxZ)
                {
                    maxZ = t.position.z;
                }
                if (t.position.y < minY)
                {
                    minY = t.position.y;
                }
                if (t.position.y > maxY)
                {
                    maxY = t.position.y;
                }
                #endregion
            }

            //sets how many nodes are on each axis
            pos_x = Mathf.FloorToInt((maxX - minX) / xzScale);
            pos_z = Mathf.FloorToInt((maxZ - minZ) / xzScale);
            pos_y = Mathf.FloorToInt((maxY - minY) / yScale);

            if(pos_y == 0)
            {
                pos_y = 1;
            }

            minPos = Vector3.zero;
            minPos.x = minX;
            minPos.z = minZ;
            minPos.y = minY;

            //Debug.Log(pos_y); // to show debug values in unity (in the console tab)

            CreateGrid(pos_x, pos_y, pos_z);
        }

        void CreateGrid(int pos_x, int pos_y, int pos_z)
        {
            grid = new Node[pos_x, pos_y, pos_z]; // set size of array in 3 dimensions
            
            // fill the array
            for(int y = 0; y < pos_y; y++)
            { 
                for(int x = 0; x < pos_x; x++)
                {
                    for (int z = 0; z < pos_z; z++)
                    {
                        Node n = new Node();
                        n.x = x;
                        n.z = z;
                        n.y = y;

                        Vector3 targetPos = minPos;
                        targetPos.x += x * xzScale;// + 0.25f; // shift by 0.5f if using a proGrids scale of 1 (1/2 of progrids scale, to offset centering)
                        targetPos.z += z * xzScale;// + 0.25f;
                        targetPos.y += y * yScale;

                        n.worldPos = targetPos; // set/record the node's position in the world

                        Vector3 collisionPosition = targetPos;// + Vector3.up* 0.25f;
                        collisionPosition.y += yScale / 2;

                        // ------ offset for floor position (to stand on top of rubble, etc.) -----------
                        RaycastHit hit;
                        Vector3 origin = n.worldPos;
                        origin.y += yScale - .1f;

                        // Debug.DrawRay(origin, Vector3.down * (yScale - .1f), Color.blue, 10);

                        if (Physics.Raycast(origin, Vector3.down, out hit, yScale - .1f))
                        {
                            GridObject gridObj = hit.transform.GetComponent<GridObject>();
                            if (gridObj != null)
                            {
                                if (gridObj.isWalkable_)
                                {
                                    n.isWalkable = true;
                                }
                                n.worldPos = hit.point;
                            }
                        }
                        //--------------------------------------------------------------------------
                        
                        //to determine if the node is walkable
                        Collider[] overlapNode = Physics.OverlapBox(collisionPosition, extends, Quaternion.identity); // find all colliders inside the given box - find everything that collides with the position of our node // Quanternion ?                                                                               //need to know what is "ground"

                        if (overlapNode.Length > 0)
                        {

                            for (int i = 0; i < overlapNode.Length; i++)
                            {
                                GridObject obj = overlapNode[i].transform.GetComponentInChildren<GridObject>();
                                if (obj != null)
                                {
                                    if (obj.isWalkable_ && n.obstacle == null)
                                    {
                                       // n.isWalkable_ = true;
                                    }
                                    else
                                    {
                                        n.isWalkable = false;
                                        n.obstacle = obj;
                                    }
                                }
                            }
                            
                        }

                        //if (n.obstacle != null) //only add non-walkable nodes to visualization
                            if(n.isWalkable) // to see only upper levels change to && y > 0
                        {
                            GameObject gameObj = Instantiate(tileViz, n.worldPos + Vector3.up * 0.05f, Quaternion.identity) as GameObject;  // n.worldPos + Vector3.one * .1f
                            n.tileViz = gameObj; //allows the tile to know its own visualization
                            gameObj.transform.parent = tileContainer.transform; //save new tiles in a container
                            gameObj.SetActive(true);

                            // nodeViz.Add(collisionPosition); // n.worldPos); 
                        }
                        else
                        {
                            if(n.obstacle == null)
                            {
                                n.isAir = true;
                            }
                        }
                        nodeViz.Add(collisionPosition);
                        grid[x, y, z] = n; // place the node in the array
                    }
                }
            }
        }

        public Node getNode(Vector3 worldPos)
        {
            Vector3 p = worldPos - minPos; // remove offset from center of world (minPos's rarely fall on 0,0,0 for ex) -> actual position relative to grid (local position)
            int x = Mathf.RoundToInt(p.x / xzScale); 
            int y = Mathf.RoundToInt(p.y / yScale);
            int z = Mathf.RoundToInt(p.z / xzScale);

            return getNode(x, y, z);
        }

        public Node getNode(int x, int y, int z)
        {
            //check for out of bounds
            if (x < 0 || x > pos_x - 1 || y < 0 || y > pos_y - 1 || z < 0 || z > pos_z - 1)
            {
                return null;
            }

            return grid[x, y, z];
        }

        private void OnDrawGizmos() //internal call in unity
        {
            if (visualizeCollisions)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < nodeViz.Count; i++)
                {
                    Gizmos.DrawWireCube(nodeViz[i], extends); // (nodeViz[i], extends) maybe problem
                }
            }
        }

    }
}