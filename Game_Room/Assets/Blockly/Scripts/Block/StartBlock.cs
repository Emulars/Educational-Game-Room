using UnityEngine;

public class StartBlock : MonoBehaviour
{
    private void Start()
    {
        GetComponentInChildren<DropPosition>().SetActive();
    }
    public void Execute()
    {
        var next = GetComponentInChildren<DropPosition>().droppedGameObject;
        if (next != null)
        {
            next.GetComponent<IBlock>().Execute();
        }
    }
}
