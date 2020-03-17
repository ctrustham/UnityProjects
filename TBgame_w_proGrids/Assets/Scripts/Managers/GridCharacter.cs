using UnityEngine;
using System.Collections.Generic;

// this was created as a "tag" to let us fill arrays with objects we designate from a level
namespace SA
{
    // a character on the grid
    public class GridCharacter : MonoBehaviour , ISelectable, IDeselect, IHighlight, IDeHighlight, IDetectable
    {
        public PlayerHolder owner;

        public GameObject highlighter; //used to highlight a character that is selected or hovered over
        public bool isSelected;

        [HideInInspector]
        public Node currentNode;
        [HideInInspector]
        public List<Node> currPath;

        public Animator animator;

        //references for grid
        #region Movement Vars
        private float walkSpeed = 1.5f; // character speed - walking, running, etc
        public float crouchSpeed = .75f;
        public float runSpeed = 3;
        public bool isCrouched;
        public bool isRunning;

        #endregion

        public void LoadPath(List<Node> path) // once a movement path has been selected/chosen
        {
            currPath = path;
        }

        public void OnInit()
        {
            owner.RegisterCharacter(this);
            highlighter.SetActive(false);
            animator = GetComponentInChildren<Animator>();
            animator.applyRootMotion = false;
        }

        #region Stance Handling
        public void setCrouch()
        {
            isRunning = false;
            isCrouched = true;

        }

        public void setRunning()
        {
            isRunning = true;
            isCrouched = false;
        }


        public void resetStance()
        {
            isRunning = false;
            isCrouched = false;
        }

        #endregion

        #region Animations
        public float GetSpeed()
        {
            float speed = walkSpeed;
            if (isCrouched )
            {
                speed = crouchSpeed;
            }
            else if (isRunning)
            {
                speed = runSpeed;
            }
            return speed;
        }

        public void PlayMovementAnimation()
        {
            if (isCrouched)
            {
                PlayAnimation("Crouch_walk");
            }
            else if (isRunning)
            {
                PlayAnimation("Run");
            }
            else
            {
                PlayAnimation("Walk");
            }
        }

        public void PlayIdleAnimation()
        {
            if (isCrouched)
            {
                PlayAnimation("Crouch_idle");
            }
            else
            {
                PlayAnimation("Idle");
            }
        }

        public void PlayAnimation(string targetAnim)
        {
            animator.CrossFade(targetAnim, 0.01f); // switch to the animation we want
        }
        #endregion

        #region Interfaces
        public void OnSelect(PlayerHolder player)
        {
            isSelected = true;
            highlighter.SetActive(true);
            player.stateManager.currChar = this; // will automatically deselect the previous character

        }
        // when the character is deselected
        public void OnDeselect(PlayerHolder player)
        {
            isSelected = false;
            highlighter.SetActive(false);
        }

        public void OnHighlight(PlayerHolder player)
        {
            highlighter.SetActive(true);
        }

        public void OnDeHighlight(PlayerHolder player)
        {
            if (!isSelected)
            {
                highlighter.SetActive(false);
            }
        }

        public Node OnDetect()
        {
            return currentNode;
        }
        #endregion
    }
}