using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton - There should only ever be one DataService and it should persist
/// between scene loads.
/// This class is responsible for loading/saving data.
/// </summary>
public class DataService : MonoBehaviour
{

	private static DataService _instance = null;
	public static DataService Instance
	{
		get
		{
			// If the instance of this class doesn't exist
			if (_instance == null)
			{
				// Check the scene for a Game Object with this class
				_instance = FindObjectOfType<DataService>();

				// If none is found in the scene then create a new Game Object
				// and add this class to it.
				if (_instance == null)
				{
					GameObject go = new GameObject(typeof(DataService).ToString());
					_instance = go.AddComponent<DataService>();
				}
			}

			return _instance;
		}
	}

	public PlayerPrefsHandler prefs { get; private set; }

	// When the scene first runs ensure that there is only one
	// instance of this class. This allows us to add it to any scene and 
	// not conflict with any pre-existing instance from a previous scene.
	private void Awake()
	{
		if (Instance != this)
		{
			Destroy(this);
		}
		else
		{
			DontDestroyOnLoad(gameObject);

			prefs = new PlayerPrefsHandler();
			prefs.ApplyPreferences();
			LoadSaveData();
			WriteSaveData();
		}
	}
	


	public SaveData SaveData { get; private set; }
	bool isDataLoaded = false;
	int currentlyLoadedProfileNumber = 0;
	public const int MAX_NUMBER_OF_PROFILES = 3;
	public void LoadSaveData(int profileNumber = 0)
	{
		if (isDataLoaded && profileNumber == currentlyLoadedProfileNumber)
			return;

		// Automatically load the first available profile.
		if (profileNumber <= 0)
		{
			for (int i = 1; i <= MAX_NUMBER_OF_PROFILES; i++)
			{
				if (File.Exists(GetSaveDataFilePath(i)))
				{
					SaveData = SaveData.ReadFromFile(GetSaveDataFilePath(i));
					currentlyLoadedProfileNumber = i;
					break;
				}
			}
		}
		else
		{
			if (File.Exists(GetSaveDataFilePath(profileNumber)))
			{
				SaveData = SaveData.ReadFromFile(GetSaveDataFilePath(profileNumber));
				
			}
			else
			{
				SaveData = new SaveData();
			}

			currentlyLoadedProfileNumber = profileNumber;
		}
	}

	private const string SAVE_DATA_FILE_NAME_BASE = "savedata";
	private const string SAVE_DATA_FILE_EXTENSION = ".txt";
	private string SAVE_DATA_DIRECTORY { get { return Application.dataPath + "/saves/"; } }

	public string GetSaveDataFilePath(int profileNumber)
	{
		if (!Directory.Exists(SAVE_DATA_DIRECTORY))
			Directory.CreateDirectory(SAVE_DATA_DIRECTORY);			

		return SAVE_DATA_DIRECTORY + SAVE_DATA_FILE_NAME_BASE + profileNumber.ToString() + SAVE_DATA_FILE_EXTENSION;
	}

	private void WriteSaveData()
	{
		if (currentlyLoadedProfileNumber <= 0)
		{
			for (int i = 1; i <= MAX_NUMBER_OF_PROFILES; i++)
			{
				if (!File.Exists(GetSaveDataFilePath(i)))
				{
					currentlyLoadedProfileNumber = i;
					break;
				}
			}
		}

		if (currentlyLoadedProfileNumber <= 0)
		{
			throw new System.Exception("Cannot WriteSaveData. No available profiles and currentlyLoadedProfile = 0");
		}
		else
		{
			if (SaveData == null)
				SaveData = new SaveData();

			SaveData.WriteToFile(GetSaveDataFilePath(currentlyLoadedProfileNumber));
		}
	}

	void OnLevelWasLoaded()
	{
		Debug.Log("Level loaded");
		WriteSaveData();
	}
}
