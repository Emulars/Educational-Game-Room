using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour  ,  IBeginDragHandler, IEndDragHandler , IDragHandler
{
    private RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    // Duplication
    [Header("Canvas")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private float scaleMultiplayer = 1f;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var itemBeingDragged = eventData.pointerDrag;
        itemBeingDragged.GetComponent<IBlock>().isDragged = true;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;


        if (transform.parent == canvas.transform)
            return;

        var draggedTransform = itemBeingDragged.GetComponent<RectTransform>();

        // Duplicate block if in the SideBar
        if (!itemBeingDragged.GetComponent<IBlock>().isInMain)
        {
            GameObject duplicate = Instantiate(itemBeingDragged, itemBeingDragged.transform.parent, false);
            duplicate.name = itemBeingDragged.name;
            OnEndDrag(duplicate);
        }
        itemBeingDragged.transform.SetParent(canvas.transform);
        var dropPositions = itemBeingDragged.GetComponentsInChildren<DropPosition>();
        foreach (var drop in dropPositions)
        {
            drop.SetActive();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / (canvas.scaleFactor * scaleMultiplayer);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        eventData.pointerDrag.GetComponent<IBlock>().isDragged = false;
    }

    public void OnEndDrag(GameObject gameObject)
    {
        gameObject.GetComponent<DragDrop>().canvasGroup.alpha = 1f;
        gameObject.GetComponent<DragDrop>().canvasGroup.blocksRaycasts = true;
        gameObject.GetComponent<IBlock>().isDragged = false;
    }

}
