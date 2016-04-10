using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Tether))]
public class Player : MonoBehaviour
{
	private Animator _animator;
	private Rigidbody2D _rb2d;
	private PlayerGroundDetector _groundTrigger;
	private Tether _tether;
	private GameObject _tetherAnchor;

	[SerializeField]
	private PlayerIdentity _identity;

	[SerializeField, Range(0, float.MaxValue)]
	private float _walkSpeed;

	[SerializeField, Range(0, float.MaxValue)]
	private float _jumpPower;

	public PlayerIdentity Identity { get { return _identity; } }

	public GameObject TetherAnchor { get { return _tetherAnchor ?? (_tetherAnchor = transform.Find("tetherAnchor").gameObject); } }

	public bool CanJump
	{
		get { return _groundTrigger.StandingOnGround; }
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_rb2d = GetComponent<Rigidbody2D>();
		_rb2d.drag = 1;
		_groundTrigger = GetComponentInChildren<PlayerGroundDetector>();
		_tether = GetComponent<Tether>();
	}

	private void Update()
	{
		HandleInput();
		UpdateJumpingAnimator();
	}

	private void UpdateJumpingAnimator()
	{
		if (CanJump && !_animator.GetBool("IsJumpingUp"))
		{
			_animator.SetBool("IsJumpingUp", false);
			_animator.SetBool("IsFallingDown", false);
		} 
		else if (_rb2d.velocity.y < 0.1)
		{
			_animator.SetBool("IsJumpingUp", false);
			_animator.SetBool("IsFallingDown", true);
		}
	}

	private void HandleInput()
	{
		bool leftKey = IsLeftButtonDown();
		bool rightKey = IsRightButtonDown();
		bool upKey = IsUpButtonDown();

		if (upKey && CanJump)
		{
			Jump();
		}
		if (leftKey == rightKey)
		{
			StandStill();
		}
		else if (leftKey)
		{
			MoveLeft();
		}
		else if (rightKey)
		{
			MoveRight();
		}
	}

	#region Button Down Checks

	private bool IsLeftButtonDown()
	{
		switch (_identity)
		{
			case PlayerIdentity.Red:
				return Input.GetKey("left");

			case PlayerIdentity.Blue:
				return Input.GetKey("a");
		}
		throw new NotSupportedException();
	}

	private bool IsRightButtonDown()
	{
		switch (_identity)
		{
			case PlayerIdentity.Red:
				return Input.GetKey("right");

			case PlayerIdentity.Blue:
				return Input.GetKey("d");
		}
		throw new NotSupportedException();
	}

	private bool IsUpButtonDown()
	{
		switch (_identity)
		{
			case PlayerIdentity.Red:
				return Input.GetKey("up");

			case PlayerIdentity.Blue:
				return Input.GetKey("w");
		}
		throw new NotSupportedException();
	}

	#endregion Button Down Checks

	#region Input Handlers

	private void MoveLeft()
	{
		_animator.SetInteger("Direction", -1);
		var maxSpeed = _walkSpeed;
		if (!CanJump)
		{
			maxSpeed *= 0.4f;
		}
		var newVelocity = _rb2d.velocity.x;
		if (newVelocity > -maxSpeed)
		{
			newVelocity = Math.Max(newVelocity - maxSpeed, -maxSpeed);
		}
		_rb2d.velocity = new Vector2(newVelocity, _rb2d.velocity.y);
	}

	private void MoveRight()
	{
		_animator.SetInteger("Direction", 1);
		var maxSpeed = _walkSpeed;
		if (!CanJump)
		{
			maxSpeed *= 0.4f;
		}
		var newVelocity = _rb2d.velocity.x;
		if (newVelocity < maxSpeed)
		{
			newVelocity = Math.Min(newVelocity + maxSpeed, maxSpeed);
		}
		_rb2d.velocity = new Vector2(newVelocity, _rb2d.velocity.y);
	}

	private void StandStill()
	{
		_animator.SetInteger("Direction", 0);
		_rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
	}

	private void Jump()
	{
		_animator.SetBool("IsJumpingUp", true);
		_animator.SetBool("IsFallingDown", false);
		_rb2d.velocity = new Vector2(_rb2d.velocity.x, 0);
		_rb2d.AddForce(new Vector2(0, _jumpPower), ForceMode2D.Impulse);
	}

	#endregion Input Handlers
}