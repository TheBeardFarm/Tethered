using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour
{
	public float transitionSpeed;

	void Update()
	{
		Vector3 redPlayerPos = GameObject.Find("RedPlayer").transform.position;
		Vector3 bluePlayerPos = GameObject.Find("BluePlayer").transform.position;
		//Calculate midpoint between players
		Vector3 midpoint = ((redPlayerPos - bluePlayerPos) * 0.5f) + bluePlayerPos;

		if (Vector3.Distance(redPlayerPos, bluePlayerPos) >= 10f)
		{
			Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, 10, transitionSpeed);
		}
		else if (Vector3.Distance(redPlayerPos, bluePlayerPos) < 10f)
		{
			Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, 5, transitionSpeed);
		}

		transform.position = new Vector3(midpoint.x, midpoint.y, transform.position.z);
	}
}
