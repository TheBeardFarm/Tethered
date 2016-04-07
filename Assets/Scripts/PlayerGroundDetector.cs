using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerGroundDetector : MonoBehaviour
{
	private int _collisionCount = 0;

	public bool StandingOnGround { get { return _collisionCount > 0; } }
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		_collisionCount++;
		Debug.Log("Collision with " + other);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		_collisionCount--;
	}
}