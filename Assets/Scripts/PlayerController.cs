using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Animator _animator;
	private float _horizontalSpeed = 5f;

	[SerializeField]
	private PlayerIdentity _identity;

	private void Start()
	{
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
		transform.position += Vector3.left * _horizontalSpeed * Time.deltaTime;
	}

	private void HandleInputRight()
	{
		transform.position += Vector3.right * _horizontalSpeed * Time.deltaTime;
	}
}