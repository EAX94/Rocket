using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalSettings : MonoBehaviour {
	private int actualLevel = 1;

	public Texture2D cursorTexture;
	private Vector2 cursorVector;

	public GameObject startPosition;

    public enum SceneLevels
    {
        Intro_UAI = 0,
        Menu = 1,
        Level_Selection = 2,
        Level_1 = 3,
        Level_2 = 4,
        Level_3 = 5,
        Level_4 = 6,
        Level_5 = 7,
        Level_6 = 8,
        Level_7 = 9,
        Level_8 = 10,
        Level_9 = 11,
        Level_10 = 12,
        Level_11 = 13,
        Level_12 = 14,
        Level_13 = 15,
        Level_14 = 16,
        Game_Finish = 17
    }

	void Start() {
		//Cursor.visible = false;

		GameObject[] bgAudio = GameObject.FindGameObjectsWithTag("BackgroundAudio");

		if(bgAudio.Length > 1) {
			Destroy(bgAudio[1]);
		}

		this.LoadConfig();

		int maxLevel = PlayerPrefs.GetInt(Helper.keyActualLevel, 1);
		this.actualLevel = SceneManager.GetActiveScene().buildIndex;

		if((this.actualLevel - (int)SceneLevels.Level_1 + 1) > maxLevel) {
			PlayerPrefs.SetInt(Helper.keyActualLevel, this.actualLevel - 2);
			PlayerPrefs.Save();
		}

		if(this.actualLevel != (int)SceneLevels.Level_1) {
			this.GetComponent<FirePortal>().firePortal = true;
		}

		this.cursorVector = new Vector2(this.cursorTexture.width / 2, this.cursorTexture.height / 2);

		Cursor.SetCursor(this.cursorTexture, this.cursorVector, CursorMode.Auto);
	}

	public void OnApplicationQuit() {
		PlayerPrefs.Save();
	}

	public void NextLevel() {
		this.transform.position = new Vector3(999999.9f, 999999.9f);

		CameraFade.StartAlphaFade(Color.black, false, 2f, 0f, () => {
			++this.actualLevel;

			if(this.actualLevel == (int)SceneLevels.Game_Finish) {
				SceneManager.LoadScene((int)SceneLevels.Game_Finish);
				return;
			}

			SceneManager.LoadScene(this.actualLevel);
		});
	}

	public void ResetPortals() {
		PortalBehaviour portal1 = GameObject.Find("portal1").GetComponent<PortalBehaviour>();
		PortalBehaviour portal2 = GameObject.Find("portal2").GetComponent<PortalBehaviour>();

		portal1.PortalOff();
		portal2.PortalOff();
	}

	public void Die() {
		this.transform.position = new Vector3(999999.9f, 999999.9f);

		CameraFade.StartAlphaFade(Color.black, false, 2f, 0f, () => {
			SceneManager.LoadScene(this.actualLevel);
		});
	}

	private void LoadConfig() {
		int sfxVolume = PlayerPrefs.GetInt(Helper.keySFX, 100);
		int musicVolume = PlayerPrefs.GetInt(Helper.keyMusic, 100);

		GameObject bgAudio = GameObject.Find("BackgroundAudio");

		if(bgAudio != null) {
			// Volume 0.3 = 100%
			bgAudio.GetComponent<AudioSource>().volume = ((0.3f * musicVolume) / 100);
		}

		GameObject portalExit = GameObject.Find("Portal_Exit_0 (1)");

		if(portalExit != null) {
			// Volume 0.7 = 100%
			portalExit.GetComponent<AudioSource>().volume = ((0.7f * sfxVolume) / 100);
		}

		// Volume 0.7 = 100%
		this.GetComponent<AudioSource>().volume = ((0.7f * sfxVolume) / 100);
	}
}
