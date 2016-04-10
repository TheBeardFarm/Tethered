using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerGroundDetector : MonoBehaviour
{
	private int _collisionCount = 0;
	private bool _outOfBounds = false;

	public bool StandingOnGround { get { return _collisionCount > 0; } }
	public bool playerOutOfBounds { get { return _outOfBounds; } set { _outOfBounds = value; } }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "WorldBounds")
		{
			_outOfBounds = true;
		}
		else
		{
			_collisionCount++;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag != "WorldBounds")
		{
			_collisionCount--;
		}
	}
}