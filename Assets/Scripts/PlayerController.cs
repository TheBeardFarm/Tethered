using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	private Animator _animator;
    private Rigidbody2D _rb2d;
	public float _horizontalSpeed = 5f;
    private Transform groundCheck;

	[SerializeField]
	private PlayerIdentity _identity;

    private bool grounded = false;
    public bool jump = false;

	private void Start()
	{
        _animator = this.GetComponent<Animator>();
        _rb2d = this.GetComponent<Rigidbody2D>();
        _rb2d.drag = 1;
        groundCheck = transform.Find("groundCheck");
	}

	private void Update()
	{
		HandleInput();
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (IsUpButtonDown() && grounded)
        {
            jump = true;
        }
	}

    void FixedUpdate()
    {
        if(jump)
        {
            _rb2d.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            jump = false;
        }
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

	private void HandleInputLeft()
	{
        _rb2d.velocity = new Vector2(-_horizontalSpeed, _rb2d.velocity.y);
	}

	private void HandleInputRight()
	{
        _rb2d.velocity = new Vector2(_horizontalSpeed, _rb2d.velocity.y);
	}

    private void HandleInputNone()
    {
        _rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
    }

}