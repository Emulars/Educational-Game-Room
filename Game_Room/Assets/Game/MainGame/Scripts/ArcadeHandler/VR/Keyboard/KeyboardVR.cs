using System.Collections;
using TMPro;
using UnityEngine;
using Valve.VR;
using VRKeys;

public class KeyboardVR : MonoBehaviour
{
	//0f, 1.35f, 2f
	public Vector3 positionRelativeToUser = new Vector3(10f, 0.5f, 2f);

	[Space(15)]
	public GameObject keyPrefab;
	public Transform keysParent;
	public float keyWidth = 0.16f;
	public float keyHeight = 0.16f;
	public bool initialized = false;

	[Space(15)]
	public string text = "";

	[Space(15)]
	public GameObject cavasStartBtt;
	public GameObject keyboardWrapper;
	public ShiftKeyVR shiftKey;
	public KeyVR[] extraKeys;
	private LetterKeyVR[] keys;

	public KeyboardLayout keyboardLayout = KeyboardLayout.Qwerty;
	private Layout layout;
	public TextMeshProUGUI displayText;
	private bool shifted = false;

	[Space(15)]
	public Color displayTextColor = Color.black;
	public Color caretColor = Color.gray;

	// Open/close keyboard
	private bool isOpen = false;

	private IEnumerator Start()
	{
		transform.localPosition = positionRelativeToUser;
		yield return StartCoroutine(SetupKeys());
		initialized = true;
	}

    private void Update()
    {
		TriggerKeyboard();
	}

    /// <summary>
    /// Setup the keys
    /// </summary>
    private IEnumerator SetupKeys()
	{
		layout = LayoutList.GetLayout(keyboardLayout);
		keys = new LetterKeyVR[layout.TotalKeys()];
		int keyCount = 0;

		// Remove previous keys
		if (keys != null)
		{
			foreach (KeyVR key in keys)
			{
				if (key != null)
				{
					Destroy(key.gameObject);
				}
			}
		}

		// Numbers row
		for (int i = 0; i < layout.row1Keys.Length; i++)
		{
			GameObject obj = (GameObject)Instantiate(keyPrefab, keysParent);
			obj.transform.localPosition += (Vector3.right * ((keyWidth * i) - layout.row1Offset));

			LetterKeyVR key = obj.GetComponent<LetterKeyVR>();
			key.character = layout.row1Keys[i];
			key.shiftedChar = layout.row1Shift[i];
			key.shifted = false;
			key.Init(obj.transform.localPosition);

			obj.name = "Key: " + layout.row1Keys[i];
			obj.SetActive(true);

			keys[keyCount] = key;
			keyCount++;

			yield return null;
		}

		// QWERTY row
		for (int i = 0; i < layout.row2Keys.Length; i++)
		{
			GameObject obj = (GameObject)Instantiate(keyPrefab, keysParent);
			obj.transform.localPosition += (Vector3.right * ((keyWidth * i) - layout.row2Offset));
			obj.transform.localPosition += (Vector3.back * keyHeight * 1);

			LetterKeyVR key = obj.GetComponent<LetterKeyVR>();
			key.character = layout.row2Keys[i];
			key.shiftedChar = layout.row2Shift[i];
			key.shifted = false;
			key.Init(obj.transform.localPosition);

			obj.name = "Key: " + layout.row2Keys[i];
			obj.SetActive(true);

			keys[keyCount] = key;
			keyCount++;

			yield return null;
		}

		// ASDF row
		for (int i = 0; i < layout.row3Keys.Length; i++)
		{
			GameObject obj = (GameObject)Instantiate(keyPrefab, keysParent);
			obj.transform.localPosition += (Vector3.right * ((keyWidth * i) - layout.row3Offset));
			obj.transform.localPosition += (Vector3.back * keyHeight * 2);

			LetterKeyVR key = obj.GetComponent<LetterKeyVR>();
			key.character = layout.row3Keys[i];
			key.shiftedChar = layout.row3Shift[i];
			key.shifted = false;
			key.Init(obj.transform.localPosition);

			obj.name = "Key: " + layout.row3Keys[i];
			obj.SetActive(true);

			keys[keyCount] = key;
			keyCount++;

			yield return null;
		}

		// ZXCV row
		for (int i = 0; i < layout.row4Keys.Length; i++)
		{
			GameObject obj = (GameObject)Instantiate(keyPrefab, keysParent);
			obj.transform.localPosition += (Vector3.right * ((keyWidth * i) - layout.row4Offset));
			obj.transform.localPosition += (Vector3.back * keyHeight * 3);

			LetterKeyVR key = obj.GetComponent<LetterKeyVR>();
			key.character = layout.row4Keys[i];
			key.shiftedChar = layout.row4Shift[i];
			key.shifted = false;
			key.Init(obj.transform.localPosition);

			obj.name = "Key: " + layout.row4Keys[i];
			obj.SetActive(true);

			keys[keyCount] = key;
			keyCount++;

			yield return null;
		}

		// Disable the key generator 
		keyPrefab.SetActive(false);
	}

	#region Keys action
	/// <summary>
	/// Add a character to the input text.
	/// </summary>
	/// <param name="character">Character.</param>
	public void AddCharacter(string character)
	{
		SelectTextArea.input.text += character;
	}

	/// <summary>
	/// Toggle whether the characters are shifted (caps).
	/// </summary>
	public bool ToggleShift()
	{
		if (keys == null)
		{
			return false;
		}

		shifted = !shifted;

		foreach (LetterKeyVR key in keys)
		{
			key.shifted = shifted;
		}

		shiftKey.Toggle(shifted);

		return shifted;
	}

	/// <summary>
	/// Backspace one character.
	/// </summary>
	public void Backspace()
	{
		print("Backspace - text.Lentgh " + text.Length);
		if (SelectTextArea.input.text.Length > 0)
		{
			SelectTextArea.input.text = SelectTextArea.input.text.Substring(0, SelectTextArea.input.text.Length - 1);
		}
	}

	/// <summary>
	/// Submit and close the keyboard.
	/// </summary>
	public void Submit()
	{

	}
	#endregion

	// Reset the position of the keyboard and open/close
	public void TriggerKeyboard()
    {
		if (SteamVR_Input.GetStateDown("default", "OpenKeyboard", SteamVR_Input_Sources.Any))
        {
			transform.localPosition = positionRelativeToUser;
			isOpen = !isOpen;
			transform.Find("Keyboard").gameObject.SetActive(isOpen);
		}
	}
}
