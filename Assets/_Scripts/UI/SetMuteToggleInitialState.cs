using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SetMuteToggleInitialState : MonoBehaviour
{
	void Start()
	{
		Toggle toggle = GetComponent<Toggle>();
		toggle.isOn = DataService.Instance.prefs.GetIsMuted();
		toggle.onValueChanged.AddListener((bool value) => { DataService.Instance.prefs.SetMuted(value); });
	}
}
