using System.Collections;
using UnityEngine;

public class ElevatorBehaviour : MonoBehaviour {
	public float minUp;
	public float maxUp;
	public float minRight;
	public float maxRight;
	public float velocity;
	public float waitWhenFinish;

	public bool toUp;

	private bool waitElevator = false;
	private bool upOrRight = true;

	public GameObject user;
	public LayerMask whatToHit;
	public GameObject enemy;

	void Start () {
		this.minUp += this.transform.position.y;
		this.maxUp += this.transform.position.y;
		this.minRight += this.transform.position.x;
		this.maxRight += this.transform.position.x;
	}

	void Update () {
		if(this.waitElevator) {
			return;
		}

		if(this.toUp) {
			if(this.upOrRight) {
				if(this.transform.position.y < this.maxUp) {
					this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (velocity * Time.deltaTime), this.transform.position.z);
				} else {
					this.upOrRight = !this.upOrRight;
					this.StartCoroutine(this.TimerWaitToStartAgain());
				}
			} else {
				if(this.transform.position.y > this.minUp) {
					this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - (velocity * Time.deltaTime), this.transform.position.z);
				} else {
					this.upOrRight = !this.upOrRight;
					this.StartCoroutine(this.TimerWaitToStartAgain());
				}
			}

			if(this.user != null) {
				RaycastHit2D rayCastHit;
				rayCastHit = Physics2D.Raycast(this.user.transform.position, Vector3.down, 1.0f, whatToHit);
				if(rayCastHit && rayCastHit.collider.CompareTag("ElevatorUpDown")) {
					if(!this.upOrRight) {
						this.user.transform.position = new Vector3(this.user.transform.position.x, this.user.transform.position.y - (velocity * Time.deltaTime), this.user.transform.position.z);
					}
				}
			}

			if(this.enemy != null) {
				if(!this.upOrRight) {
					this.enemy.transform.position = new Vector3(this.enemy.transform.position.x, this.enemy.transform.position.y - (velocity * Time.deltaTime), this.enemy.transform.position.z);
				}
			}
		} else {
			if(this.upOrRight) {
				if(this.transform.position.x < this.maxRight) {
					this.transform.position = new Vector3(this.transform.position.x + (velocity * Time.deltaTime), this.transform.position.y, this.transform.position.z);
				} else {
					this.upOrRight = !this.upOrRight;
					this.StartCoroutine(this.TimerWaitToStartAgain());
				}
			} else {
				if(this.transform.position.x > this.minRight) {
					this.transform.position = new Vector3(this.transform.position.x - (velocity * Time.deltaTime), this.transform.position.y, this.transform.position.z);
				} else {
					this.upOrRight = !this.upOrRight;
					this.StartCoroutine(this.TimerWaitToStartAgain());
				}
			}

			if(this.user != null) {
				RaycastHit2D rayCastHit = Physics2D.Raycast(this.user.transform.position, Vector3.down, 1.0f, whatToHit);
				if(rayCastHit && rayCastHit.collider.CompareTag("Elevator")) {
					if(this.upOrRight) {
						this.user.transform.position = new Vector3(this.user.transform.position.x + (velocity * Time.deltaTime), this.user.transform.position.y, this.user.transform.position.z);
					} else {
						this.user.transform.position = new Vector3(this.user.transform.position.x - (velocity * Time.deltaTime), this.user.transform.position.y, this.user.transform.position.z);
					}
				}
			}
		}
	}

	private IEnumerator TimerWaitToStartAgain() {
		this.waitElevator = true;

		yield return new WaitForSeconds(this.waitWhenFinish);

		this.waitElevator = false;
	}
}
