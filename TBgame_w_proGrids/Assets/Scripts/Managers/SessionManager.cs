using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SessionManager : MonoBehaviour
    {
        int turnIndex;
        public Turn[] turns;

        public GridManager gridManager;
        bool isInit;

        public float delta;



        // public Node currNode; //session manager (esp for cur player) is going to be treated as the data container for the local player
        #region Initialization
        private void Start()
        {
            gridManager.Init();
            PlaceUnits();
            InitStateManagers();
            isInit = true;
        }

        void InitStateManagers()
        {
            foreach (Turn t in turns)
            {
                t.player.Init();
            }
        }

        void PlaceUnits()
        {
            // create an array of GridCharacter (our creation) objects and fill it with the list
            // of GridCharacter (unity object) objects found within the GameObject
            GridCharacter[] units = GameObject.FindObjectsOfType<GridCharacter>();
            foreach (GridCharacter u in units)
            {
                u.OnInit();
                //Debug.Log("Found an object");
                Node n = gridManager.getNode(u.transform.position);
                if (n != null)
                {
                    u.transform.position = n.worldPos;
                    n.character = u;
                    u.currentNode = n;
                }
            }
        }
        #endregion

        #region Pathfinding
        public LineRenderer pathVis;
        bool isPathFinding;

        public void PathfinderCall(GridCharacter character, Node targetNode)
        {
            if (!isPathFinding)
            {
                isPathFinding = true;

                Node start = character.currentNode;
                Node target = targetNode;

                if (start != null && target != null)
                {
                    PathfinderMaster.masterPathfinder.RequestPathFind(character, start, target, PathfinderCallback, gridManager);
                }
                else
                {
                    isPathFinding = false;
                }
            }
        }

        void PathfinderCallback(List<Node> p, GridCharacter c)
        {
            //Debug.LogWarning("Path CallBack, p.count = " + p.Count);
            isPathFinding = false;
            if (p == null)
            {
                //Debug.LogWarning("Path not valid");
                return;
            }

            pathVis.positionCount = p.Count + 1; // tell the line how many positions/coordinates to contain
            List<Vector3> allPositions = new List<Vector3>();
            allPositions.Add(c.currentNode.worldPos + Vector3.up * 0.2f); // add the initial node to start the path
            for (int i = 0; i < p.Count; i++) // load all the path positions
            {
                allPositions.Add(p[i].worldPos + Vector3.up * 0.2f);
            }

            c.LoadPath(p); // tell the character what path to take
            pathVis.SetPositions(allPositions.ToArray()); // draw a line using the loaded positions to show the path
        }

        public void ClearPath(StateManager states)
        {
            pathVis.positionCount = 0; // reset the path (when switching characters for example)
            if (states.currChar != null)
            {
                states.currChar.currPath = null;
            }
        }
        #endregion

        private void Update()
        {
            if (!isInit){
                return;
            }

            delta = Time.deltaTime;

            if (turns[turnIndex].Execute(this)) //if turn is over
            {
                turnIndex++;
                if(turnIndex > turns.Length - 1)
                {
                    turnIndex = 0;
                }
            }
        }
    }
}