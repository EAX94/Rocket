using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class Button_Actions : MonoBehaviour {
	bool firstTime = false;

	public void Start() {
		this.firstTime = false;
	}

	public void Play_Button() {
		if(!this.firstTime) {
			this.firstTime = true;

			CameraFade.StartAlphaFade(Color.black, false, 2f, 0f, () => {
				SceneManager.LoadScene((int)GlobalSettings.SceneLevels.Level_Selection);
			});
		}
	}

	public void Quit_Button() {
		Process.GetCurrentProcess().Kill();
		//Application.Quit();
	}
}
