using UnityEngine;
using UnityEngine.UI;

public class BlockUpdateByBlock : MonoBehaviour , IBlock
{

    private string variableName;
    private DropPosition next, value;
    public bool isDragged { get; set; }
    public bool isInMain { get; set; }

    private void Start()
    {
        isDragged = false;
        next = transform.Find("DropNextBlock").GetComponent<DropPosition>();
        value = transform.Find("DropValueBlock").GetComponent<DropPosition>();
    }

    public void Execute()
    {
        variableName = transform.Find("InputField").Find("Text").GetComponent<Text>().text;

        ErrorCode err = IBlock.IsValidName(variableName);
        if (err != ErrorCode.NoError)
        {
            ErrorMessage(err);
            return;
        }

        if (value.droppedGameObject == null)
        {
            ErrorMessage(ErrorCode.NotDroppedValue);
            return;
        }
        var MessageInfo = (variableName, value.droppedGameObject.tag, value.droppedGameObject);
        SendMessageUpwards("UpdateVariableByBlock",MessageInfo);
        
        var nextBlock = next.droppedGameObject;
        if (nextBlock != null)
        {
             if (nextBlock.GetComponent<IBlock>() != null)
                nextBlock.GetComponent<IBlock>().Execute();
            else
                nextBlock.GetComponent<IEndStatement>().Execute();
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
