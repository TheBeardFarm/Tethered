using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	private Animator _animator;
	private Rigidbody2D _rb2d;
	private PlayerGroundDetector _groundTrigger;

	[SerializeField]
	private PlayerIdentity _identity;
	[SerializeField]
	private float _walkSpeed = 25f;
	[SerializeField]
	private float _jumpPower = 15f;

	public PlayerIdentity Identity { get { return _identity; } }

	public bool CanJump
	{
		get { return _groundTrigger.StandingOnGround; }
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_rb2d = GetComponent<Rigidbody2D>();
		_rb2d.drag = 1;
		_groundTrigger = transform.GetComponentInChildren<PlayerGroundDetector>();
	}

	private void Update()
	{
		HandleInput();
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
		var newVelocity = _rb2d.velocity.x;
		if (newVelocity > -_walkSpeed)
		{
			newVelocity = Math.Max(newVelocity - _walkSpeed, -_walkSpeed);
		}
		_rb2d.velocity = new Vector2(newVelocity, _rb2d.velocity.y);
	}

	private void MoveRight()
	{
		_animator.SetInteger("Direction", 1);
		var newVelocity = _rb2d.velocity.x;
		if (newVelocity < _walkSpeed)
		{
			newVelocity = Math.Min(newVelocity + _walkSpeed, _walkSpeed);
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
		_rb2d.velocity = new Vector2(_rb2d.velocity.x, 0);
		_rb2d.AddForce(new Vector2(0, _jumpPower), ForceMode2D.Impulse);
	}

	#endregion Input Handlers
}