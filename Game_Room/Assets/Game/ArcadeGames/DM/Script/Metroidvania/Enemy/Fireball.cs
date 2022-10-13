using Assets.DM.Script.Metroidvania.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    
    [SerializeField] private float damage = 100f;
    [SerializeField] private float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Called by EnemyMovement OnTriggerStay()
    public void Fire(Rigidbody2D body)
    {
        body.velocity = new Vector2(-speed, body.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("Hit");
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerMovement>().OnDamage(damage); // Only if the player is hit, calucalte the damage
        Destroy(gameObject);
    }
}
