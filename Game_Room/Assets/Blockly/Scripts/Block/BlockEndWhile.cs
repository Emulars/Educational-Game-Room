using Assets.Blockly.Scripts.Block.Interface;
using UnityEngine;

namespace Assets.Blockly.Scripts.Block
{
    public class BlockEndWhile : MonoBehaviour, IBlock , IEndStatement
    {
        public bool IsDragged { get; set; }
        public bool IsInMain { get; set; }
        private BlockWhile restartBlock;
        public bool timeToRepeat = false;

        private void Start()
        {
            restartBlock = transform.parent.parent.GetComponent<BlockWhile>();
        }

        private void Update()
        {
            if(!timeToRepeat)return;
            Restart();
        }

        public void Execute()
        {
            Debug.Log("while cycle ended");
            timeToRepeat = true;
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
}