using UnityEngine;

namespace Assets {
	class UserBehaviour : MonoBehaviour {
        public AudioSource audioJump;
		private float m_MaxSpeed = 7f;
		private float m_JumpForce = 1200f;
		private Transform m_GroundCheckLeft;
		private Transform m_GroundCheckRight;
		//const float k_GroundedRadius = .2f;
		private bool m_Grounded;
		private Animator m_Anim;
		private Rigidbody2D m_Rigidbody2D;
		private bool m_FacingRight = true;
		private int maxVelocityN = -50;
		private int maxVelocity = 50;
		private bool m_Jump;
		public LayerMask layerMask;

		private int star = 0;

		private void Awake() {
			m_GroundCheckLeft = transform.Find("GroundCheckLeft");
			m_GroundCheckRight = transform.Find("GroundCheckRight");
			m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
            audioJump = GetComponent<AudioSource>();
		}

		private void Update() {
			if(!m_Jump) {
				m_Jump = Input.GetButtonDown("Jump");
			}
		}

		private void FixedUpdate() {
			if(m_Rigidbody2D.velocity.y < maxVelocityN) {
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, maxVelocityN);
			} else if(m_Rigidbody2D.velocity.y > maxVelocity) {
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, maxVelocity);
			}

			if(m_Rigidbody2D.velocity.x < maxVelocityN) {
				m_Rigidbody2D.velocity = new Vector2(maxVelocityN, m_Rigidbody2D.velocity.y);
			} else if(m_Rigidbody2D.velocity.x > maxVelocity) {
				m_Rigidbody2D.velocity = new Vector2(maxVelocity, m_Rigidbody2D.velocity.y);
			}

			m_Grounded = false;

			/*if(m_Rigidbody2D.velocity.y < -1f || m_Rigidbody2D.velocity.y > 1f) {
				m_Grounded = false;
			}*/

			bool checkBoth = true;

			Collider2D[] colliders = Physics2D.OverlapPointAll(new Vector2(m_GroundCheckLeft.position.x, m_GroundCheckLeft.position.y-0.2f), layerMask);
			for(int i = 0; i < colliders.Length; i++) {
				if(colliders[i].gameObject != gameObject)
					checkBoth = false;
			}

			if(checkBoth) {
				colliders = Physics2D.OverlapPointAll(new Vector2(m_GroundCheckRight.position.x, m_GroundCheckRight.position.y - 0.2f), layerMask);
				for(int i = 0; i < colliders.Length; i++) {
					if(colliders[i].gameObject != gameObject)
						checkBoth = false;
				}
			}

			if(!checkBoth) {
				m_Grounded = true;
			}

			/*Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, layerMask);
			for(int i = 0; i < colliders.Length; i++) {
				if(colliders[i].gameObject != gameObject)
					m_Grounded = true;
			}*/

			m_Anim.SetBool("Ground", m_Grounded);
			m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

			float h = Input.GetAxis("Horizontal");

			this.Move(h, m_Jump);
			m_Jump = false;
		}


		public void Move(float move, bool jump) {
			m_Anim.SetFloat("Speed", Mathf.Abs(move));

			float xVelocity = Mathf.Abs(m_Rigidbody2D.velocity.x);

			if(xVelocity <= m_MaxSpeed) {
				m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
			} else {
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y);
			}
			
			if(move > 0 && !m_FacingRight) {
				Flip();
			} else if(move < 0 && m_FacingRight) {
				Flip();
			}

			if(m_Grounded && jump && m_Anim.GetBool("Ground")) {
				m_Grounded = false;
				m_Anim.SetBool("Ground", false);
                audioJump.Play();
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			}
		}


		private void Flip() {
			m_FacingRight = !m_FacingRight;

			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		public void FlipToLeft() {
			if(!m_FacingRight) {
				return;
			}

			this.Flip();
		}

		public void FlipToRight() {
			if(m_FacingRight) {
				return;
			}

			this.Flip();
		}

		public void StarCollected() {
			++this.star;
		}

		public void OnPortalCollision(PortalBehaviour thisPortal, PortalBehaviour negativePortal, Transform entityTransform, Rigidbody2D entityRigidBody) {
			if(negativePortal.horizontalPortalDown) {
				float yVelocity = Mathf.Abs(entityRigidBody.velocity.y);

				if(yVelocity < 7.0f) {
					yVelocity = 7.0f;
				}

				entityTransform.position = new Vector3(negativePortal.transform.position.x, negativePortal.transform.position.y + thisPortal.paddingVertical, entityTransform.position.z);
				entityRigidBody.velocity = new Vector2(entityRigidBody.velocity.x, yVelocity);
			} else if(negativePortal.horizontalPortalUp) {
				entityTransform.position = new Vector3(negativePortal.transform.position.x, negativePortal.transform.position.y - thisPortal.paddingVertical, entityTransform.position.z);
			} else if(negativePortal.verticalPortalLeft) {
				float yVelocity = entityRigidBody.velocity.y * -1.0f;
				yVelocity = (yVelocity == 0.0f) ? 7.1f : yVelocity;

				entityTransform.position = new Vector3(negativePortal.transform.position.x + thisPortal.paddingHorizontal, negativePortal.transform.position.y, entityTransform.position.z);

				this.FlipToRight();
				entityRigidBody.velocity = new Vector2(yVelocity, 0.0f);
			} else if(negativePortal.verticalPortalRight) {
				float yVelocity = entityRigidBody.velocity.y;
				yVelocity = (yVelocity == 0.0f) ? -7.1f : yVelocity;

				entityTransform.position = new Vector3(negativePortal.transform.position.x - thisPortal.paddingHorizontal, negativePortal.transform.position.y, entityTransform.position.z);

				this.FlipToLeft();
				entityRigidBody.velocity = new Vector2(yVelocity, 0.0f);
			}
		}
	}
}