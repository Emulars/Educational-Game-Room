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
        
        [Header("Attack")]
        [SerializeField] private GameObject fireBallPrefab;
        [SerializeField] private Collider2D attackArea;
        [SerializeField] private float attackCooldown;
        private Vector2 fireBallStartPos = new Vector2(0.368f, -0.08699989f);
        private Vector2 enemyPos;
        private float attackTimer = 0f;
        private int fireBallCounter = 0;
        
        void Awake()
        {
            animator = GetComponent<Animator>();
            collider = GetComponent<Collider2D>();
        }
        // Start is called before the first frame update
        private void Start()
        {
            enemyPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            currentHealth = maxHealth;
            attackTimer = attackCooldown;
        }

        // Update is called once per frame
        void Update()
        {
            if(currentHealth <= 0f && !isDead)
            {
                animator.SetTrigger(EnemyAnimationParam.IsDead);
                collider.isTrigger = true;
                isDead = true;
            }
        }

        public void OnDamage(float damage)
        {
            currentHealth -= damage;
            if(damage >= 0f)
                animator.SetTrigger(EnemyAnimationParam.IsHit);
        }

        private bool canAttack()
        {
            if (fireBallCounter >= 5)
            {
                attackArea.enabled = false;
                return false;
            }
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
                return false;
            }
            else
            {
                attackTimer = attackCooldown;
                return true;
            }
        }
            
        
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (isDead) return;
            if (!collision.CompareTag("Player")) return;
            if (!canAttack()) return;
            var fireBall = Instantiate(fireBallPrefab, enemyPos-fireBallStartPos, Quaternion.identity, transform);
            fireBall.transform.localScale = new Vector3(1,1,1);
            fireBall.GetComponent<Fireball>().Fire(fireBall.GetComponent<Rigidbody2D>());
            animator.SetTrigger(EnemyAnimationParam.IsInrange);
            fireBallCounter++;
        }
    }
}
