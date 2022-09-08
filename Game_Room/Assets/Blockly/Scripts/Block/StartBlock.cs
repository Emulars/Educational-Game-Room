using UnityEngine;

namespace UEBlockly
{
    public class StartBlock : MonoBehaviour
    {
        private void Start()
        {
            GetComponentInChildren<DropPositionVR>().SetActive(true);
        }
        public void Execute()
        {
            print("start");
            var next = GetComponentInChildren<DropPositionVR>().droppedGameObject;
            if (next != null)
            {
                next.GetComponent<IBlock>().Execute();

            }
        }
    }
}
