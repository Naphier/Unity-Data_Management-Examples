using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using EX2;

/// <summary>
/// Attach this to any game object. I like to attach it to the canvas containing the buttons that will
/// load profiles. That way it's in a 'logical' place and easy to find.
/// </summary>
public class SaveSlotButtonHandler : MonoBehaviour
{
	/// <summary>
	/// Assign each of the button labels here. They should be in order of their appearance (top to bottom).
	/// </summary>
	public Text[] buttonLabels;

	/// <summary>
	/// This is the text that will display when 
	/// </summary>
	private const string EMPTY_SLOT = "New Game";
	private const string USED_SLOT = "Load Save ";

	void Start()
	{
		SetButtonLabels();
	}

	/// <summary>
	/// Sets the label on each button to indicate whether we're loading an empty slot or
	/// loading an actual profile.
	/// </summary>
	void SetButtonLabels()
	{
		if (buttonLabels.Length != DataService.MAX_NUMBER_OF_PROFILES)
		{
			Debug.LogError(
				"Incorrect number of button labels. Must be exactly " +
				DataService.MAX_NUMBER_OF_PROFILES);
		}
		else
		{
			// For every possible profile number.
			for (int i = 0; i < DataService.MAX_NUMBER_OF_PROFILES; i++)
			{
				// If the profile file exists,
				// Then set the label to say the profile exists (i.e. 'Load Save 1')
				if (File.Exists(DataService.Instance.GetSaveDataFilePath(i + 1)))
				{
					buttonLabels[i].text = USED_SLOT + (i + 1).ToString();
				}
				else
				{
					// Otherwise set the label to just say 'New Game" indicating it is an empty slot.
					buttonLabels[i].text = EMPTY_SLOT;
				}
			}
		}
	}

	// This should be assigned to each button via the inspector.
	// The parameter in the inspector's on click event will be 1,2, or 3
	/// <summary>
	/// Called from the OnClick methods for buttons.
	/// </summary>
	/// <param name="profileNumber"></param>
	public void LoadGame(int profileNumber)
	{
		// Load the save data file
		DataService.Instance.LoadSaveData(profileNumber);
		// Load the last level the player was in
		SceneManager.LoadScene(DataService.Instance.SaveData.lastLevel);
	}
}

