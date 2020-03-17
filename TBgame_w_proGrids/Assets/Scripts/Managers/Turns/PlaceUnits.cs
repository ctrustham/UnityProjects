/* This is not needed - was used for practice, but unit placement is done within the "Session Manager" to place units at the start of the game
using UnityEngine;
using System.Collections;

namespace SA
{
    [CreateAssetMenu(menuName ="Phases/Place Units")] //needed for scriptable objects
    public class PlaceUnits : Phase
    {
        public override bool isComplete(Session_Manager sm)
        {
            //PlaceUnitsOnGrid(sm); //2 ways to do logic - this or override
            return true;
        }

        public override void OnStartPhase(Session_Manager sm)
        {
            if (isInit)
                return;
            isInit = true;
             
           PlaceUnitsOnGrid(sm);
        }

        public override void OnEndPhase(Session_Manager sm)
        {
            
        }

        void PlaceUnitsOnGrid(Session_Manager sm)
        {
            GridCharacter[] units = sm.gridObject.GetComponentsInChildren<GridCharacter>();

            Debug.Log("PlaceUnitsOnGrid");
            foreach (GridCharacter u in units)
            {
                //Debug.Log(u.name);
                Node n = sm.gridManager.Getnode(u.transform.position);
                if(n != null)
                {
                    u.transform.position = n.worldPos;
                }
            }
        }
    }
}
*/