using UnityEngine;

namespace SandVich.Characters
{
	public class PlayerMovement : MonoBehaviour
	{
		[Header("General")]
		public Animator animator;
		public Rigidbody2D rig;
		public string animatorMoveFloat = "Move";
		public CharacterController2D controller;

		public float runSpeed = 40f;

		float horizontalMove = 0f;
		bool jump = false;
		bool crouch = false;
		[Header("Cheats")]
		public bool noclip = false;
		public float noclipSpeed = 0.1f;

		[HideInInspector]
		float defaultGravityScale;
	
		void Start()
		{
			defaultGravityScale = rig.gravityScale;
		}

		void Update()
		{

			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

			if (Input.GetButtonDown("Jump"))
			{
				jump = true;
			}

			if (Input.GetButtonDown("Crouch"))
			{
			crouch = true;
			} else if (Input.GetButtonUp("Crouch"))
			{
				crouch = false;
			}
			if (noclip)
			{
				rig.gravityScale = 0f;
				rig.simulated = false;
				if (Input.GetKey(KeyCode.W))
				{
					this.transform.Translate(this.transform.up * noclipSpeed);
				}
				if (Input.GetKey(KeyCode.S))
				{
					this.transform.Translate(-this.transform.up * noclipSpeed);
				}
				if (Input.GetKey(KeyCode.A))
				{
					this.transform.Translate(-this.transform.right * noclipSpeed);
				}
				if (Input.GetKey(KeyCode.D))
				{
					this.transform.Translate(this.transform.right * noclipSpeed);
				}
				if (Input.GetKeyDown(KeyCode.LeftShift))
				{
					noclipSpeed = 1.0f;
				}
				else if (Input.GetKeyUp(KeyCode.LeftShift))
				{
					noclipSpeed = 0.1f;
				}
			} else
			{
				rig.gravityScale = defaultGravityScale;
				rig.simulated = true;
			}
		}

		void FixedUpdate()
		{
			// Move our character
			controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
			animator.SetFloat(animatorMoveFloat, 0.3f * horizontalMove);
			jump = false;
		}
	}
}
