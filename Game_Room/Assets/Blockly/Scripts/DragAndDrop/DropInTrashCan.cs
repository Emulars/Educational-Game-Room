using UnityEngine;
using Valve.VR.InteractionSystem;

namespace UEBlockly
{
    public class DropInTrashCan : MonoBehaviour
    {
        public void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<IBlock>() == null || other.GetComponent<Throwable>().attached) return;
            Destroy(other.gameObject);
        }
    }
}