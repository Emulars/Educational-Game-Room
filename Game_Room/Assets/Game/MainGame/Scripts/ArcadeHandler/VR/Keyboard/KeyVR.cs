using System.Collections;
using TMPro;
using UnityEngine;
using Valve.VR;

public class KeyVR : MonoBehaviour
{
	public KeyboardVR keyboard;

	public TextMeshPro label;

	public Material inactiveMat;

	public Material activeMat;

	public Material disabledMat;

	public Vector3 defaultPosition;

	public Vector3 pressedPosition;

	public Vector3 pressDirection = Vector3.down;

	public float pressMagnitude = 0.1f;

	public bool autoInit = false;

	private bool isPressing = false;

	private bool disabled = false;

	protected MeshRenderer meshRenderer;

	private void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();

		if (autoInit)
		{
			Init(transform.localPosition);
		}
	}

	/// <summary>
	/// Initialize the key with a default position and pressed position.
	/// </summary>
	/// <param name="defaultPos">Default position.</param>
	public void Init(Vector3 defaultPos)
	{
		defaultPosition = defaultPos;
		pressedPosition = defaultPos + (Vector3.down * 0.01f);
	}

	private void OnEnable()
	{
		isPressing = false;
		disabled = false;
		transform.localPosition = defaultPosition;
		meshRenderer.material = inactiveMat;

		OnEnableExtras();
	}

	/// <summary>
	/// Override this to add custom logic on enable.
	/// </summary>
	protected virtual void OnEnableExtras()
	{
		// Override me!
	}

	/// <summary>
	/// Override this to handle trigger events. Only fires when
	/// a downward trigger event occurred from the collider
	/// matching keyboard.colliderName.
	/// </summary>
	/// <param name="other">Collider.</param>
	public virtual void HandleTriggerEnter(Collider other)
	{
		// Override me!
	}

}
