using UnityEngine;
using UnityEngine.UI;
using DataService = EX1.DataService;
// We'll switch to this in part 2
//using DataService = EX2.DataService;

/// <summary>
/// Handles the initial setting of the UI Toggle value
/// and assigns the onValueChanged listener to the UI Toggle component.
/// </summary>
// RequireComponent ensures that when we can only add this component to a UI Toggle
// It also ensures that when we attempt to GetComponent<Toggle> that it exists.
[RequireComponent(typeof(Toggle))]
public class MuteToggleHandler : MonoBehaviour
{
	void Start()
	{
		// Get the reference to the attached toggle component.
		Toggle toggle = GetComponent<Toggle>();

		// Set the initial value that was stored in player prefs.
		toggle.isOn = DataService.Instance.prefs.GetIsMuted();

		// Set up the onValueChanged listener
		// This is done here instead of in the inspector for a few reasons:
		//	- DataService contains the PlayerPrefsHandler reference and Unity won't let us
		//	  access that through the inspector.
		//	- DataService is a singleton and may or may not be in the scene so we can't always
		//	  assign it to via the inspector.
		//	- This makes the script completely self-contained. No other script needs access to this script.
		// The only fallback is that this class is not extensible, but it really doesn't need to be.
		toggle.onValueChanged.AddListener(
			(bool value) => 
			{
				DataService.Instance.prefs.SetMuted(value);
			});
	}
}
