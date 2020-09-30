using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelCollider : MonoBehaviour {
    private AudioSource audioSource;
	/*public Sprite[] sprites;
	private int portalActualFrame;
	private SpriteRenderer spriteRender;*/

	public void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

		/*this.spriteRender = this.GetComponent<SpriteRenderer>();
		this.portalActualFrame = 0;*/

		StartCoroutine(this.TimerChangePortalFrame());
	}


	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.CompareTag("Player")) {
            audioSource.Play();
			collider.GetComponent<GlobalSettings>().NextLevel();
		}
	}

	private IEnumerator TimerChangePortalFrame() {
		yield return new WaitForSeconds(0.04f);

		this.transform.Rotate(0, 0, 2);

		/*this.spriteRender.sprite = this.sprites[this.portalActualFrame];

		if(++this.portalActualFrame == this.sprites.Length) {
			this.portalActualFrame = 0;
		}*/

		StartCoroutine(TimerChangePortalFrame());
	}
}