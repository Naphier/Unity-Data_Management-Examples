using UnityEngine;

namespace EX1
{
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
				prefs.RestorePreferences();
				// In Unity 5.4 OnLevelWasLoaded has been deprecated and the action
				// now occurs through this callback.
#if UNITY_5_4_OR_NEWER
				SceneManager.sceneLoaded += OnLevelWasLoaded;
#endif
			}
		}

		/// <summary>
		/// Ensure that the player preferences are applied to the new scene.
		/// </summary>
		// In Unity 5.4 OnLevelWasLoaded has been deprecated and the action
		// now occurs through 'SceneManager.sceneLoaded' callback.
		void OnLevelWasLoaded()
		{
			prefs.RestorePreferences();
		}
	}
}