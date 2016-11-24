using UnityEngine;
using UnityEngine.SceneManagement;
using EX2;

public class TestSaveData : MonoBehaviour
{
	private static TestSaveData _instance;
	void Awake()
	{
		// Making this object persist through the scenes so I only have to add it to one.
		if (_instance == null)
		{
			gameObject.name = "[TSD instance]";
			DontDestroyOnLoad(gameObject);
			_instance = this;
		}

		if (this != _instance)
			Destroy(gameObject);
	}


	void Update()
	{
		if (DataService.Instance.SaveData == null)
			return;

		if (Input.GetKeyDown(KeyCode.Alpha1))
			DataService.Instance.SaveData.coins++;

		if (Input.GetKeyDown(KeyCode.Alpha2))
			DataService.Instance.SaveData.coins--;

		if (Input.GetKeyDown(KeyCode.Q))
			DataService.Instance.SaveData.health++;

		if (Input.GetKeyDown(KeyCode.W))
			DataService.Instance.SaveData.health--;

		if (Input.GetKeyDown(KeyCode.A))
			DataService.Instance.SaveData.lives++;

		if (Input.GetKeyDown(KeyCode.S))
			DataService.Instance.SaveData.lives--;

		if (Input.GetKeyDown(KeyCode.F))
		{
			if (DataService.Instance.SaveData.powerUps.Contains(PowerUp.Fireballs))
				DataService.Instance.SaveData.powerUps.Remove(PowerUp.Fireballs);
			else
				DataService.Instance.SaveData.powerUps.Add(PowerUp.Fireballs);
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			if (DataService.Instance.SaveData.powerUps.Contains(PowerUp.DoubleJump))
				DataService.Instance.SaveData.powerUps.Remove(PowerUp.DoubleJump);
			else
				DataService.Instance.SaveData.powerUps.Add(PowerUp.DoubleJump);
		}

		if (Input.GetKeyDown(KeyCode.Return))
		{
			DataService.Instance.WriteSaveData();
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(currentSceneIndex + 1);
		}

		if (Input.GetKeyDown(KeyCode.M))
			SceneManager.LoadScene(1);
	}

	void OnGUI()
	{
		string saveData = "NOT LOADED YET";
		if (DataService.Instance.SaveData != null)
			saveData = DataService.Instance.SaveData.ToString();

		string debug = string.Format(
			"Currently Loaded Profile number: {0}\n" + 
			"SaveData: \n{1}\n\n" + 
			"Press 1/2 to inc/dec coins\n" +
			"Press Q/W to inc/dec health\n" + 
			"Press A/S to inc/dec life\n" + 
			"Press F to add/remove Fireballs powerup\n" +
			"Press D to add/remove DoubleJump powerup\n" + 
			"Press ENTER to save\n" + 
			"Press SPACE to load next level\n" + 
			"Press M for menu",
			DataService.Instance.currentlyLoadedProfileNumber,
			saveData
			);

		
		GUI.Label(new Rect(0, 0, Screen.width, Screen.height), debug);
	}


}
