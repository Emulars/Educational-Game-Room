
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ButtonVR : MonoBehaviour
{
    //ButtonVRn
    public static bool button1 = false, button2 = false;
    public void OnButtonDown(Hand fromHand)
    {

        if (gameObject.name.Equals("ButtonVR1"))
            button1 = true;
        else if (gameObject.name.Equals("ButtonVR2"))
            button2 = true;
        fromHand.TriggerHapticPulse(1000);

        Debug.Log(gameObject.name + " is  pressed");
        Debug.Log("button1 is  " + button1);
        Debug.Log("button2 is  " + button2);
    }

    public void OnButtonUp(Hand fromHand)
    {
        if (gameObject.name.Equals("ButtonVR1"))
            button1 = false;
        else if (gameObject.name.Equals("ButtonVR2"))
            button2 = false;

        Debug.Log(gameObject.name + " is  up");
        Debug.Log("button1 is  " + button1);
        Debug.Log("button2 is  " + button2);
    }
}
