using UnityEngine;

public class GenericBlock : MonoBehaviour , IBlock
{
    public bool isDragged { get; set; }
    public bool isInMain { get; set; }
    // Start is called before the first frame update
    private void Start()
    {
        isDragged = false;
        isInMain = false;
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void ErrorMessage(ErrorCode errorCode)
    {
        throw new System.NotImplementedException();
    }

    public float RecoursiveGetSize(GameObject toResizeBlock, GameObject endStatement)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            isInMain = collision.CompareTag("Main");
    }

}
