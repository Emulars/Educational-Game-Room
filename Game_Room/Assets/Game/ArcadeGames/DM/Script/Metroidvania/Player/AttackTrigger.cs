using Assets.DM.Script.Metroidvania.Enemy;
using UnityEngine;

namespace Assets.DM.Script.Metroidvania.Player
{
    public class AttackTrigger : MonoBehaviour
    {
        [SerializeField] public float damage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger != true && collision.CompareTag("Enemy"))
                //collision.SendMessageUpwards("onDamage", damage);   // SEND MESSAGE TO METHOD "onDamage"
                collision.transform.GetComponent<EnemyMovement>().OnDamage(damage);
        }
    }
}
