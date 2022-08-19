using Assets.Blockly.Scripts.Block.Interface;
using UnityEngine;

namespace Assets.Blockly.Scripts.Block
{
    public class GenericBlock : MonoBehaviour , IBlock
    {
        public bool IsDragged { get; set; }
        public bool IsInMain { get; set; }
        // Start is called before the first frame update
        private void Start()
        {
            IsDragged = false;
            IsInMain = false;
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        public void ErrorMessage(ErrorCode errorCode)
        {
            throw new System.NotImplementedException();
        }

        public float RecoursiveGetSize(GameObject toResizeBlock, GameObject endStatement)
        {
            throw new System.NotImplementedException();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger)
                IsInMain = collision.CompareTag("Main");
        }

    }
}
