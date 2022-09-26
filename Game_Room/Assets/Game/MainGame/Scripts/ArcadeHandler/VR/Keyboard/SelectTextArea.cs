using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class SelectTextArea : MonoBehaviour
{
    public static InputField input;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "LaserPointer") return;

        if(SteamVR_Input.GetStateDown("default", "SelectTextArea", SteamVR_Input_Sources.Any))
        {
            input = GetComponent<InputField>();
            print("SelectTextArea: " + input.text);
        }
    }
}