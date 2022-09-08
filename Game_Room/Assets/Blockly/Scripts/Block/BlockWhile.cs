using UnityEngine;

namespace UEBlockly
{ 
    public class BlockWhile : MonoBehaviour, IBlock
    {
        private float defaultSize, size;
        public bool isDragged { get; set; }
        private GameObject endWhileBlock;
        private DropPositionVR nextIfTrue, nextIfFalse, value;
        private bool sentIsFinish = false;

        private void Start()
        {
            isDragged = false;
            defaultSize = size = transform.Find("DropNextBlock").GetComponent<RectTransform>().sizeDelta.y;

            endWhileBlock = transform.Find("container").Find("endwhile").gameObject;

            nextIfTrue = transform.Find("DropNextBlock").GetComponent<DropPositionVR>();
            nextIfFalse = endWhileBlock.transform.Find("DropNextNotContainedBlock").GetComponent<DropPositionVR>();
            value = transform.Find("DropConditionBlock").GetComponent<DropPositionVR>();
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
            var MessageInfo = (gameObject, value.droppedGameObject.tag, value.droppedGameObject);
            SendMessageUpwards("CheckCondition", MessageInfo);

        }
        //recive the condition result from calculator
        public void GetConditionResult(string value)
        {
            var nextBlock = IBlock.IsTrue(value) ? nextIfTrue.droppedGameObject : nextIfFalse.droppedGameObject;
            if (nextBlock != null)
            {
                if (nextBlock.GetComponent<IBlock>() != null)
                    nextBlock.GetComponent<IBlock>().Execute();
                else
                    nextBlock.GetComponent<IEndStatement>().Execute();
            }
            else if (!sentIsFinish && nextBlock == null)
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
    }
}
