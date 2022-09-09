using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().Select();
    }

    public void Clicked()
    {
        Debug.Log("pressed");
    }
}
