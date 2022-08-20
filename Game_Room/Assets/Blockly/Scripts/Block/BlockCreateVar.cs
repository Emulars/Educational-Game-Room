using UnityEngine;
using UnityEngine.UI;

public class BlockCreateVar : MonoBehaviour, IBlock
{
    public bool isDragged { get; set; }
    public bool isInMain { get; set; }

    private void Start()
    {
        isDragged = false;
    }

    public void Execute()
    {
        string variableName = transform.Find("InputField").Find("Text").GetComponent<Text>().text;

        ErrorCode err = IBlock.IsValidName(variableName);
        if (err != ErrorCode.NoError)
        {
            ErrorMessage(err);
            return;
        }

        SendMessageUpwards("CreateVariable", variableName);

        var next = GetComponentInChildren<DropPosition>().droppedGameObject;
        if(next != null)
        {
            next.GetComponent<IBlock>().Execute();
        }
        else
        {
            SendMessageUpwards("IsFinish", gameObject.name);
        }
    }

    public void ErrorMessage(ErrorCode errorCode)
    {
        SendMessageUpwards("CatchError", errorCode);
    }

    public float RecoursiveGetSize(GameObject toResizeBlock, GameObject endStatement)
    {
        var next = GetComponentInChildren<DropPosition>();

        var sizeY = gameObject.GetComponent<RectTransform>().sizeDelta.y;

        // If there are not other objects after this one
        if (next.droppedGameObject == null || next.droppedGameObject.GetComponent<IEndStatement>() != null)
        {
            next.droppedGameObject = endStatement;
            return sizeY;
        }

        return sizeY + next.droppedGameObject.GetComponent<IBlock>().RecoursiveGetSize(toResizeBlock, endStatement);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            isInMain = collision.CompareTag("Main");
    }

}
