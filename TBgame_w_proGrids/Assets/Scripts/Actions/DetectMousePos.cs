using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class HandleMouseInteractions : StateActions //abstract class must be implemented
    {
        GridCharacter prevChar;

        /// <summary>
        /// Overrides StateActions.Exectue
        /// <para>
        /// Defines what execute will do when mouse position is detected
        /// </para>
        /// </summary>
        /// <param name="states"></param>
        /// <param name="sm"></param>
        /// <param name="t"></param>
        public override void Execute(StateManager states, SessionManager sm, Turn t)
        {
            bool mouseClick = Input.GetMouseButtonDown(0); // left click

            if (prevChar != null)
            {
                prevChar.OnDeHighlight(states.playerHolder);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; // create an instance of a raycast hit - ie information about the colision
            if (Physics.Raycast(ray, out hit, 10000)) // shoot the ray out 1000 units and store the result in "hit"
            {
                Node n = sm.gridManager.getNode(hit.point);

                IDetectable detectable = hit.transform.GetComponent<IDetectable>();

                if (detectable != null) // we probably hit a character but the mouse is telling us that we hit a different nodde
                {
                    n = detectable.OnDetect(); // overrides the node to detect the capsule collider hit from the mouse (allows selection of a character)
                }

                if (n != null)
                {
                    if (n.character != null)
                    {
                        if (n.character.owner == states.playerHolder) // highlighted your own character
                        {
                            n.character.OnHighlight(states.playerHolder);
                            prevChar = n.character;
                            sm.ClearPath(states);
                        }
                        else // highlighted an enemy unit
                        {

                        }
                    }


                    if (states.currChar != null && n.character == null) // as long as there is a character seleccted, and the space is empty
                    {
                        if (mouseClick)
                        {
                            if (states.currChar.currPath != null || states.currChar.currPath.Count > 0)
                            {
                                states.setState("moveOnPath");
                            }
                        }
                        else
                        {
                            PathDetection(states, sm, n);
                        }
                    }
                    else
                    {
                        if (mouseClick)
                        {
                            if (n.character != null)
                            {
                                if (n.character.owner == states.playerHolder) // highlighted your own character
                                {
                                    n.character.OnSelect(states.playerHolder);
                                    states.prevNode = null; // forces a new path detection calculation because the previous node is not the same as the current node
                                    sm.ClearPath(states);
                                }
                            }
                            else
                            {
                                if (states.currChar)
                                {

                                }
                            }
                        }
                    }
                }
                else
                {
                    //Debug.Log("Node not found");
                }
            }
        }

        void PathDetection(StateManager states, SessionManager sm, Node n)
        {
            states.currNode = n;
            if (states.currNode != null)
            {
                //Debug.Log("-------------Node found");
                if (states.currNode != states.prevNode || states.prevNode == null) // remove || states.prevNode == null ?
                {
                    states.prevNode = states.currNode;
                    sm.PathfinderCall(states.currChar, states.currNode);
                }
            }
        }
    }
}