using UnityEngine;
using Valve.VR.InteractionSystem;

namespace UEBlockly
{
    public class DropPositionVR : MonoBehaviour
    {
        private bool isAttached = false, isActive = false;
        [SerializeField] private bool onlyValueBlock = false;
        public GameObject droppedGameObject;

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }

        public void OnTriggerStay(Collider other)
        {
            if (!isActive || isAttached) return;
            if (other.GetComponent<IBlock>() == null || other.GetComponent<Throwable>().attached) return;
            if (onlyValueBlock && other.GetComponent<ValueBlock>() == null) return;
            if (!onlyValueBlock && other.GetComponent<ValueBlock>() != null) return;
            other.transform.SetParent(transform);
            droppedGameObject = other.gameObject;
            droppedGameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            droppedGameObject.transform.localPosition = Vector3.zero;
            isAttached = true;
        }

        public void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.Equals(droppedGameObject)) return;
            droppedGameObject = null;
            isAttached = false;
        }
    }
}