using Assets.Blockly.Scripts.Block.Interface;
using UnityEngine;

namespace Assets.Blockly.Scripts.Block
{
    public class BlockEndIf : MonoBehaviour, IBlock , IEndStatement
    {
        public bool IsDragged { get; set; }
        public bool IsInMain { get; set; }
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
}
