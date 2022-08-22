using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Hero Stats")]
    private Vector3 playerInitialPosition;
    private Animator playerAnimator;

    public int damage;
    public int magicDamage;
    public int maxHP;
    public int currentHP;
    
    [Header("Hero HUD")]
    [SerializeField] private TMP_Text menuHP;
    [SerializeField] private Slider HPBar;

    [Header("Magic spells")] 
    [SerializeField] private GameObject fireBall;

    private Vector3 fireBallPosition;

    void Start()
    {
        currentHP = maxHP;
        PrintCurrentHP();
        HPBar.maxValue = maxHP;
        HPBar.value = maxHP;

        playerAnimator = GetComponent<Animator>();
        playerInitialPosition = transform.localPosition;
        fireBallPosition = fireBall.transform.position;
    }

    private void PrintCurrentHP()
    {
        menuHP.SetText(currentHP + " / " + maxHP);
    }

    public void TakeDamage(int damage)
    {
        IEnumerator DamageAnimation()
        {
            yield return new WaitForSeconds(0.4f);
            playerAnimator.SetBool("TakeDamage", true);
            yield return new WaitForSeconds(1f);
            playerAnimator.SetBool("TakeDamage", false);
            HPBar.value = currentHP;
            PrintCurrentHP();
        }

        IEnumerator DeathAnimation()
        {
            playerAnimator.SetBool("isDied", true);
            yield return new WaitForSeconds(0.9f);
            HPBar.value = currentHP;
            PrintCurrentHP();
        }

        if ((currentHP - damage) <= 0)
            currentHP = 0;
        else
            currentHP -= damage;
        StartCoroutine(currentHP == 0 ? DeathAnimation() : DamageAnimation());
        
    }

    public void Attack(Transform enemy)
    {
        IEnumerator AttackAnimation()
        {
            Vector3 attackPosition = new Vector3(enemy.transform.position.x-2f , enemy.transform.position.y+0.5f, playerInitialPosition.z);
            transform.position = attackPosition;
            playerAnimator.SetBool("isAttacking",true);
            yield return new WaitForSeconds(0.5f);
            playerAnimator.SetBool("isAttacking", false);
            transform.position = playerInitialPosition;
        }
        StartCoroutine(AttackAnimation());
        
    }

    public void LaunchFireBall()
    {
        IEnumerator AttackAnimation()
        {
            var FBAnimator = fireBall.GetComponent<Animator>();
            fireBall.SetActive(true);
            FBAnimator.SetBool("isAttacking", true);
            yield return new WaitForSeconds(0.7f);
            FBAnimator.SetBool("isAttacking", false);
            fireBall.transform.position = fireBallPosition;
            fireBall.SetActive(false);
        }
        StartCoroutine(AttackAnimation());

    }

    private void OnMove(InputValue movementValue)
    {
    }

    private void OnFire()
    {
    }

    public void StopAttack()
    {
    }

    public void LockMovement()
    {
    }

    public void UnlockMovement()
    {
    }
    
}
