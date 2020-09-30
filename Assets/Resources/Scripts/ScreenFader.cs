using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Threading;

public class ScreenFader : MonoBehaviour {
	void Start() {
		CameraFade.StartAlphaFade(Color.black, false, 2f, 2f, () => {
            SceneManager.LoadScene((int)GlobalSettings.SceneLevels.Menu);
        });
	}
}