using UnityEngine;
using System;

public class CameraFade : MonoBehaviour {
	private static CameraFade mInstance = null;
	private static CameraFade instance {
		get {
			if(mInstance == null) {
				mInstance = GameObject.FindObjectOfType(typeof(CameraFade)) as CameraFade;

				if(mInstance == null) {
					mInstance = new GameObject("CameraFade").AddComponent<CameraFade>();
				}
			}

			return mInstance;
		}
	}

	void Awake() {
		if(mInstance == null) {
			mInstance = this as CameraFade;
			instance.init();
		}
	}

	public GUIStyle m_BackgroundStyle = new GUIStyle();
	public Texture2D m_FadeTexture;
	public Color m_CurrentScreenOverlayColor = new Color(0, 0, 0, 0);
	public Color m_TargetScreenOverlayColor = new Color(0, 0, 0, 0);
	public Color m_DeltaColor = new Color(0, 0, 0, 0);
	public int m_FadeGUIDepth = -1000;

	public float m_FadeDelay = 0;
	public Action m_OnFadeFinish = null;
	
	public void init() {
		instance.m_FadeTexture = new Texture2D(1, 1);
		instance.m_BackgroundStyle.normal.background = instance.m_FadeTexture;
	}
	
	void OnGUI() {
		if(Time.time > instance.m_FadeDelay) {
			if(instance.m_CurrentScreenOverlayColor != instance.m_TargetScreenOverlayColor) {
				if(Mathf.Abs(instance.m_CurrentScreenOverlayColor.a - instance.m_TargetScreenOverlayColor.a) < Mathf.Abs(instance.m_DeltaColor.a) * Time.deltaTime) {
					instance.m_CurrentScreenOverlayColor = instance.m_TargetScreenOverlayColor;
					SetScreenOverlayColor(instance.m_CurrentScreenOverlayColor);
					instance.m_DeltaColor = new Color(0, 0, 0, 0);

					if(instance.m_OnFadeFinish != null) {
						instance.m_OnFadeFinish();
					}
					
					Die();
				} else {
					SetScreenOverlayColor(instance.m_CurrentScreenOverlayColor + instance.m_DeltaColor * Time.deltaTime);
				}
			}
		}
		
		if(m_CurrentScreenOverlayColor.a > 0) {
			GUI.depth = instance.m_FadeGUIDepth;
			GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), instance.m_FadeTexture, instance.m_BackgroundStyle);
		}
	}
	
	private static void SetScreenOverlayColor(Color newScreenOverlayColor) {
		instance.m_CurrentScreenOverlayColor = newScreenOverlayColor;
		instance.m_FadeTexture.SetPixel(0, 0, instance.m_CurrentScreenOverlayColor);
		instance.m_FadeTexture.Apply();
	}
	
	public static void StartAlphaFade(Color newScreenOverlayColor, bool isFadeIn, float fadeDuration) {
		if(fadeDuration <= 0.0f) {
			SetScreenOverlayColor(newScreenOverlayColor);
		} else {
			if(isFadeIn) {
				instance.m_TargetScreenOverlayColor = new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0);
				SetScreenOverlayColor(newScreenOverlayColor);
			} else {
				instance.m_TargetScreenOverlayColor = newScreenOverlayColor;
				SetScreenOverlayColor(new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0));
			}

			instance.m_DeltaColor = (instance.m_TargetScreenOverlayColor - instance.m_CurrentScreenOverlayColor) / fadeDuration;
		}
	}
	
	public static void StartAlphaFade(Color newScreenOverlayColor, bool isFadeIn, float fadeDuration, float fadeDelay) {
		if(fadeDuration <= 0.0f) {
			SetScreenOverlayColor(newScreenOverlayColor);
		} else {
			instance.m_FadeDelay = Time.time + fadeDelay;

			if(isFadeIn) {
				instance.m_TargetScreenOverlayColor = new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0);
				SetScreenOverlayColor(newScreenOverlayColor);
			} else {
				instance.m_TargetScreenOverlayColor = newScreenOverlayColor;
				SetScreenOverlayColor(new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0));
			}

			instance.m_DeltaColor = (instance.m_TargetScreenOverlayColor - instance.m_CurrentScreenOverlayColor) / fadeDuration;
		}
	}
	
	public static void StartAlphaFade(Color newScreenOverlayColor, bool isFadeIn, float fadeDuration, float fadeDelay, Action OnFadeFinish) {
		if(fadeDuration <= 0.0f) {
			SetScreenOverlayColor(newScreenOverlayColor);
		} else {
			instance.m_OnFadeFinish = OnFadeFinish;
			instance.m_FadeDelay = Time.time + fadeDelay;

			if(isFadeIn) {
				instance.m_TargetScreenOverlayColor = new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0);
				SetScreenOverlayColor(newScreenOverlayColor);
			} else {
				instance.m_TargetScreenOverlayColor = newScreenOverlayColor;
				SetScreenOverlayColor(new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0));
			}
			
			instance.m_DeltaColor = (instance.m_TargetScreenOverlayColor - instance.m_CurrentScreenOverlayColor) / fadeDuration;
		}
	}

	void Die() {
		mInstance = null;
		Destroy(gameObject);
	}

	void OnApplicationQuit() {
		mInstance = null;
	}
}