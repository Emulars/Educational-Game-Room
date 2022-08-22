using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchBlocks : MonoBehaviour
{
    public static bool launch = false;
    private Button startBtt;

    void Start()
    {
        startBtt = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!launch)return;
        startBtt.onClick.Invoke();
        launch = false;
    }
}
