using UnityEngine;

namespace Assets.DM.Script.Metroidvania.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 20f;
        private float currentHealth;
        private Animator animator;
        private new Collider2D collider;
        private bool isDead = false;

        void Awake()
        {
            animator = GetComponent<Animator>();
            collider = GetComponent<Collider2D>();
        }
        // Start is called before the first frame update
        private void Start()
        {
            currentHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            if(currentHealth <= 0f && !isDead)
            {
                animator.SetTrigger(EnemyAnimationParam.IsDead);
                collider.isTrigger = true;
                isDead = true;
                //Destroy(gameObject);
            }

        }

        public void OnDamage(float damage)
        {
            currentHealth -= damage;
            if(damage >= 1f)
                animator.SetTrigger(EnemyAnimationParam.IsHit);
            //animator.SetBool(EnemyAnimationParam.isHit, false);
        }
    }
}
