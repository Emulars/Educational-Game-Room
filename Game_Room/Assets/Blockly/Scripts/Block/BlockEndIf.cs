using UnityEngine;

namespace UEBlockly
{
    public class BlockEndIf : MonoBehaviour, IEndStatement
    {
        public void Execute()
        {
            Debug.Log("Block " + this.name + "if ended");
            transform.parent.parent.GetComponent<BlockIf>().EndIf();
        }

    }
}


