using UnityEngine;
using System.Collections;

public class ConfigOptions : MonoBehaviour {
	public static int sfxVolume = 100;
	public static int musicVolume = 100;
	
	public static void SaveSFX(int x) {
		PlayerPrefs.SetInt(Helper.keySFX, x);
		Save();
	}

	public static void SaveMusic(int x) {
		PlayerPrefs.SetInt(Helper.keyMusic, x);
		Save();
	}

	public static int LoadSFX() {
		return PlayerPrefs.GetInt(Helper.keySFX, 100);
	}

	public static int LoadMusic() {
		return PlayerPrefs.GetInt(Helper.keyMusic, 100);
	}

	public void Awake() {
		LoadConfig();
	}

	public void Start() {
		GameObject[] bgAudio = GameObject.FindGameObjectsWithTag("BackgroundAudio");

		if(bgAudio.Length > 1) {
			Destroy(bgAudio[0]);
		}
	}

	public static void LoadConfig() {
		sfxVolume = PlayerPrefs.GetInt(Helper.keySFX, 100);
		musicVolume = PlayerPrefs.GetInt(Helper.keyMusic, 100);

		GameObject bgAudio = GameObject.Find("BackgroundAudio");

		if(bgAudio != null) {
			// Volume 0.3 = 100%
			bgAudio.GetComponent<AudioSource>().volume = ((0.3f * musicVolume) / 100);
		}
	}

	public static void Save() {
		PlayerPrefs.Save();
	}
}
