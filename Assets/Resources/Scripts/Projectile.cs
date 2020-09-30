using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	private bool toRight;
	private bool toLeft;
	private bool toUp;
	private bool toDown;

	public float velocity = 5.0f;

	private Rigidbody2D rigidBody;

	public Sprite[] sprites;

	[HideInInspector]
	public GameObject owner;

	void Start () {
		this.rigidBody = GetComponent<Rigidbody2D>();
	}

	void Update () {
		if(this.toRight) {
			this.rigidBody.velocity = new Vector2(velocity, 0);
		} else if(this.toLeft) {
			this.rigidBody.velocity = new Vector2(-velocity, 0);
		} else if(this.toUp) {
			this.rigidBody.velocity = new Vector2(0, velocity);
		} else if(this.toDown) {
			this.rigidBody.velocity = new Vector2(0, -velocity);
		}

		this.transform.Rotate(0, 0, Random.Range(2, 10));
	}

	public void SetPosition(Vector3 pos) {
		this.transform.position = new Vector3(pos.x, pos.y + 0.25f, pos.z);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.CompareTag("Projectile") || collider.CompareTag("Portal")) {
			return;
		}

		if(collider.CompareTag("Enemy")) {
			if(collider.GetComponent<EnemyBehaviour>().gameObject != this.owner) {
				Destroy(this.gameObject);
				collider.GetComponent<EnemyBehaviour>().Die();
			}

			return;
		}

		Destroy(this.gameObject);

		if(collider.CompareTag("Player")) {
			collider.GetComponent<GlobalSettings>().Die();
		}
	}

	public void OnPortalCollision(PortalBehaviour thisPortal, PortalBehaviour negativePortal, Transform entityTransform, Rigidbody2D entityRigidBody) {
		if(negativePortal.horizontalPortalDown) {
			entityTransform.position = new Vector3(negativePortal.transform.position.x, negativePortal.transform.position.y + thisPortal.paddingVertical, entityTransform.position.z);
			this.ToUp();
		} else if(negativePortal.horizontalPortalUp) {
			entityTransform.position = new Vector3(negativePortal.transform.position.x, negativePortal.transform.position.y - thisPortal.paddingVertical, entityTransform.position.z);
			this.ToDown();
		} else if(negativePortal.verticalPortalLeft) {
			entityTransform.position = new Vector3(negativePortal.transform.position.x + thisPortal.paddingHorizontal, negativePortal.transform.position.y, entityTransform.position.z);
			this.ToRight();
		} else if(negativePortal.verticalPortalRight) {
			entityTransform.position = new Vector3(negativePortal.transform.position.x - thisPortal.paddingHorizontal, negativePortal.transform.position.y, entityTransform.position.z);
			this.ToLeft();
		}
	}

	public void ToRight() {
		this.toRight = true;
		this.toLeft = false;
		this.toUp = false;
		this.toDown = false;
	}

	public void ToLeft() {
		this.toRight = false;
		this.toLeft = true;
		this.toUp = false;
		this.toDown = false;
	}

	public void ToUp() {
		this.toRight = false;
		this.toLeft = false;
		this.toUp = true;
		this.toDown = false;
	}

	public void ToDown() {
		this.toRight = false;
		this.toLeft = false;
		this.toUp = false;
		this.toDown = true;
	}

	public void RandomSprite() {
		this.GetComponent<SpriteRenderer>().sprite = this.sprites[Random.Range(0, 2)];
	}
}
