using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveSlotButtonHandler : MonoBehaviour
{
	public Text[] buttonLabels;
	private const string EMPTY_SLOT = "New Game";
	private const string USED_SLOT = "Load Save ";

	void Start()
	{
		if (buttonLabels.Length != DataService.MAX_NUMBER_OF_PROFILES)
		{
			Debug.LogError(
				"Incorrect number of button labels. Must be exactly " +
				DataService.MAX_NUMBER_OF_PROFILES);
		}
		else
		{
			for (int i = 0; i < DataService.MAX_NUMBER_OF_PROFILES; i++)
			{
				if (File.Exists(DataService.Instance.GetSaveDataFilePath(i + 1)) &&
					!DataService.Instance.SaveData.IsDefault())
				{
					buttonLabels[i].text = USED_SLOT + (i + 1).ToString();
				}
				else
				{
					buttonLabels[i].text = EMPTY_SLOT;
				}
			}
		}
	}

	/// <summary>
	/// Called from the OnClick methods for buttons.
	/// </summary>
	/// <param name="profileNumber"></param>
	public void LoadGame(int profileNumber)
	{
		DataService.Instance.LoadSaveData(profileNumber);
		SceneManager.LoadScene(DataService.Instance.SaveData.lastLevel);
	}
}
