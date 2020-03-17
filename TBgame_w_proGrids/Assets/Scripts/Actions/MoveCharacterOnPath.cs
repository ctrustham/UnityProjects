using UnityEngine;

namespace SA
{
    public class MoveCharacterOnPath : StateActions
    {
        bool isInitialized;
        float t;
        float rotationT;
        float speed;
        Node startNode;
        Node targetNode;
        int index;
        Quaternion targetRotation; // for character rotation while moving
        Quaternion startingRotation;

        bool firstInit; // for animation

        public override void Execute(StateManager states, SessionManager sm, Turn turn)
        {
            GridCharacter c = states.currChar;
            if (!isInitialized)
            {
                if (c == null || index > c.currPath.Count - 1)
                {
                    states.SetStartingState();
                    return;
                }

                isInitialized = true;
                startNode = c.currentNode;
                targetNode = c.currPath[index];

                // if we go a little eover 1 while moving from node to node then we dont want to cut off a digit?
                // possibly removes stutter/jitter while moving
                float t_ = t - 1;
                t_ = Mathf.Clamp01(t_);
                t = t_;

                //move consistant speed
                float distance = Vector3.Distance(startNode.worldPos, targetNode.worldPos);
                speed = c.GetSpeed() / distance;

                Vector3 direction = targetNode.worldPos - startNode.worldPos;
                targetRotation = Quaternion.LookRotation(direction);
                startingRotation = c.transform.rotation;
                if (!firstInit)
                {
                    c.PlayMovementAnimation();
                    firstInit = true;
                }
            }

            t += states.delta * speed;
            rotationT += states.delta * c.GetSpeed() * 2; // add seperate rotation speed later

            if (rotationT > 1)
            {
                rotationT = 1; // dont want this above 1
            }

            c.transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, rotationT);


            if (t> 1)
            {
                isInitialized = false;

                c.currentNode.character = null;
                c.currentNode = targetNode;
                c.currentNode.character = c; // change the target node to have a character on it

                index++;

                if (index > states.currChar.currPath.Count - 1) // we moved onto our path
                {
                    t = 1;
                    index = 0;

                    states.SetStartingState();
                    c.PlayIdleAnimation();
                    firstInit = false;
                }
            }

            Vector3 targetPos = Vector3.Lerp(startNode.worldPos, targetNode.worldPos, t);
            c.transform.position = targetPos;
        }
    }
}