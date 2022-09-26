using TMPro;
using UnityEngine;
using Valve.VR;

public class BackspaceKeyVR : KeyVR
{
	public override void HandleTriggerEnter(Collider other)
	{
		keyboard.Backspace();
	}

	public void DoAction(GameObject key)
	{
		var keyLabel = key.transform.Find("Label").GetComponent<TextMeshPro>().text;
		print("KeyVR: key pressed - " + keyLabel);

		if ((string.IsNullOrEmpty(keyLabel))) return;

		if (keyLabel.Equals("BACKSPACE"))
			keyboard.Backspace();
	}
}
