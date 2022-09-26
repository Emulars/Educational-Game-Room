/* SceneHandler.cs*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class LaserGrabber : MonoBehaviour
{
    private SteamVR_LaserPointer laserPointer;
    private Transform grabbedObject;
    private bool isInside = false;

    void Awake()
    {
        laserPointer = GetComponent<SteamVR_LaserPointer>();
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    private void Update()
    {
        if (isInside && SteamVR_Input.GetState("default", "InteractUI", SteamVR_Input_Sources.Any))
        {
            grabbedObject.SendMessage("HandHoverUpdate", GetComponent<Hand>(),SendMessageOptions.DontRequireReceiver);
        }
    }


    public void PointerClick(object sender, PointerEventArgs e)
    {   
        // Virtual Keyboard
        if (e.target.name.Contains("Key"))
        {
            e.target.GetComponent<LetterKeyVR>().DoAction(e.target.gameObject);
        }
        if (e.target.name.Contains("Shift"))
        {
            e.target.GetComponent<ShiftKeyVR>().DoAction(e.target.gameObject);
        }
        if (e.target.name.Contains("Backspace"))
        {
            e.target.GetComponent<BackspaceKeyVR>().DoAction(e.target.gameObject);
        }

        if (e.target.GetComponent<Button>() != null)
        {
            Debug.Log("Button was clicked");
            var btt = e.target.GetComponent<Button>();
            btt.onClick.Invoke();
        }
        else if (e.target.name == "Image")
        {
            Debug.Log("Image was clicked");
        }        
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {              
        grabbedObject = e.target;
        isInside = true;
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        isInside = false;
        grabbedObject = null;
    }
}