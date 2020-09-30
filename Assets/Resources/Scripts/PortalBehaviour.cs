using UnityEngine;
using System.Collections;
using Assets;

public class PortalBehaviour : MonoBehaviour {
	private bool portalEnabled;
	private SpriteRenderer spriteRender;

	[HideInInspector]
	public bool verticalPortalLeft;
	[HideInInspector]
	public bool verticalPortalRight;
	[HideInInspector]
	public bool horizontalPortalUp;
	[HideInInspector]
	public bool horizontalPortalDown;
	[HideInInspector]
	public float paddingHorizontal = 2.05f;
	[HideInInspector]
	public float paddingVertical = 1.75f;

	private int portalActualFrame;

	public PortalBehaviour negativePortal;
	public Sprite[] portalFrames;

	void Start() {
		this.portalEnabled = false;
		
		this.spriteRender = this.GetComponent<SpriteRenderer>();
		this.spriteRender.color = new Color(this.spriteRender.color.r, this.spriteRender.color.g, this.spriteRender.color.b, 0.0f);

		this.portalActualFrame = 0;

		StartCoroutine(this.TimerChangePortalFrame());
	}

	public void PortalOn(Vector2 portalPosition, int portalOrientation, WallBehaviour wallHit) {
		this.portalEnabled = true;
		//this.spriteRender = this.GetComponent<SpriteRenderer>();
		this.spriteRender.color = new Color(this.spriteRender.color.r, this.spriteRender.color.g, this.spriteRender.color.b, 1.0f);

		this.transform.position = portalPosition;
		this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, 90.0f * portalOrientation);

		this.verticalPortalLeft = wallHit.verticalWallLeft;
		this.verticalPortalRight = wallHit.verticalWallRight;
		this.horizontalPortalUp = wallHit.horizontalWallUp;
		this.horizontalPortalDown = wallHit.horizontalWallDown;

		this.portalActualFrame = 0;
	}

	public Color PortalColor() {
		return this.spriteRender.color;
	}

	public void PortalOff() {
		this.portalEnabled = false;
		this.spriteRender.color = new Color(this.spriteRender.color.r, this.spriteRender.color.g, this.spriteRender.color.b, 0.0f);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		this.PortalBehave(collider);
	}

	void OnTriggerStay2D(Collider2D collider) {
		this.PortalBehave(collider);
	}

	private void PortalBehave(Collider2D collider) {
		if(this.portalEnabled && this.negativePortal.portalEnabled) {
			this.negativePortal.portalEnabled = false;
			this.StartCoroutine(this.TimerEnablePortal());

			Transform entityTransform = collider.GetComponentInParent<Transform>();
			Rigidbody2D entityRigidBody = collider.GetComponentInParent<Rigidbody2D>();

			UserBehaviour userBehaviour = collider.GetComponentInParent<UserBehaviour>();
			EnemyBehaviour enemyBehaviour = collider.GetComponentInParent<EnemyBehaviour>();
			Projectile projectileBehaviour = collider.GetComponentInParent<Projectile>();

			if(userBehaviour) {
				userBehaviour.OnPortalCollision(this, this.negativePortal, entityTransform, entityRigidBody);
			} else if(enemyBehaviour) {
				enemyBehaviour.OnPortalCollision(this, this.negativePortal, entityTransform, entityRigidBody);
			} else if(projectileBehaviour) {
				projectileBehaviour.OnPortalCollision(this, this.negativePortal, entityTransform, entityRigidBody);
			}
		}
	}

	private IEnumerator TimerEnablePortal() {
		yield return new WaitForSeconds(0.1f);

		negativePortal.portalEnabled = true;
	}

	private IEnumerator TimerChangePortalFrame() {
		yield return new WaitForSeconds(0.04f);

		if(this.portalEnabled) {
			this.spriteRender.sprite = this.portalFrames[this.portalActualFrame];

			if(++this.portalActualFrame == this.portalFrames.Length) {
				this.portalActualFrame = 0;
			}
		}

		StartCoroutine(TimerChangePortalFrame());
	}
}
