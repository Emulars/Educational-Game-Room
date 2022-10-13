using UnityEngine;

namespace Assets.DM.Script.Metroidvania.Player
{
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField] public float speed;
        [SerializeField] public bool enableJump = false;
        [SerializeField] private LayerMask groundLayer;
        private Rigidbody2D body;
        private Animator animator;
        private CapsuleCollider2D capsuleCollider;
        private float horizontalInput;

        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private GameObject healthBarObj;
        private float currentHealth;

        private void Awake()
        {
            // Grab reference for object's components
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider2D>();
            currentHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            // Equals to 0 if no X direction
            // Equals to 1 if direction right
            // Quals to -1 if direction left
            //horizontalInput = Input.GetAxis(PlayerAnimationParam.AxisXinput);
            horizontalInput = JoystickController.leverVector.x;

            // Walk 
            if(speed >= 1f && speed <= 10f)
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            // Flip player when moving left/right
            if (horizontalInput > 0.01f)
                transform.localScale = Vector3.one;
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector3(-1, 1, 1);

            // Jump
            //if (Input.GetKey(KeyCode.Space) && isOnGround() && (enableJump && speed==1f))
            if (ButtonVR.button2 && isOnGround() && (enableJump && speed == 1f))
                Jump();

            // Set animation
            animator.SetBool(PlayerAnimationParam.IsMoving, horizontalInput != 0);
            animator.SetBool(PlayerAnimationParam.IsOnGround, isOnGround());
        }

        private void Jump()
        {
            body.velocity = new Vector2(body.velocity.x, speed);
            animator.SetTrigger(PlayerAnimationParam.JumpTriggerName);
        }

        private bool isOnGround()
        {
            RaycastHit2D raycastHit = Physics2D.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, capsuleCollider.direction, 0, Vector2.down, 0.1f, groundLayer);
            return raycastHit.collider != null;
        }

        public bool CanAttack()
        {
            return horizontalInput == 0 && isOnGround();
        }

        public void OnDamage(float damage)
        {
            currentHealth -= damage;
            print("Player Health: " + currentHealth);
            HealthBar();
            if(currentHealth <= 0)
            {
                // Activate death zone
                healthBarObj.transform.GetComponentInChildren<BoxCollider2D>().size = new Vector2(1f, 0.5f);
            }
        }

        private void HealthBar()
        {
            healthBarObj.transform.localScale =
                new Vector3(Mathf.InverseLerp(0f, maxHealth, currentHealth), 1f);
        }
    }
}
