using Assets.Blockly.Scripts.Block.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Blockly.Scripts.DragAndDrop
{
    public class DropInTrashCan : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.GetComponent<IBlock>() == null) return; //cannot drop not IBlock obj
            Destroy(eventData.pointerDrag);
        }
    }
}