using TMPro;
using UnityEngine;
using Valve.VR;

public class LetterKeyVR : KeyVR
{
	public TextMeshPro shiftedLabel;

	public string character = "";

	public string shiftedChar = "";

	private bool _shifted = false;

	public bool shifted
	{
		get { return _shifted; }
		set
		{
			_shifted = value;
			label.text = _shifted ? shiftedChar : character;
			shiftedLabel.text = _shifted ? character : shiftedChar;
		}
	}

	public string GetCharacter()
	{
		return _shifted ? shiftedChar : character;
	}

	public override void HandleTriggerEnter(Collider other)
	{
		keyboard.AddCharacter(GetCharacter());
	}

	public void DoAction(GameObject key)
	{
		var keyLabel = key.transform.Find("Label").GetComponent<TextMeshPro>().text;
		print("KeyVR: key pressed - " + keyLabel);

		if ((string.IsNullOrEmpty(keyLabel))) return;

		// Check the type of key selected
		switch (keyLabel)
		{
			// Call KeyBoardVR method to shift the char
			case "SHIFT":
				keyboard.ToggleShift();
				break;

			// Call KeyBoardVR method to remove char from string
			case "BACKSPACE":
				keyboard.Backspace();
				break;

			// Call KeyBoardVR method to submit the string
			case "ENTER":
				keyboard.Submit();
				break;

			// Call KeyBoardVR method to add char to string
			default:
				keyboard.AddCharacter(keyLabel);
				break;
		}
	}
}
