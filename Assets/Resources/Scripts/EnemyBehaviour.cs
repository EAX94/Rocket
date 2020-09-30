using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
	public bool toUp = false;
	public bool toDown = false;
	public bool toRight = true;
	public bool toLeft = false;

	public float fireRate = 1.0f;
	public float projectileSpeed = 5;

	private int explosionActualFrame;
	public Sprite[] explosionSprite;

	private SpriteRenderer spriteRender;

	void Start () {
		this.explosionActualFrame = 0;
		this.StartCoroutine(this.FireRate());

		this.spriteRender = this.GetComponent<SpriteRenderer>();
	}

	private IEnumerator FireRate() {
		yield return new WaitForSeconds(this.fireRate);

		GameObject gameObject = Instantiate(Resources.Load("Prefabs/projectile")) as GameObject;
		Projectile projectile = gameObject.GetComponent<Projectile>();

		projectile.owner = this.gameObject;
		projectile.RandomSprite();

		if(this.toRight) {
			projectile.ToRight();
		} else if(this.toLeft) {
			projectile.ToLeft();
		} else if(this.toUp) {
			projectile.ToUp();
		} else if(this.toDown) {
			projectile.ToDown();
		}

		projectile.velocity = this.projectileSpeed;
		projectile.SetPosition(this.transform.position);

		this.StartCoroutine(this.FireRate());
	}

	void OnCollisionEnter2D(Collision2D collider) {
		if(this.gameObject.tag == "Enemy" && collider.gameObject.CompareTag("Player")) {
			collider.gameObject.GetComponent<GlobalSettings>().Die();
		}
	}

	public void OnPortalCollision(PortalBehaviour thisPortal, PortalBehaviour negativePortal, Transform entityTransform, Rigidbody2D entityRigidBody) {
		if(negativePortal.horizontalPortalDown) {
			float yVelocity = Mathf.Abs(entityRigidBody.velocity.y);

			if(yVelocity < 10.0f) {
				yVelocity = 10.0f;
			}

			entityTransform.position = new Vector3(negativePortal.transform.position.x, negativePortal.transform.position.y + thisPortal.paddingVertical, entityTransform.position.z);

			this.ToUp();

			entityRigidBody.velocity = new Vector2(entityRigidBody.velocity.x, yVelocity);
		} else if(negativePortal.horizontalPortalUp) {
			entityTransform.position = new Vector3(negativePortal.transform.position.x, negativePortal.transform.position.y - thisPortal.paddingVertical, entityTransform.position.z);
			this.ToDown();
		} else if(negativePortal.verticalPortalLeft) {
			float yVelocity = entityRigidBody.velocity.y * -1.0f;
			yVelocity = (yVelocity == 0.0f) ? 10.1f : yVelocity;

			entityTransform.position = new Vector3(negativePortal.transform.position.x + thisPortal.paddingHorizontal, negativePortal.transform.position.y, entityTransform.position.z);

			this.ToRight();
			entityRigidBody.velocity = new Vector2(yVelocity, 0.0f);
		} else if(negativePortal.verticalPortalRight) {
			float yVelocity = entityRigidBody.velocity.y;
			yVelocity = (yVelocity == 0.0f) ? -10.1f : yVelocity;

			entityTransform.position = new Vector3(negativePortal.transform.position.x - thisPortal.paddingHorizontal, negativePortal.transform.position.y, entityTransform.position.z);

			this.ToLeft();
			entityRigidBody.velocity = new Vector2(yVelocity, 0.0f);
		}
	}

	public void ToRight() {
		this.toRight = true;
		this.toLeft = false;
		this.toUp = false;
		this.toDown = false;

		this.transform.rotation = Quaternion.Euler(0, -180, 0);
	}

	public void ToLeft() {
		this.toRight = false;
		this.toLeft = true;
		this.toUp = false;
		this.toDown = false;

		this.transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	public void ToUp() {
		this.toRight = false;
		this.toLeft = false;
		this.toUp = true;
		this.toDown = false;

		this.transform.rotation = Quaternion.Euler(0, 0, -90);
	}

	public void ToDown() {
		this.toRight = false;
		this.toLeft = false;
		this.toUp = false;
		this.toDown = true;

		this.transform.rotation = Quaternion.Euler(0, 0, 90);
	}

	public void Die() {
		this.fireRate = 999999.9f;

		StartCoroutine(TimerChangeExplosionFrame());
	}

	private IEnumerator TimerChangeExplosionFrame() {
		yield return new WaitForSeconds(0.06f);

		if(this != null) {
			this.GetComponent<Rigidbody2D>().isKinematic = true;
			this.GetComponent<BoxCollider2D>().enabled = false;
			this.gameObject.tag = "Untagged";
			this.transform.localScale = new Vector3(3.6f, 3.4f, 1f);
			this.spriteRender.sprite = this.explosionSprite[this.explosionActualFrame];

			if(++this.explosionActualFrame < this.explosionSprite.Length) {
				StartCoroutine(TimerChangeExplosionFrame());
			} else {
				Destroy(this.gameObject);
			}
		}
	}
}
