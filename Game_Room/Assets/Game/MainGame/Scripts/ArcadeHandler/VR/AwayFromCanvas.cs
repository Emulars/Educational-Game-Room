using UnityEngine;

/// <summary>
/// Script to add on any Canvas object that can be dragged.
/// If this object is not in a Canvas it will be deleted from the scene.
/// </summary>

public class AwayFromCanvas : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null) // gameObject.scene.name
            Destroy(gameObject);
    }
}
