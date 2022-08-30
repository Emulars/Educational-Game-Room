using UnityEngine;

namespace Assets.DM.Script.Metroidvania.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float attackCooldown;
        private float attackTimer = 0f;
        private bool isAttacking = false;

        private Animator animator;
        private PlayerMovement playerMovement;
        [SerializeField] Collider2D attackTrigger;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
            attackTrigger.enabled = false;
        }

        private void Update()
        {
            //if(Input.GetKey(KeyCode.F) && !isAttacking && playerMovement.CanAttack())
            if (ButtonVR.button1 && !isAttacking && playerMovement.CanAttack())
            {
                isAttacking = true;
                attackTimer = attackCooldown;
                attackTrigger.enabled = true;
            }

            if(isAttacking)
            {
                if (attackTimer > 0)
                    attackTimer -= Time.deltaTime;
                else
                {
                    isAttacking = false;
                    attackTrigger.enabled = false;
                }
            }
            animator.SetBool(PlayerAnimationParam.IsAttacking, isAttacking);
        }
    }
}
