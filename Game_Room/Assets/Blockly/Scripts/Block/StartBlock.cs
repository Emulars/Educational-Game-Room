using UnityEngine;

namespace UEBlockly
{
    public class StartBlock : MonoBehaviour
    {
        private void Start()
        {
            GetComponentInChildren<DropPositionVR>().SetActive(true);
            LaunchBlocks.firstLaunch = false;
        }
        public void Execute()
        {
            print("start");
            LaunchBlocks.firstLaunch = true;
            var next = GetComponentInChildren<DropPositionVR>().droppedGameObject;
            if (next != null)
            {
                next.GetComponent<IBlock>().Execute();

            }
        }
    }
}
