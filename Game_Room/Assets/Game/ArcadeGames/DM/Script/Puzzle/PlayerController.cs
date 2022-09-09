using System.Collections.Generic;
using Assets.DM.Script.Metroidvania.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.DM.Script.Puzzle
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 1f;
        public float collisionOffset = 0.05f;
        public ContactFilter2D movementFilter;

        List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
        Vector2 movementInput;
        Rigidbody2D rb;
        Animator animator;
        SpriteRenderer spriteRenderer;

        // Start is called before the first update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate() {

            movementInput = JoystickController.leverVector;

            //if movement input is not 0, try to move
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                // If not successful try move only on x axe
                if(!success)
                    success = TryMove(new Vector2(movementInput.x, 0));
        
                // If not successful try move only on y axe
                if(!success)
                    success = TryMove(new Vector2(0, movementInput.y));

                animator.SetBool(PlayerAnimationParam.IsMoving, success);
            }
            else
            {
                animator.SetBool(PlayerAnimationParam.IsMoving, false);
            }


            // Set direction of sprite to movement direction
            if(movementInput.x < 0)
                spriteRenderer.flipX = true;
            else if(movementInput.x > 0)       
                spriteRenderer.flipX = false;

        }

        private bool TryMove(Vector2 direction)
        {
            if(direction != Vector2.zero)
            {
                //Check for potential collision
                int count = rb.Cast(
                    direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collision
                    movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                    castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                    moveSpeed * Time.fixedDeltaTime + collisionOffset // The amount to cast equal to the movement plus an offset
                );

                if(count == 0)
                {
                    rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // Cannot move if there's no direction to move in
                return false;
            }        
        }
    }
}

