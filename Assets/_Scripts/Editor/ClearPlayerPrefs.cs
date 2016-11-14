using UnityEngine;
using UnityEditor;

public class ClearPlayerPrefs : Editor
{
	[MenuItem("Edit/Clear All PlayerPrefs")]
	static void ClearAll()
	{
		PlayerPrefs.DeleteAll();
	}
}