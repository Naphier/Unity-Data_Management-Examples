using UnityEngine;
using UnityEngine.UI;
using DataService = EX1.DataService;
// We'll switch to this in part 2
//using DataService = EX2.DataService;

[RequireComponent(typeof(Slider))]
public class VolumeSliderHandler : MonoBehaviour
{
	void Start()
	{
		Slider slider = GetComponent<Slider>();
		slider.value = DataService.Instance.prefs.GetVolume();
		slider.onValueChanged.AddListener(
			(float value) =>
			{
				DataService.Instance.prefs.SetVolume(value);
			});
	}
}
