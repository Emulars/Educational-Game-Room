using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEndIf : MonoBehaviour, IBlock , IEndStatement
{
    public bool isDragged { get; set; }
    public bool isInMain { get; set; }
    public void Execute()
    {
        Debug.Log("if ended");
        transform.parent.parent.GetComponent<BlockIf>().EndIf();
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
