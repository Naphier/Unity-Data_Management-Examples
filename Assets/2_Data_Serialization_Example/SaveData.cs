using UnityEngine;
using System.IO; // Required fro reading/writing to files.
using System.Collections.Generic; // Used for Lists

/// <summary>
/// The different types of powerups a player can have.
/// </summary>
public enum PowerUp
{
	Fireballs,
	DoubleJump
}

/// <summary>
/// Responsible for:
/// - Maintaining the stats for a player and their progress
/// - Writing this data to a file.
/// - Reading this data from a file.
/// </summary>
public class SaveData
{
	#region Defaults
	public const string DEFAULT_LEVEL = "level1";
	private const int DEFAULT_COINS = 0;
	private const int DEFAULT_HEALTH = 100;
	private const int DEFAULT_LIVES = 3;
	#endregion

	// We initialize all of the stats to be default values.
	public int coins = DEFAULT_COINS;
	public int health = DEFAULT_HEALTH;
	public int lives = DEFAULT_LIVES;
	public List<PowerUp> powerUps = new List<PowerUp>();
	public string lastLevel = DEFAULT_LEVEL;

	const bool DEBUG_ON = true;

	/// <summary>
	/// Writes the instance of this class to the specified file in JSON format.
	/// </summary>
	/// <param name="filePath">The file name and full path to write to.</param>
	public void WriteToFile(string filePath)
	{
		// Convert the instance ('this') of this class to a JSON string with "pretty print" (nice indenting).
		string json = JsonUtility.ToJson(this, true);

		// Write that JSON string to the specified file.
		File.WriteAllText(filePath, json);

		// Tell us what we just wrote if DEBUG_ON is on.
		if (DEBUG_ON)
			Debug.LogFormat("WriteToFile({0}) -- data:\n{1}", filePath, json);
	}

	/// <summary>
	/// Returns a new SaveData object read from the data in the specified file.
	/// </summary>
	/// <param name="filePath">The file to attempt to read from.</param>
	public static SaveData ReadFromFile(string filePath)
	{
		// If the file doesn't exist then just return the default object.
		if (!File.Exists(filePath))
		{
			Debug.LogErrorFormat("ReadFromFile({0}) -- file not found, returning new object", filePath);
			return new SaveData();
		}
		else
		{
			// If the file does exist then read the entire file to a string.
			string contents = File.ReadAllText(filePath);

			// If debug is on then tell us the file we read and its contents.
			if (DEBUG_ON)
				Debug.LogFormat("ReadFromFile({0})\ncontents:\n{1}", filePath, contents);

			// If it happens that the file is somehow empty then tell us and return a new SaveData object.
			if (string.IsNullOrEmpty(contents))
			{
				Debug.LogErrorFormat("File: '{0}' is empty. Returning default SaveData");
				return new SaveData();
			}

			// Otherwise we can just use JsonUtility to convert the string to a new SaveData object.
			return JsonUtility.FromJson<SaveData>(contents);
		}
	}

	/// <summary>
	/// This is used to check if the SaveData object is the same as the default.
	/// i.e. it hasn't been written to yet.
	/// </summary>
	public bool IsDefault()
	{
		return (
			coins == DEFAULT_COINS &&
			health == DEFAULT_HEALTH &&
			lives == DEFAULT_LIVES &&
			lastLevel == DEFAULT_LEVEL &&
			powerUps.Count == 0);
	}

	/// <summary>
	/// A friendly string representation of this object.
	/// </summary>
	public override string ToString()
	{
		string[] powerUpsStrings = new string[powerUps.Count];
		for (int i = 0; i < powerUps.Count; i++)
		{
			powerUpsStrings[i] = powerUps[i].ToString();
		}

		return string.Format(
			"coins: {0}\nhealth: {1}\nlives: {2}\npowerUps: {3}\nlastLevel: {4}",
			coins, 
			health, 
			lives, 
			"[" + string.Join(",", powerUpsStrings) + "]",
			lastLevel
			);
	}
}

