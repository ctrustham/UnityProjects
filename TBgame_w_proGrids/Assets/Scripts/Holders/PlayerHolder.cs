using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SA
{
    //[System.Serializable]
    [CreateAssetMenu(menuName = "Game/Player Holder")]
    public class PlayerHolder : ScriptableObject
    {
        [System.NonSerialized]
        public StateManager stateManager;
        [System.NonSerialized]
        public GameObject stateManagerObject;
        //[SerializeField]
        public GameObject stateManagerPrefab;
        
        [System.NonSerialized] // create a new list each time the game is run (instead of only creating a new list the first time the editor is opened)
        public List<GridCharacter> chars = new List<GridCharacter>();

        public void Init()
        {
            stateManagerObject = Instantiate(stateManagerPrefab) as GameObject;
            stateManager = stateManagerObject.GetComponent<StateManager>();
            stateManager.playerHolder = this; //tell the state manager which player's turn it is at the styart of the game (two way reflection?)
        }


        public void RegisterCharacter(GridCharacter c) // use 'c' to clearly indicate it is a holder var (see below)
        {
            // (here using "if (!chars.Contains(character))" becomes posibly strange to read/unintuitive
            if (!chars.Contains(c)) //if c is not in the array
            {
                chars.Add(c);
            }
        }

        public void UnregisterCharacter(GridCharacter c)
        {
            if (chars.Contains(c)) //if c is in the array
            {
                chars.Remove(c);
            }
        }
    }
}