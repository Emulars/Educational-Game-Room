using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    
    [SerializeField] private float damage = 20f;
    [SerializeField] private float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Called by EnemyMovement OnTriggerStay()
    public void Fire()
    {
        //transform.Translate(Vector2.left * speed * Time.deltaTime);
        rb.velocity = new Vector2(speed * Time.deltaTime, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("Hit");
        if (!collision.gameObject.CompareTag("Player")) return;
        //collision.gameObject.SendMessage("ApplyDamage", 10);
        Destroy(gameObject);
    }
}
