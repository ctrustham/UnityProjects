using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class StateManager : MonoBehaviour
    {
        public State currState;
        public State startingState;
        public bool forceExit;

        public Node currNode;
        public Node prevNode;
        public float delta;

        //being put in here instead of player holder because that is a scriptable object and so would need to have too many non-serialized fields
        public PlayerHolder playerHolder;
        GridCharacter _currentChar;
        public GridCharacter currChar{ //the currently selected/active character
            get {
                return _currentChar;
            }
            set{
                if (_currentChar != null) //if there is a character already selected then deselect/unhighlight it
                {
                    _currentChar.OnDeselect(playerHolder);
                }
                _currentChar = value;
            }
        }

        protected Dictionary<string, State> allStates = new Dictionary<string, State>(); // holds all the states so we can transition from one to another, and are able to refer to the sate by the string rather than an int

        private void Start()
        {
            Init();
        }

        // will be run the first time this is run
        //abstract is going to force to create a state manager through a sub-class
        public abstract void Init();


        //control when the Tick is running - beacause this is a turn-based game
        public void Tick(SessionManager sm, Turn turn) // turn - so we can have access to player holder
        {
            delta = sm.delta;

            if (currState != null)
            {
               currState.Tick(this, sm, turn);
            }

            forceExit = false;
        }

        //moving from one state to another
        public void setState(string id)
        {
            State targetState = getState(id);
            if (targetState == null)
            {
                Debug.LogError("State with id : " + id + " cannot be found. Check your states and ids!");
            }
            currState = targetState;
        }

        public void SetStartingState()
        {
            currState = startingState;
        }

        State getState(string id)
        {
            State result = null;
            allStates.TryGetValue(id, out result); // get the state from the dictionary
            return result;
        }
    }
}