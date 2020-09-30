using UnityEngine;
using System;

public class WallBehaviour : MonoBehaviour {
	public bool verticalWallLeft;
	public bool verticalWallRight;
	public bool horizontalWallUp;
	public bool horizontalWallDown;
	public bool portalAvailable = true;
	public Guid wallGuid;
	
	void Start () {
		this.wallGuid = Guid.NewGuid();
	}
}
