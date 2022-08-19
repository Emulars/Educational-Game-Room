using System.Collections;
using UnityEngine;

namespace Assets.DM.Script.Object
{
    public class ButtonDoorController : MonoBehaviour
    {
        [SerializeField] private Animator myDoor = null;

        private bool doorOpen = false;
    
        [SerializeField] private string openAnimationName = "DoorOpen";
        [SerializeField] private string closeAnimationName = "DoorClose";

        [SerializeField] private int waitTimer = 1;
        [SerializeField] private bool pauseInteraction = false;


        private IEnumerator PauseDoorInteraction()
        {
            pauseInteraction = true;
            yield return new WaitForSeconds(waitTimer);
            pauseInteraction = false;
        }

        public void PlayAnimation()
        {
            if(!doorOpen && !pauseInteraction)
            {
                myDoor.Play(openAnimationName, 0, 0.0f);
                doorOpen = true;
                StartCoroutine(PauseDoorInteraction());
            }
            else if (doorOpen && !pauseInteraction)
            {
                myDoor.Play(closeAnimationName, 0, 0.0f);
                doorOpen = false;
                StartCoroutine(PauseDoorInteraction());
            }
        }
 
    }
}
