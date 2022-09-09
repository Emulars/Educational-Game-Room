using UnityEngine;
using Valve.VR.InteractionSystem;

namespace UEBlockly
{
    [RequireComponent(typeof(Throwable))]
    [RequireComponent(typeof(Collider))]
    public class DragVR : MonoBehaviour
    {
        private Throwable throwable;
        private bool isDragging = false;
        [SerializeField]private bool isFirstGrab = true;
        private DropPositionVR[] dropPositions;
        private Transform originalParent;
        [SerializeField] private Transform canvas;

        private void Awake()
        {
            originalParent = transform.parent;
            throwable = GetComponent<Throwable>();
            dropPositions = GetComponentsInChildren<DropPositionVR>();
        }
        private void Update()
        {
            if (throwable.attached)
            {
                //duplicate blocks
                if(isFirstGrab)
                {
                    GameObject duplicate = Instantiate(gameObject, originalParent, true);
                    duplicate.name = name;
                    duplicate.GetComponent<DragVR>().isDragging = false;
                    duplicate.GetComponent<DragVR>().SetTriggers(false);
                    duplicate.GetComponent<Throwable>().attached = false;
                    isFirstGrab = false;
                }    
                isDragging = true;
                SetTriggers(false);
            }
            if (!throwable.attached && isDragging)
            {
                isDragging = false;
                SetTriggers(true);
            }

        }
        private void SetTriggers(bool isActive)
        {
            foreach (var drop in dropPositions)
            {
                drop.SetActive(isActive);
            }
        }
    }
}