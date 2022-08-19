using Assets.Blockly.Scripts.Block.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Blockly.Scripts.DragAndDrop
{
    public class DragDrop : MonoBehaviour  ,  IBeginDragHandler, IEndDragHandler , IDragHandler
    {
        private RectTransform rectTransform;
        public CanvasGroup canvasGroup;
        // Duplication
        [SerializeField] private Canvas canvas;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var itemBeingDragged = eventData.pointerDrag;
            itemBeingDragged.GetComponent<IBlock>().IsDragged = true;
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;


            if (transform.parent == canvas.transform)
                return;

            var draggedTransform = itemBeingDragged.GetComponent<RectTransform>();

            // Duplicate block if in the SideBar
            if (!itemBeingDragged.GetComponent<IBlock>().IsInMain)
            {
                GameObject duplicate = Instantiate(itemBeingDragged, itemBeingDragged.transform.parent, false);
                duplicate.name = itemBeingDragged.name;
                OnEndDrag(duplicate);
            }
            itemBeingDragged.transform.SetParent(canvas.transform);

        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta /canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            eventData.pointerDrag.GetComponent<IBlock>().IsDragged = false;
        }

        public void OnEndDrag(GameObject gameObject)
        {
            gameObject.GetComponent<DragDrop>().canvasGroup.alpha = 1f;
            gameObject.GetComponent<DragDrop>().canvasGroup.blocksRaycasts = true;
            gameObject.GetComponent<IBlock>().IsDragged = false;
        }

    }
}
