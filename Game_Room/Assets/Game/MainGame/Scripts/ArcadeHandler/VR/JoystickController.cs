using UnityEngine;
using Valve.VR.InteractionSystem;

public class JoystickController : MonoBehaviour
{
    [Header("movement values")]
    [SerializeField] private float smoothLookSpeed = 15f;
    [SerializeField] private float minDegrees = -45f, maxDegrees = 45f;
    [SerializeField] private float angleX, angleY;
    [SerializeField] private float LeverPercentageX = 0, LeverPercentageY = 0;
    [SerializeField] private float deadZone = 0.001f;
    public static Vector2 leverVector;
    private Interactable interactable;
    private Rigidbody rb;
    Quaternion initialRotation;
    Vector3 currentRotation;
    // Awake is called before the first frame update
    void Awake()
    {
        interactable = this.GetComponent<Interactable>();
        rb = this.GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
       
    }
    void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    private void OnAttachedToHand(Hand hand)
    {
        if (rb) rb.isKinematic = false;
    }
    //-------------------------------------------------
    // Called every Update() while this GameObject is attached to the hand
    //-------------------------------------------------
    private void HandAttachedUpdate(Hand hand)
    {
        void doJoystickLook()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Quaternion originalRotation = transform.rotation;
            // Use the Grabber as our look target
            // Convert to local position so we can remove the x axis
            //Vector3 localTargetPosition = transform.InverseTransformPoint(grab.GetPrimaryGrabber().transform.position);
            Vector3 localTargetPosition = transform.InverseTransformPoint(hand.transform.position);

            // Convert back to world position 
            Vector3 targetPosition = transform.TransformPoint(localTargetPosition);
            transform.LookAt(targetPosition, transform.up);

            //Smooth transition
            Quaternion newRot = transform.rotation;
            transform.rotation = originalRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.fixedDeltaTime * smoothLookSpeed);        
        }
        doJoystickLook();


        // Lock our local position and axis in Update to avoid jitter
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

        // Get the modified angle of of the lever. Use this to get percentage based on Min and Max angles.
        currentRotation = transform.localEulerAngles;
        angleX = Mathf.Floor(currentRotation.x);
        angleX = (angleX > 180) ? angleX - 360 : angleX;

        angleY = Mathf.Floor(currentRotation.y);
        angleY = (angleY > 180) ? angleY - 360 : angleY;

        // Cap Angles X
        if (angleX > maxDegrees)
        {
            transform.localEulerAngles = new Vector3(maxDegrees, currentRotation.y, currentRotation.z);
        }
        else if (angleX < minDegrees)
        {
            transform.localEulerAngles = new Vector3(minDegrees, currentRotation.y, currentRotation.z);
        }

        // Cap Angles Z
        if (angleY > maxDegrees)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, currentRotation.y, maxDegrees);
        }
        else if (angleY < minDegrees)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, currentRotation.y, minDegrees);
        }

        // Set percentage of level position
        LeverPercentageX = (angleY - minDegrees) / (maxDegrees - minDegrees) * 100;
        LeverPercentageY = (angleX - minDegrees) / (maxDegrees - minDegrees) * 100;

        // Lever value changed event
        //OnJoystickChange(LeverPercentageX, LeverPercentageY);

        // Lever Vector Changed Event
        float xInput = Mathf.Lerp(-1f, 1f, LeverPercentageX / 100);
        float yInput = Mathf.Lerp(-1f, 1f, LeverPercentageY / 100);

        // Reset any values that are inside the deadzone
        if (deadZone > 0)
        {
            if (Mathf.Abs(xInput) < deadZone)
            {
                xInput = 0;
            }
            if (Mathf.Abs(yInput) < deadZone)
            {
                yInput = 0;
            }
        }

        leverVector = new Vector2(-xInput, -yInput);
    }
    private void OnDetachedFromHand(Hand hand)
    {
        leverVector = new Vector2(0, 0);
        if (rb) rb.isKinematic = true;
        //rimetto tutto a porto
        transform.localRotation = 
            Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * smoothLookSpeed);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}