using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public enum AttackDirection
    {
        left,right
    }

    public AttackDirection attackDirection;
    private Vector2 rightAttackOffset;

    private Collider2D swordCollider;


    [SerializeField] private int damage = 20;

    private void Start()
    {
        swordCollider = GetComponent<Collider2D>();
    }

    public void Attack()
    {
        Debug.Log("attacco");
        switch (attackDirection)
        {
            case AttackDirection.left:
                AttackLeft();
                break;
            case AttackDirection.right:
                AttackRight();
                break;
            //altre direzioni
        }
        
    }

    public void AttackRight()
    {
        swordCollider.enabled = true;
        transform.localPosition = Vector3.zero;
    }

    public void AttackLeft()
    {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(-0.25f, 0, 0);
    }

    public void AttackStop()
    {
        swordCollider.enabled = false;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag!="enemy")return;
        Debug.Log("nemico colpito");
        var enemy = other.GetComponent<OpenWorldEnemyBehaviourScript>();
        enemy.TakeDamage(damage);
    }
}
