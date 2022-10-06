using UnityEngine;

namespace Assets.DM.Script.Metroidvania.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 20f;
        [SerializeField] private GameObject fireBallPrefab;
        private Vector2 fireBallStartPos = new Vector2(0.659f, -0.08699989f);
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
            }

        }

        public void OnDamage(float damage)
        {
            currentHealth -= damage;
            if(damage >= 0f)
                animator.SetTrigger(EnemyAnimationParam.IsHit);
        }

        /* TO CHECK
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            animator.SetBool(EnemyAnimationParam.IsInrange, true);
            var fireBall = Instantiate(fireBallPrefab, fireBallStartPos, Quaternion.identity, transform);
            fireBall.GetComponent<Fireball>().Fire();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            animator.SetBool(EnemyAnimationParam.IsInrange, false);
        }*/
    }
}
