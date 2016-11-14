using UnityEngine;
using System.Collections;

public class PlayerPrefsHandler
{
	#region PlayerPrefs keys
	public const string MUTE_INT = "mute";
	#endregion

	public void ApplyPreferences()
	{
		SetMuted(GetIsMuted());
	}

	public void SetMuted(bool value)
	{
		PlayerPrefs.SetInt(MUTE_INT, value ? 1 : 0);
		AudioListener.pause = value;
		Debug.LogFormat("SetMuted({0})", value);
	}

	public bool GetIsMuted()
	{
		return PlayerPrefs.GetInt(MUTE_INT, 0) == 1;
	}
}
