using UnityEngine;
using System.Collections;

namespace SA
{
    public class PlayerStateManager : StateManager
    {
        public override void Init()
        {
            VariablesHolder gameVars = Resources.Load("GameVariables") as VariablesHolder;

            State interactions = new State(); // player interactions
            interactions.actions.Add(new InputManager(gameVars));
            interactions.actions.Add(new HandleMouseInteractions());
            interactions.actions.Add(new MoveCameraTransform(gameVars));

            State wait = new State(); // does nothing atm
            State moveOnPath = new State();
            moveOnPath.actions.Add(new MoveCharacterOnPath());

            currState = interactions;
            startingState = interactions;

            allStates.Add("moveOnPath", moveOnPath);
            allStates.Add("interactions", interactions);
            allStates.Add("wait", wait);
        }
    }
}