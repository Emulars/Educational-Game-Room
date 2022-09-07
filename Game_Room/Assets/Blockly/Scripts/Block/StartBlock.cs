using UnityEngine;

public class StartBlock : MonoBehaviour
{
    private void Start()
    {
        //GetComponentInChildren<DropPosition>().SetActive();
    }
    public void Execute()
    {
        print("start");
        // var next = GetComponentInChildren<DropPosition>().droppedGameObject;
        var next = GetComponentInChildren<DropPositionVR>().droppedGameObject;
        if (next != null)
        {
            next.GetComponent<IBlock>().Execute();
            
        }
    }
}
