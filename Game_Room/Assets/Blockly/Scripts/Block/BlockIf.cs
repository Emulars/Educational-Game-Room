using UnityEngine;

namespace UEBlockly
{
    public class BlockIf : MonoBehaviour, IBlock
    {
        private float defaultSize, size;
        public bool isDragged { get; set; }
        private GameObject endIfBlock;
        private DropPositionVR nextIfTrue, nextIfFalse, value;

        private void Start()
        {
            isDragged = false;
            defaultSize = size = transform.Find("DropNextBlock").GetComponent<RectTransform>().sizeDelta.y;

            endIfBlock = transform.Find("container").Find("endif").gameObject;

            nextIfTrue = transform.Find("DropNextBlock").GetComponentInChildren<DropPositionVR>();
            nextIfFalse = endIfBlock.transform.Find("DropNextNotContainedBlock").GetComponentInChildren<DropPositionVR>();
            value = transform.Find("DropConditionBlock").GetComponentInChildren<DropPositionVR>();
        }

        private void Update()
        {
            var nextBlock = nextIfTrue.droppedGameObject;

            // Check if the block has a DropNextBlock
            if (nextBlock != null)
            {
                size = 2f * defaultSize + nextBlock.GetComponent<IBlock>().RecoursiveGetSize(gameObject, endIfBlock);
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
            else
            {
                SendMessageUpwards("IsFinish", gameObject.name);
            }
        }

        public void EndIf()
        {
            GetConditionResult("false");
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

            return sizeYcontainer + nextIfFalse.droppedGameObject.GetComponent<IBlock>()
                .RecoursiveGetSize(toResizeBlock, endStatement);
        }
    }
}