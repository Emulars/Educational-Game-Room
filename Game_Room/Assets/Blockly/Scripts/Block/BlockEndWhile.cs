using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BlockEndWhile : MonoBehaviour, IBlock , IEndStatement
{
    public bool isDragged { get; set; }
    public bool isInMain { get; set; }
    private BlockWhile restartBlock;
    public bool TimeToRepeat = false;

    private void Start()
    {
        restartBlock = transform.parent.parent.GetComponent<BlockWhile>();
    }

    private void Update()
    {
        if(!TimeToRepeat)return;
        Restart();
    }

    public void Execute()
    {
        Debug.Log("while cycle ended");
        TimeToRepeat = true;
    }
    public void Restart()
    {
        transform.parent.parent.GetComponent<BlockWhile>().Execute();
    }
    public void ErrorMessage(ErrorCode errorCode)
    {
        throw new System.NotImplementedException();
    }

    public float RecoursiveGetSize(GameObject toResizeBlock, GameObject endStatement)
    {
        throw new System.NotImplementedException();
    }
    
    public float RecoursiveGetSize(GameObject toResizeBlock)
    {
        throw new System.NotImplementedException();
    }

}