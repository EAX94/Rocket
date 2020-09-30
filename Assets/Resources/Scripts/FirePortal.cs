using UnityEngine;
using System;

public class FirePortal : MonoBehaviour {
	private float lastShoot;
	public Material diffuseMaterial;
	private uint portalCount;
	private float paddingHorizontal = 2.05f;
	private float paddingVertical = 1.75f;

	public LayerMask whatToHit;
	public float fireRate;
	public GameObject userWithGun;
	public GameObject portal1;
	public GameObject portal2;

	[HideInInspector]
	public bool firePortal = false;

	void Start () {
		this.fireRate = 0.3f;
		this.lastShoot = 0.0f;
		this.portalCount = 0;

		//this.diffuseMaterial = new Material(Shader.Find("Unlit/Texture"));
	}
	
	void Update () {
		if(!this.firePortal) {
			return;
		}

		if(Time.timeScale == 0) {
			return;
		}
		
		if(Input.GetButtonDown("Fire1")) {
			float actualTime = Time.time;

			if(actualTime >= this.lastShoot) {
				this.lastShoot = actualTime + this.fireRate;
				this.FireGun(Color.blue);
			}
		}
	}

	private void FireGun(Color portalColor) {
		Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 userPosition = new Vector2(userWithGun.transform.position.x, userWithGun.transform.position.y);

		RaycastHit2D rayCastHit = Physics2D.Raycast(userPosition, mousePosition - userPosition, 5000, whatToHit);

		if(rayCastHit.collider == null) {
			return;
		}

		//Debug.DrawLine(userPosition, (mousePosition - userPosition) * 5000, portalColor, 0.3f);

		this.DrawPortal(portalColor, rayCastHit, mousePosition, userPosition);
	}

	private bool DrawPortal(Color portalColor, RaycastHit2D rayCastHit, Vector2 mousePosition, Vector2 userPosition) {
		WallBehaviour wallHit = (WallBehaviour)rayCastHit.collider.GetComponent("WallBehaviour");

		this.DrawLineEffect(portalColor, rayCastHit, userPosition);

		if(!wallHit) {
			return false;
		}

		if(!wallHit.portalAvailable) {
			return false;
		}

		bool portalOk = this.CheckSpaceInWall(wallHit.verticalWallLeft || wallHit.verticalWallRight, userPosition, ref rayCastHit, wallHit.wallGuid);

		if(!portalOk) {
			return false;
		}

		++this.portalCount;

		GameObject actualPortal;
		actualPortal = (this.portalCount % 2 == 0) ? portal1 : portal2;

		int portalOrientation;

		if(wallHit.verticalWallLeft || wallHit.verticalWallRight) {
			rayCastHit.point = new Vector2(rayCastHit.point.x - 0.75f, rayCastHit.point.y);

			if(wallHit.verticalWallRight) {
				rayCastHit.point = new Vector2(rayCastHit.point.x + 1.5f, rayCastHit.point.y);
			}

			portalOrientation = 0;
		} else {
			rayCastHit.point = new Vector2(rayCastHit.point.x, rayCastHit.point.y + 0.75f);

			if(wallHit.horizontalWallDown) {
				rayCastHit.point = new Vector2(rayCastHit.point.x, rayCastHit.point.y - 1.5f);
			}

			portalOrientation = 1;
		}

		PortalBehaviour portalScript = (PortalBehaviour)actualPortal.GetComponent("PortalBehaviour");
		portalScript.PortalOn(rayCastHit.point, portalOrientation, wallHit);

		return true;
	}

	private bool CheckSpaceInWall(bool verticalWall, Vector2 userPosition, ref RaycastHit2D rayCastHit, Guid wallGuid) {
		RaycastHit2D rayCastHitCopy = rayCastHit;

		if(verticalWall) {
			rayCastHitCopy.point = new Vector2(rayCastHitCopy.point.x, rayCastHitCopy.point.y + this.paddingVertical);

			if(!this.IsThisGameObject(userPosition, rayCastHitCopy, wallGuid)) {
				rayCastHitCopy.point = new Vector2(rayCastHitCopy.point.x, rayCastHit.collider.bounds.max.y - this.paddingVertical);
				rayCastHit.point = rayCastHitCopy.point;

				return true;
			}

			rayCastHitCopy.point = new Vector2(rayCastHitCopy.point.x, rayCastHitCopy.point.y - this.paddingVertical*2f);
			
			if(!this.IsThisGameObject(userPosition, rayCastHitCopy, wallGuid)) {
				rayCastHitCopy.point = new Vector2(rayCastHitCopy.point.x, rayCastHit.collider.bounds.min.y + this.paddingVertical);
				rayCastHit.point = rayCastHitCopy.point;

				return true;
			}
		} else {
			rayCastHitCopy.point = new Vector2(rayCastHitCopy.point.x + this.paddingHorizontal, rayCastHitCopy.point.y);

			if(!this.IsThisGameObject(userPosition, rayCastHitCopy, wallGuid)) {
				rayCastHitCopy.point = new Vector2(rayCastHit.collider.bounds.max.x - this.paddingHorizontal + 0.5f, rayCastHitCopy.point.y);
				rayCastHit.point = rayCastHitCopy.point;

				return true;
			}

			rayCastHitCopy.point = new Vector2(rayCastHitCopy.point.x - this.paddingHorizontal*2, rayCastHitCopy.point.y);

			if(!this.IsThisGameObject(userPosition, rayCastHitCopy, wallGuid)) {
				rayCastHitCopy.point = new Vector2(rayCastHit.collider.bounds.min.x + this.paddingHorizontal - 0.5f, rayCastHitCopy.point.y);
				rayCastHit.point = rayCastHitCopy.point;

				return true;
			}
		}

		return true;
	}

	private bool IsThisGameObject(Vector2 userPosition, RaycastHit2D thisRayCastHit, Guid wallGuid) {
		RaycastHit2D[] rayCastHits;
		WallBehaviour wallHit;
		rayCastHits = Physics2D.RaycastAll(userPosition, thisRayCastHit.point - userPosition, 5000, whatToHit);

		for(int i = 0; i < rayCastHits.Length; ++i) {
			wallHit = (WallBehaviour)rayCastHits[i].collider.GetComponent("WallBehaviour");

			if(!wallHit) {
				continue;
			}

			if(wallHit.wallGuid == wallGuid) {
				return true;
			}
		}

		return false;
	}

	private void DrawLineEffect(Color portalColor, RaycastHit2D rayCastHit, Vector2 userPosition) {
		LineRenderer lineRender = this.gameObject.AddComponent<LineRenderer>();

		lineRender.material = this.diffuseMaterial;
		lineRender.SetVertexCount(2);
		lineRender.SetColors(portalColor, portalColor);
		lineRender.SetPosition(0, userPosition);
		lineRender.SetPosition(1, rayCastHit.point);
		lineRender.SetWidth(0.03f, 0.03f);

		Destroy(lineRender, 0.29f);
	}
}
