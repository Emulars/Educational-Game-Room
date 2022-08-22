using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class enemyBehaviour : MonoBehaviour
{
    public string name;
    public int damage;
    public int healing_power;
    public int maxHP;
    public int currentHP;

    [SerializeField] private TMP_Text menuName;
    [SerializeField] private TMP_Text menuHP;
    [SerializeField] private Slider HPBar;

    [SerializeField] private enemyBehaviour Boss;

    private Animator animator;

    void Start()
    {
        currentHP = maxHP;
        menuName.SetText(name);
        PrintCurrentHP();
        HPBar.maxValue = maxHP;
        HPBar.value = maxHP;

        animator = GetComponent<Animator>();
    }

    private void PrintCurrentHP()
    {
        menuHP.SetText(currentHP + " / " + maxHP);
    }

    public void TakeDamage(int takenDamage)
    {
        IEnumerator DamageAnimation()
        {
            animator.SetBool("getDamage", true);
            yield return new WaitForSeconds(0.9f);
            animator.SetBool("getDamage", false);
        }

        IEnumerator DeathAnimation()
        {
            animator.SetBool("isDead", true);
            yield return new WaitForSeconds(0.9f);
            Destroy(gameObject);
        }
        if((currentHP-takenDamage)>0)
        {
            currentHP -= takenDamage;
        }
        else
        {
            currentHP = 0;
        }
        PrintCurrentHP();
        HPBar.value = currentHP;
        StartCoroutine(currentHP <= 0 ? DeathAnimation() : DamageAnimation());
    }

    public void Act(GameObject hero)
    {
        IEnumerator HealAnimation()
        {
            animator.SetBool("healing", true);
            yield return new WaitForSeconds(0.9f);
            animator.SetBool("healing", false);
        }
      /*  if (Boss != null && Boss.currentHP < 50)
        {
            StartCoroutine(HealAnimation());
            Boss.Heal(healing_power);
        }
        else if (currentHP <= 10 && currentHP>0)
        {
            StartCoroutine(HealAnimation());
            Heal(healing_power);
        }
        else*/
            Attack(hero);
        
    }

    private void Attack(GameObject hero)
    {
        IEnumerator AttackAnimation()
        {
            animator.SetBool("jump", true);
            yield return new WaitForSeconds(0.9f);
            animator.SetBool("jump", false);
            hero.GetComponent<PlayerBehaviour>().TakeDamage(damage);
        }
        StartCoroutine(AttackAnimation());
        //hero.GetComponent<PlayerBehaviour>().TakeDamage(damage);

    }

    private void Heal(int healingPower)
    {
        currentHP += healingPower;
        PrintCurrentHP();
        HPBar.value = currentHP;
    }
}
