using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Animator _animator;
	private float _horizontalSpeed = 5f;

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
		return Input.GetKey("left");
	}

	private bool IsRightButtonDown()
	{
		return Input.GetKey("right");
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