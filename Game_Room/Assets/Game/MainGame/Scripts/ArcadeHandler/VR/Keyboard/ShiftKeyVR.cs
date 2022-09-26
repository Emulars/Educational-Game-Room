using TMPro;
using UnityEngine;
using Valve.VR;

public class ShiftKeyVR : KeyVR
{
	private bool shifted = false;

	public override void HandleTriggerEnter(Collider other)
	{
		keyboard.ToggleShift();
	}
	public void Toggle(bool shift)
	{
		shifted = shift;

		meshRenderer.material = shifted ? activeMat : inactiveMat;
	}

	public void DoAction(GameObject key)
	{
		var keyLabel = key.transform.Find("Label").GetComponent<TextMeshPro>().text;
		print("KeyVR: key pressed - " + keyLabel);

		if ((string.IsNullOrEmpty(keyLabel))) return;

		if(keyLabel.Equals("SHIFT"))
			keyboard.ToggleShift();
	}
}
