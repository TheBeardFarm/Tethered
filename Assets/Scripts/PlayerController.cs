using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	private Animator _animator;
	private Rigidbody2D _rb2d;
	private float _horizontalSpeed = 5f;
	private Transform _groundTraceTarget;
	private bool _isStandingOnGround = false;

	[SerializeField]
	private PlayerIdentity _identity;

	public bool CanJump
	{
		get { return _isStandingOnGround; }
	}

	private void Start()
	{
		_animator = this.GetComponent<Animator>();
		_rb2d = this.GetComponent<Rigidbody2D>();
		_rb2d.drag = 1;
		_groundTraceTarget = transform.Find("groundCheck");
	}

	private void Update()
	{
		PrepareUpdate();
		HandleInput();
	}

	private void PrepareUpdate()
	{
		_isStandingOnGround = Physics2D.Linecast(transform.position, _groundTraceTarget.position, 1 << LayerMask.NameToLayer("Ground"));
	}

	private void HandleInput()
	{
		bool leftKey = IsLeftButtonDown();
		bool rightKey = IsRightButtonDown();
		bool upKey = IsUpButtonDown();

		if (upKey && CanJump)
		{
			HandleInputJump();
		}

		if (leftKey == rightKey)
		{
			HandleInputNone();
		}
		else if (leftKey)
		{
			HandleInputLeft();
		}
		else if (rightKey)
		{
			HandleInputRight();
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

	private void HandleInputLeft()
	{
		_animator.SetInteger("Direction", -1);
		_rb2d.velocity = new Vector2(-_horizontalSpeed, _rb2d.velocity.y);
	}

	private void HandleInputRight()
	{
		_animator.SetInteger("Direction", 1);
		_rb2d.velocity = new Vector2(_horizontalSpeed, _rb2d.velocity.y);
	}

	private void HandleInputNone()
	{
		_animator.SetInteger("Direction", 0);
		_rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
	}

	private void HandleInputJump()
	{
		_rb2d.velocity = new Vector2(_rb2d.velocity.x, 0);
		_rb2d.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
	}

	#endregion Input Handlers
}