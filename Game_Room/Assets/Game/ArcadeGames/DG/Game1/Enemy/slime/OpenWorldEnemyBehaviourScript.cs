using UnityEngine;

public class OpenWorldEnemyBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Header("enemy stats")]
    [SerializeField] private int hp = 1;
    [SerializeField] private float movementSpeed = 2;
    [SerializeField] private int damage = 2;
    [SerializeField] public float triggerDistance = 15;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Move", 2.3f);
    }

    private void Update()
    {
        Move();
    }

    public void TakeDamage(int damage)
    {
        hp = (~(hp - damage) >> 31) & (hp - damage);
        if (hp == 0)
            Defeated();
    }

    private void Defeated()
    {
        animator.SetTrigger("isDefeated");
    }

    //chiamato nell'animazione
    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Move()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < triggerDistance)
        {
            animator.SetBool("walk", true);
            spriteRenderer.flipX = player.transform.position.x > transform.position.x;
            transform.position += (player.transform.position - transform.position).normalized * movementSpeed *
                                  Time.deltaTime;
        }
        else
        {
            animator.SetBool("walk", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger damage");
        if (other.tag != "Player") return;
        player.GetComponent<OpenWorldPlayerController>().TakeDamage(damage);
    }
}
