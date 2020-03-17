using UnityEngine;
using System.Collections;
using System.Collections.Generic; //allows use of List<>

namespace SA
{
    public class Pathfinder
    {
        GridManager gridManager;
        GridCharacter character;
        Node startNode;
        Node endNode;
        List<Node> targetPath;

        public volatile float timer;
        //volatile - used for multi threading -> it can be changed outside the current context -> getting the latest value it has when used
        public volatile bool jobDone = false;
        

        public delegate void PathfindingComplete(List<Node> n, GridCharacter character);
        public PathfindingComplete completeCallback;

        // constructor for pathfinder object
        public Pathfinder(GridCharacter c, Node start, Node target, PathfindingComplete callback, GridManager gm)
        {
            this.gridManager = gm;
            character = c;
            startNode = start;
            endNode = target;
            completeCallback = callback;
        }

        // public facing find-path method
        public void FindPath()
        {
            targetPath = FindPathActual();
            jobDone = true;
        }

        public void NotifyComplete()
        {
            //Debug.Log("NotifyComplete()");
            completeCallback?.Invoke(targetPath, character); // equivalent to below
            /*if (completeCallback != null)
            {
                completeCallback(targetPath, character);
            }*/
        }

        // private method that actually finds the path
        List<Node> FindPathActual()
        {
            //using basic A* methodology
            List<Node> foundPath = new List<Node>(); // the final completed list representing the path
            List<Node> openSet = new List<Node>(); // The set of currently discovered nodes that are not evaluated yet.
                                                   // Initially, only the start node is known.
            HashSet<Node> closedSet = new HashSet<Node>(); // the list of nodes that have been evaluated

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currNode = openSet[0];

                // current:= the node in openSet having the lowest fScore value
                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currNode.fCost ||
                        (openSet[i].fCost == currNode.fCost && openSet[i].hCost < currNode.hCost))
                    {
                        if (!currNode.Equals(openSet[i]))
                        {
                            currNode = openSet[i];
                        }
                    }
                }

                openSet.Remove(currNode);
                closedSet.Add(currNode);

                //
                if (currNode.Equals(endNode))
                {
                    foundPath = RetracePath(startNode, currNode);
                    break;
                }

                foreach (Node neighbour in GetNeighbours(currNode))
                {
                    if (!closedSet.Contains(neighbour)) // if the neighbour has not been checked
                    {
                        float newMovementCostToNeighbour = currNode.gCost + GetDistance(currNode, neighbour);

                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, endNode);
                            neighbour.parentNode = currNode;
                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                        }
                    }
                }
            }

            return foundPath;
        }


        // traverse the path from end to start, return the path from start to finish
        List<Node> RetracePath(Node start, Node end)
        {
            List<Node> path = new List<Node>();
            Node currNode = end; // start at the end
            while (currNode != start)
            {
                path.Add(currNode);
                currNode = currNode.parentNode; // traverse towards the start of the path -> parent node is closer to start then child, and penultimate parent is the start node
            }
            path.Reverse(); // reverse the list so it goes from start to end
            return path;
        }

        List<Node> GetNeighbours(Node node)
        {
            List<Node> returnList = new List<Node>();

            //check each node in a 3x3 (x3?) area
            for (int x = -1; x <= 1; x++)
            {
             //   for (int y = -1; y <= 1; y++)
              //  {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x == 0 && z == 0) // ignore the middle (the current node)
                        {
                            continue;
                        }

                        int _x = x + node.x;
                        int _y = startNode.y;
                        int _z = z + node.z;

                        Node n = GetNode(_x, _y, _z); // only checking on the same lavel as the current node
                    if (n != null)
                    {
                        Node newNode = GetNeighbour(n);

                        if (newNode != null)
                        {
                            returnList.Add(newNode);
                        }
                    }
                    }
               // }
            }
            return returnList;
        }

        Node GetNode(int x, int y, int z)
        {
            return gridManager.getNode(x, y, z);
        }


        // Just checks if node is walkable atm
        Node GetNeighbour(Node n)
        {
            Node returnVal = null;

            if (n.isWalkable)
            {
                Node aboveNode = GetNode(n.x, n.y + 1, n.z);
                if (aboveNode == null || aboveNode.isAir || character.isCrouched) // if there is nothing at torso level blocking movement, or the character is crouching to go under it
                {
                    returnVal = n;
                }
                else
                {
                    
                }
                
            }

            return returnVal;
        }

        // taken from Wiki for A*
        int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Mathf.Abs(nodeA.x - nodeB.x);
            int distZ = Mathf.Abs(nodeA.z - nodeB.z);

            if(distX > distZ)
            {
                return 14 * distZ + 10 * (distX - distZ);
            }
            return 14 * distX + 10 * (distZ - distX);
        }
    }
}