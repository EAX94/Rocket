using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
	public Canvas quitMenu;
	public Canvas optionMenu;
	public Button startText;
	public Button exitText;
	public Button optionText;
	public Canvas resetMenu;

	void Start() {
		Time.timeScale = 1f;

		quitMenu = quitMenu.GetComponent<Canvas>();
		optionMenu = optionMenu.GetComponent<Canvas>();
		startText = startText.GetComponent<Button>();
		exitText = exitText.GetComponent<Button>();
		optionText = optionText.GetComponent<Button>();
		resetMenu = resetMenu.GetComponent<Canvas>();
		quitMenu.enabled = false;
		optionMenu.enabled = false;
		resetMenu.enabled = false;
	}

	public void OptionsPressed() {
		optionMenu.enabled = true;
		startText.interactable = false;
		exitText.interactable = false;
		optionText.interactable = false;

		Slider[] sliderOptions = optionMenu.GetComponentsInChildren<Slider>(false);

		sliderOptions[0].value = ConfigOptions.LoadSFX();
		sliderOptions[1].value = ConfigOptions.LoadMusic();
	}

	public void ExitPressed() {
		quitMenu.enabled = true;
		startText.interactable = false;
		exitText.interactable = false;
		optionText.interactable = false;
		resetMenu.enabled = false;
	}

	public void NoPressed() {
		quitMenu.enabled = false;
		startText.interactable = true;
		exitText.interactable = true;
		optionText.interactable = true;
		resetMenu.enabled = false;
	}

	public void DiscardPressed() {
		this.SetBGAudioVolume(ConfigOptions.LoadMusic());

		optionMenu.enabled = false;
		startText.interactable = true;
		exitText.interactable = true;
		optionText.interactable = true;
	}

	public void SavePressed() {
		Slider[] sliderOptions = optionMenu.GetComponentsInChildren<Slider>(false);

		ConfigOptions.SaveSFX((int)sliderOptions[0].value);
		ConfigOptions.SaveMusic((int)sliderOptions[1].value);

		optionMenu.enabled = false;
		startText.interactable = true;
		exitText.interactable = true;
		optionText.interactable = true;
	}

	public void SliderSFXValueChange() {
		/*Slider[] sliderOptions = optionMenu.GetComponentsInChildren<Slider>(false);
		GameObject soundExample = GameObject.Find("audioTest");

		if(soundExample != null) {
			soundExample.GetComponent<AudioSource>().volume = (0.7f * sliderOptions[0].value) / 100;

			if(!soundExample.GetComponent<AudioSource>().isPlaying) {
				soundExample.GetComponent<AudioSource>().Play();
			}
		}*/
	}

	public void SliderMusicValueChange() {
		Slider[] sliderOptions = optionMenu.GetComponentsInChildren<Slider>(false);
		this.SetBGAudioVolume(sliderOptions[1].value);
	}

	public void SetBGAudioVolume(float x) {
		GameObject bgAudio = GameObject.Find("BackgroundAudio");

		if(bgAudio != null) {
			// Volume 0.3 = 100%
			bgAudio.GetComponent<AudioSource>().volume = ((0.3f * x) / 100);
		}
	}

	public void ResetPressed() {
		quitMenu.enabled = false;
		startText.interactable = false;
		exitText.interactable = false;
		optionText.interactable = false;
		resetMenu.enabled = true;
	}

	public void ResetYes() {
		PlayerPrefs.SetInt(Helper.keyActualLevel, 1);
		PlayerPrefs.Save();

		this.NoPressed();
	}
}
