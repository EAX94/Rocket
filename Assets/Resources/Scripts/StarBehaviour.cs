using Assets;
using UnityEngine;

public class StarBehaviour : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D collider) {
		Destroy(this.gameObject);
		collider.GetComponent<UserBehaviour>().StarCollected();
	}
}