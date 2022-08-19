using Assets.Blockly.Scripts.Block.Interface;
using Assets.Blockly.Scripts.DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Blockly.Scripts.Block
{
    public class BlockUpdateByBlock : MonoBehaviour , IBlock
    {

        private string variableName;
        private DropPosition next, value;
        public bool IsDragged { get; set; }
        public bool IsInMain { get; set; }

        private void Start()
        {
            IsDragged = false;
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
            var messageInfo = (variableName, value.droppedGameObject.tag, value.droppedGameObject);
            SendMessageUpwards("UpdateVariableByBlock",messageInfo);
        
            var nextBlock = next.droppedGameObject;
            if (nextBlock != null)
            {
                nextBlock.GetComponent<IBlock>().Execute();
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
                IsInMain = collision.CompareTag("Main");
        }

    }
}
