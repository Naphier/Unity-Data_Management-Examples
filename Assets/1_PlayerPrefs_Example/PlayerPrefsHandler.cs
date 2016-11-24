using UnityEngine;

/// <summary>
/// Handles the saving, recalling, and applying of all PlayerPrefs for the application.
/// </summary>
public class PlayerPrefsHandler
{
	/// <summary>
	/// Storing the PlayerPrefs keys in constants is a good practice!
	/// This saves you from having to make multiple changes in your code should you change the key value,
	/// allows you to make use of Intellisense for typing out the key (intead of mistyping the actual string),
	/// and since it is public and const you can access it anywhere without needing an instance of this class
	/// (i.e. by typing PlayerPrefsHandler.MUTE_INT).
	/// I like to append my PlayerPrefs keys with the type of the pref (i.e. _INT, _STR, _F)
	/// </summary>
	#region PlayerPrefs keys
	public const string MUTE_INT = "mute";
	public const string VOLUME_F = "volume";
	#endregion

	private const bool DEBUG_ON = true;

	/// <summary>
	/// This method should call all other methods that will apply saved or default preferences.
	/// We should call this as soon as possible when loading our application.
	/// </summary>
	public void RestorePreferences()
	{
		SetMuted(GetIsMuted());
		SetVolume(GetVolume());
	}

	/// <summary>
	/// Sets the AudioListener to be (un)muted and saves the value to player prefs.
	/// </summary>
	/// <param name="muted">Whether we should mute or not.</param>
	public void SetMuted(bool muted)
	{
		// Set the MUTE_INT key to 1 if muted, 0 if not muted
		PlayerPrefs.SetInt(MUTE_INT, muted ? 1 : 0);

		// Pausing the AudioListener will disable all sounds.
		AudioListener.pause = muted;

		if (DEBUG_ON)
			Debug.LogFormat("SetMuted({0})", muted);
	}

	/// <summary>
	/// Reads from PlayerPrefs to tell us if we should mute or not.
	/// </summary>
	/// <returns>Whether the MUTE_INT pref has been set to 1 or not.</returns>
	public bool GetIsMuted()
	{
		// If the value of the MUTE_INT key is 1 then sound is muted, otherwise it is not muted.
		// The default value of the MUTE_INT key is 0 (i.e. not muted).
		return PlayerPrefs.GetInt(MUTE_INT, 0) == 1;
	}

	/// <summary>
	/// Sets the volume on the AudioListener and saves the value to PlayerPrefs.
	/// </summary>
	/// <param name="volume">A value between 0 and 1</param>
	public void SetVolume(float volume)
	{
		// Prevent values less than 0 and greater than 1 from
		// being stored in the PlayerPrefs (AudioListener.volume expects a value between 0 and 1).
		volume = Mathf.Clamp(volume, 0, 1);

		PlayerPrefs.SetFloat(VOLUME_F, volume);
		AudioListener.volume = volume;
	}


	/// <summary>
	/// Retrieves the stored or default (1) volume from PlayerPrefs
	/// and ensures it is no less than 0 and no greater than 1
	/// </summary>
	/// <returns>The volume setting between 0 and 1</returns>
	public float GetVolume()
	{
		return Mathf.Clamp(PlayerPrefs.GetFloat(VOLUME_F, 1), 0, 1);
	}
}
