using Assets.Blockly.Scripts.Block.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Blockly.Scripts.DragAndDrop
{
    public class DropPosition : MonoBehaviour , IDropHandler
    {
        public GameObject droppedGameObject;
        private bool isAttached = false;

        void Update()
        {
            if (!isAttached) return;
        
            if (droppedGameObject.GetComponent<IBlock>().IsDragged)
            {
                isAttached = false;
                droppedGameObject = null;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if(eventData.pointerDrag.GetComponent<IBlock>() == null)return; //cannot drop not IBlock obj
            //auto positioning when the component is dropped
            eventData.pointerDrag.GetComponent<RectTransform>().position =
                GetComponentInParent<RectTransform>().position;
        
            droppedGameObject = eventData.pointerDrag;
            isAttached = true;

            eventData.pointerDrag.transform.SetParent(gameObject.transform.parent);
        }
    }
}
