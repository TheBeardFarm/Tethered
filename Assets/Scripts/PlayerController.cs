using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	private Animator _animator;
	private Rigidbody2D _rb2d;
	private float _horizontalSpeed = 5f;

	[SerializeField]
	private PlayerIdentity _identity;

	private void Start()
	{
		_rb2d = GetComponent<Rigidbody2D>();
		_rb2d.drag = 1;
	}

	private void Update()
	{
		HandleInput();
	}

	private void HandleInput()
	{
		bool leftKey = IsLeftButtonDown();
		bool rightKey = IsRightButtonDown();

		if (leftKey)
		{
			HandleInputLeft();
		}
		if (rightKey)
		{
			HandleInputRight();
		}
		if (!leftKey && !rightKey)
		{
			HandleInputNone();
		}
	}

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

	private void HandleInputLeft()
	{
		_rb2d.velocity = new Vector2(_horizontalSpeed, _rb2d.velocity.y);
		//transform.position += Vector3.left * _horizontalSpeed * Time.deltaTime;
	}

	private void HandleInputRight()
	{
		_rb2d.velocity = new Vector2(-_horizontalSpeed, _rb2d.velocity.y);
		//transform.position += Vector3.right * _horizontalSpeed * Time.deltaTime;

	private void HandleInputNone()
	{
		_rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
	}
}