using Assets.Blockly.Scripts.Block.Interface;
using Assets.Blockly.Scripts.DragAndDrop;
using UnityEngine;

namespace Assets.Blockly.Scripts.Block
{
    public class BlockWhile : MonoBehaviour, IBlock
    {
        private float defaultSize, size;
        public bool IsDragged { get; set; }
        public bool IsInMain { get; set; }
        private GameObject endWhileBlock;
        private DropPosition nextIfTrue, nextIfFalse, value;
        private bool sentIsFinish = false;

        private void Start()
        {
            IsDragged = false;
            defaultSize = size = transform.Find("DropNextBlock").GetComponent<RectTransform>().sizeDelta.y;

            endWhileBlock = transform.Find("container").Find("endwhile").gameObject;

            nextIfTrue = transform.Find("DropNextBlock").GetComponentInChildren<DropPosition>();
            nextIfFalse = endWhileBlock.transform.Find("DropNextNotContainedBlock").GetComponentInChildren<DropPosition>();
            value = transform.Find("DropConditionBlock").GetComponentInChildren<DropPosition>();
        }

        private void Update()
        {
            var nextBlock = nextIfTrue.droppedGameObject;

            // Check if the block has a DropNextBlock
            if (nextBlock != null)
            {
                size = 2f * defaultSize + nextBlock.GetComponent<IBlock>().RecoursiveGetSize(gameObject, endWhileBlock);
            }
            else
            {
                size = defaultSize;
            }
            //UpdateSize
            var containerRectTransform = transform.Find("container").GetComponent<RectTransform>();
            containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -size);
        }

        public void Execute()
        {

            if (value.droppedGameObject == null)
            {
                ErrorMessage(ErrorCode.NotDroppedValue);
                return;
            }
            var messageInfo = (gameObject, value.droppedGameObject.tag, value.droppedGameObject);
            SendMessageUpwards("CheckCondition", messageInfo);

        }

        public void GetConditionResult(string value)
        {
            var nextBlock = IBlock.IsTrue(value) ? nextIfTrue.droppedGameObject : nextIfFalse.droppedGameObject;
            if (nextBlock != null)
            {
                nextBlock.GetComponent<IBlock>().Execute();
            }
            else if(!sentIsFinish && nextBlock == null)
            {
                SendMessageUpwards("IsFinish", gameObject.name);
                sentIsFinish = true;
            }
        }

        public void ErrorMessage(ErrorCode errorCode)
        {
            SendMessageUpwards("CatchError", errorCode);
        }

        public float RecoursiveGetSize(GameObject toResizeBlock, GameObject endStatement)
        {
            var sizeYcontainer = transform.Find("container").GetComponent<RectTransform>().sizeDelta.y;

            // If there are not other objects after this one
            if (nextIfFalse.droppedGameObject == null || nextIfFalse.droppedGameObject.GetComponent<IEndStatement>() != null)
            {
                nextIfFalse.droppedGameObject = endStatement;
                return sizeYcontainer;
            }

            return sizeYcontainer + nextIfFalse.droppedGameObject.GetComponent<IBlock>().RecoursiveGetSize(toResizeBlock, endStatement);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger)
                IsInMain = collision.CompareTag("Main");
        }

    }
}
