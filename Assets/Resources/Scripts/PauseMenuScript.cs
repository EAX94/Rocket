using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {
	public Canvas pauseMenu;
	public Canvas confirmExitMenu;
	public Canvas optionMenu;
	private float timeValue;

	void Start() {
		pauseMenu = pauseMenu.GetComponent<Canvas>();
		confirmExitMenu = confirmExitMenu.GetComponent<Canvas>();
		optionMenu = optionMenu.GetComponent<Canvas>();
		pauseMenu.enabled = false;
		confirmExitMenu.enabled = false;
		optionMenu.enabled = false;
	}

	void Update() {
		if(!pauseMenu.enabled) {
			if(Input.GetKeyDown(KeyCode.Escape)) {
				Time.timeScale = 0;
				pauseMenu.enabled = true;
			}
		} else {
			if(Input.GetKeyDown(KeyCode.Escape)) {
				if(!optionMenu.enabled && !confirmExitMenu.enabled) {
					pauseMenu.enabled = false;
					Time.timeScale = 1f;
				}
			}
		}
	}

	public void ResumePressed() {
		Time.timeScale = 1f;
		pauseMenu.enabled = false;
	}

	public void ExitMenuPressed() {
		confirmExitMenu.enabled = true;
	}

	public void NoPressed() {
		confirmExitMenu.enabled = false;
	}

	public void YesPressed() {
		if(SceneManager.GetActiveScene().buildIndex == (int)GlobalSettings.SceneLevels.Menu) {
			Application.Quit();
		} else {
			SceneManager.LoadScene((int)GlobalSettings.SceneLevels.Menu);
		}
	}

	public void Quit_Button() {
		if(SceneManager.GetActiveScene().buildIndex == (int)GlobalSettings.SceneLevels.Menu) {
			Application.Quit();
		} else {
			SceneManager.LoadScene((int)GlobalSettings.SceneLevels.Menu);
		}
	}

	public void OptionsPausePressed() {
		optionMenu.enabled = true;

		Slider[] sliderOptions = optionMenu.GetComponentsInChildren<Slider>(false);

		sliderOptions[0].value = ConfigOptions.LoadSFX();
		sliderOptions[1].value = ConfigOptions.LoadMusic();
	}

	public void DiscardPressed() {
		this.SetBGAudioVolume(ConfigOptions.LoadMusic());
		optionMenu.enabled = false;
	}

	public void SavePressed() {
		Slider[] sliderOptions = optionMenu.GetComponentsInChildren<Slider>(false);

		ConfigOptions.SaveSFX((int)sliderOptions[0].value);
		ConfigOptions.SaveMusic((int)sliderOptions[1].value);

		optionMenu.enabled = false;
	}

	public void SliderSFXValueChange() {
		Slider[] sliderOptions = optionMenu.GetComponentsInChildren<Slider>(false);
		/*GameObject soundExample = GameObject.Find("audioTest");

		if(soundExample != null) {
			soundExample.GetComponent<AudioSource>().volume = (0.7f * sliderOptions[0].value) / 100;

			if(!soundExample.GetComponent<AudioSource>().isPlaying) {
				soundExample.GetComponent<AudioSource>().Play();
			}
		}*/

		GameObject portalExit = GameObject.Find("Portal_Exit_0 (1)");

		if(portalExit != null) {
			// Volume 0.7 = 100%
			portalExit.GetComponent<AudioSource>().volume = (0.7f * sliderOptions[0].value) / 100;
		}

		GameObject rocketCharacter = GameObject.Find("RocketCharacter");

		if(rocketCharacter != null) {
			// Volume 0.7 = 100%
			rocketCharacter.GetComponent<AudioSource>().volume = (0.7f * sliderOptions[0].value) / 100;
		}
	}

	public void SliderMusicValueChange() {
		Slider[] sliderOptions = optionMenu.GetComponentsInChildren<Slider>(false);
		this.SetBGAudioVolume(sliderOptions[1].value);
	}

	public void SetBGAudioVolume(float x) {
		GameObject bgAudio = GameObject.Find("BackgroundAudio");

		if(bgAudio != null) {
			// Volume 0.3 = 100%
			bgAudio.GetComponent<AudioSource>().volume = (0.3f * x) / 100;
		}
	}
}
