using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenWorldPlayerController : MonoBehaviour
{
    [Header("player stats")]
    [SerializeField] private int magicDamage;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float collisionOffset = 0.05f;
    //for movement
    private bool canMove = true;
    private Vector2 movementInput;
    private SpriteRenderer spriteRenderer;
    //for collisions
    private Rigidbody2D rigidbody2D;
    [SerializeField] private ContactFilter2D movementFilter;
    private readonly List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private BoxCollider2D collider;
    //for attacks
    [SerializeField]private SwordAttack swordAttack;
    //for animations
    private Animator animator;
    
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        void Move()
        {
            if (!canMove) return;
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);
                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                    if (!success)
                        success = TryMove(new Vector2(0, movementInput.y));
                }

                animator.SetBool("isWalking", success);
            }
            else
                animator.SetBool("isWalking", false);

            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
                swordAttack.attackDirection = SwordAttack.AttackDirection.left;
            }

            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
                swordAttack.attackDirection = SwordAttack.AttackDirection.right;
            }
        }

        collider.enabled = bool.Parse(Executor.variabili["collisioni"]);
        Move();
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction == Vector2.zero) return false;

        int count = rigidbody2D.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            rigidbody2D.MovePosition(rigidbody2D.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    private void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    private void OnFire()
    {
        animator.SetTrigger("isAttackingTrigger");
        swordAttack.Attack();
    }

    public void StopAttack()
    {
        swordAttack.AttackStop();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void TakeDamage(int damage)
    {
        animator.SetBool("TakeDamage",true);
        if ((currentHP - damage) <= 0) 
            currentHP = 0;
        else
            currentHP -= damage;
        animator.SetBool("TakeDamage", false);
    }
}
