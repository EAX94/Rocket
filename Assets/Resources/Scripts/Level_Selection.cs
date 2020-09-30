using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Selection : MonoBehaviour {
	public GameObject[] uiButtons;
	
	void Start() {
		int maxLevel = PlayerPrefs.GetInt(Helper.keyActualLevel, 1);

		for(int i = 0; i < uiButtons.Length; ++i) {
			if(maxLevel > i) {
				uiButtons[i].GetComponent<Button>().interactable = true;
			} else {
				uiButtons[i].GetComponent<Button>().interactable = false;
			}
		}
	}

	public void LoadLevel(Button btn) {
		try {
			GameObject bgAudio = GameObject.Find("BackgroundAudio");

			if(bgAudio != null) {
				Destroy(bgAudio.gameObject);
			}

			int value = int.Parse(btn.GetComponentInChildren<Text>().text);
			GlobalSettings.SceneLevels level = (GlobalSettings.SceneLevels)value + 2;
			SceneManager.LoadScene((int)level);
		} catch(System.Exception) {
			Debug.Log("Level out of range!");
		}

	}

	public void LoadMainMenu() {
		SceneManager.LoadScene((int)GlobalSettings.SceneLevels.Menu);
	}

	public void Retry() {
		SceneManager.LoadScene((int)GlobalSettings.SceneLevels.Level_Selection);
	}
}
