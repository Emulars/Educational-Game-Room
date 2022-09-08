using UnityEngine;

namespace UEBlockly
{
    public class ValueBlock : MonoBehaviour, IBlock
    {
        public bool isDragged { get; set; }
        // Start is called before the first frame update
        private void Start()
        {
            isDragged = false;
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

    }
}