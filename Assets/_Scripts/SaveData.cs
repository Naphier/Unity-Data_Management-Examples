using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveData
{
	public const string FIRST_LEVEL = "level1";
	private const int DEFAULT_COINS = 0;
	private const int DEFAULT_HEALTH = 100;
	private const int DEFAULT_LIVES = 3;

	public int coins = DEFAULT_COINS;
	public int health = DEFAULT_HEALTH;
	public int lives = DEFAULT_LIVES;
	public List<PowerUp> powerUps = new List<PowerUp>();
	public string lastLevel = FIRST_LEVEL;

	public void WriteToFile(string filePath)
	{
		string json = JsonUtility.ToJson(this, true);
		File.WriteAllText(filePath, json);
		Debug.LogFormat("WriteToFile({0}) -- data:\n{1}", filePath, json);
	}

	public static SaveData ReadFromFile(string filePath)
	{
		if (!File.Exists(filePath))
		{
			Debug.LogFormat("ReadFromFile({0}) -- file not found, returning new object", filePath);
			return new SaveData();
		}
		else
		{
			string contents = File.ReadAllText(filePath);
			Debug.LogFormat("ReadFromFile({0})\ncontents:\n{1}", filePath, contents);
			return JsonUtility.FromJson<SaveData>(contents);
		}
	}

	public bool IsDefault()
	{
		return (
			coins == DEFAULT_COINS &&
			health == DEFAULT_HEALTH &&
			lives == DEFAULT_LIVES &&
			lastLevel == FIRST_LEVEL &&
			powerUps.Count == 0);
	}
}

public enum PowerUp
{
	Fireballs,
	DoubleJump
}
