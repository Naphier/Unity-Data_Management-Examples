using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

namespace EX2
{
	/// <summary>
	/// This class is responsible for loading/saving data.
	/// </summary>
	public class DataService : MonoBehaviour
	{
		private static DataService _instance = null;
		public static DataService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<DataService>();

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
				prefs.RestorePreferences();

#if UNITY_5_4_OR_NEWER
				SceneManager.sceneLoaded += OnLevelWasLoaded;
#endif
			}
		}

		void OnLevelWasLoaded()
		{
			prefs.RestorePreferences();

			// If we haven't loaded any SaveData yet then load it. 
			// This also sets the currentProfile number.
			if (SaveData == null)
				LoadSaveData();

			// Set the player's progress if this is not the main menu scene.
			// In my project this is scene 0 and scene 1
			Scene activeScene = SceneManager.GetActiveScene();
			if (activeScene.buildIndex > 1)
			{
				SaveData.lastLevel = activeScene.path.Replace("Assets/", "").Replace(".unity", "");
			}

			// Write the save data to file, saving the player's stats and progress.
			WriteSaveData();
		}



		/// <summary>
		/// The currently loaded Save Data.
		/// </summary>
		public SaveData SaveData { get; private set; }

		/// <summary>
		/// Use this to prevent reloading the data when a new scene loads.
		/// </summary>
		bool isDataLoaded = false;

		/// <summary>
		/// Store the currently loaded profile number here.
		/// </summary>
		public int currentlyLoadedProfileNumber { get; private set; }

		/// <summary>
		/// The maximum number of profiles we'll allow our users to have.
		/// </summary>
		public const int MAX_NUMBER_OF_PROFILES = 3;

		/// <summary>
		/// Loads the save data for a specific profile number. 
		/// This will eventually be called from a button.
		/// </summary>
		/// <param name="profileNumber">(Optional) the profile number to load, 
		/// omit to automatically load the first profile found.</param>
		public void LoadSaveData(int profileNumber = 0)
		{
			if (isDataLoaded && profileNumber == currentlyLoadedProfileNumber)
				return;

			// Automatically load the first available profile.
			if (profileNumber <= 0)
			{
				// We iterate through the possible profile numbers in case one with a lower number
				// no longer exists.
				for (int i = 1; i <= MAX_NUMBER_OF_PROFILES; i++)
				{
					if (File.Exists(GetSaveDataFilePath(i)))
					{
						// Once the file is found, load it from the calculated file name.
						SaveData = SaveData.ReadFromFile(GetSaveDataFilePath(i));
						// And set the current profile number for later use when we save.
						currentlyLoadedProfileNumber = i;
						break;
					}
				}
			}
			else
			{
				// If the profileNumber parameter is supplied then we'll look to see if that exists.
				if (File.Exists(GetSaveDataFilePath(profileNumber)))
				{
					// If the file exists then load the SaveData from the calculated file name.
					SaveData = SaveData.ReadFromFile(GetSaveDataFilePath(profileNumber));

				}
				else
				{
					// Otherwise just return a new
					SaveData = new SaveData();
				}

				// And set the current profile number for later use when we save.
				currentlyLoadedProfileNumber = profileNumber;
			}
		}

		/// <summary>
		/// The base name of our save data files.
		/// </summary>
		private const string SAVE_DATA_FILE_NAME_BASE = "savedata";
		/// <summary>
		/// The extension of our save data files.
		/// </summary>
		private const string SAVE_DATA_FILE_EXTENSION = ".txt";

		/// <summary>
		/// The directory our save data files will be stored in. 
		/// This is done through a getter because we're calling to a non-constant member (Application.dataPath)
		/// to construct this.
		/// </summary>
		private string SAVE_DATA_DIRECTORY { get { return Application.dataPath + "/saves/"; } }

		/// <summary>
		/// The full path and file name for our SaveData file.
		/// ex: 'c:\projectdirectory\assets\saves\savedata1.txt'
		/// </summary>
		/// <param name="profileNumber">The number profile to load (must be greater than 0).</param>
		public string GetSaveDataFilePath(int profileNumber)
		{
			// If the profile number is less than 1 then throw an exception.
			if (profileNumber < 1)
				throw new System.ArgumentException("profileNumber must be greater than 1. Was: " + profileNumber);

			// Ensure that the directory exists.
			if (!Directory.Exists(SAVE_DATA_DIRECTORY))
				Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

			// Construct the string representation of the directory + file name.
			return SAVE_DATA_DIRECTORY + SAVE_DATA_FILE_NAME_BASE + profileNumber.ToString() + SAVE_DATA_FILE_EXTENSION;
		}

		/// <summary>
		/// Writes the save data to file.
		/// </summary>
		public void WriteSaveData()
		{
			// If for some accidental reason we forgot to assign a profile number,
			// then check to see if there is any unused profile number (i.e. a file doesn't exist for it). 
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

			// If we couldn't find an empty profile then throw an exception because something went very wrong.
			if (currentlyLoadedProfileNumber <= 0)
			{
				throw new System.Exception("Cannot WriteSaveData. No available profiles and currentlyLoadedProfile = 0");
			}
			else
			{
				// Otherwise save the SaveData to file.

				// If the save data doesn't exist yet, 
				// then create a new default save data.
				if (SaveData == null)
					SaveData = new SaveData();

				// Finally save it to th file using the constructed path + file name
				SaveData.WriteToFile(GetSaveDataFilePath(currentlyLoadedProfileNumber));
			}
		}
	}
}
