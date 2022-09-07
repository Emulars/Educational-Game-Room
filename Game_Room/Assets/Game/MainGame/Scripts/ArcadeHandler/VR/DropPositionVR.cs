using UnityEngine;
using Valve.VR.InteractionSystem;

public class DropPositionVR : MonoBehaviour
{
    private bool isAtacched = false, isActive = true;
    private GameObject block;
    public GameObject droppedGameObject;
    public void OnTriggerEnter(Collider other)
    {
        print("trigger enter");
        if (other.GetComponent<IBlock>() == null || !isActive) return;
        block = other.gameObject;
    }
    public void OnTriggerStay(Collider other)
    {
        print("trigger stay");
        if (other.GetComponent<IBlock>() == null || !isActive) return;
        if (block==null || block.GetComponent<Throwable>().attached) return;
        block.transform.SetParent(transform);
        droppedGameObject = block;
        droppedGameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        droppedGameObject.transform.localPosition = Vector3.zero;
    }
    public void OnTriggerExit(Collider other)
    {
        print("trigger exit");
        if (other.GetComponent<IBlock>() == null || !isActive) return;
        block = null;
    }
}
