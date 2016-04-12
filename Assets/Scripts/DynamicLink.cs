using System;
using UnityEngine;

// [RequireComponent(typeof(BoxCollider2D))]
// [RequireComponent(typeof(DistanceJoint2D))]
public class DynamicLink : MonoBehaviour {

	// object at the "start" of the link
	[SerializeField]
	public Rigidbody2D TargetA
	{
		get { return targetA; }
		set
		{
			targetA = value;
			if (distanceLimiter) {
				distanceLimiter.connectedBody = value;
			}
			LinkTargets();
		}
	}

	// object at the "end" of the link (this will attach to another thing, like a player, or nothing if not set)
	[SerializeField]
	public Rigidbody2D TargetB
	{
		get { return targetB; }
		set
		{
			targetB = value;

			LinkTargets();
		}
	}

	// Anchor point for the connection in object A's local space.
	public Vector2 AnchorPointB
	{
		get { return anchorPointB; }
		set
		{
			anchorPointB = value;
		}
	}

	private Rigidbody2D targetA;
	private Rigidbody2D targetB;
	private DistanceJoint2D distanceLimiter;
	private Collider2D hingeHitbox;
	private Vector2 anchorPointB = Vector2.zero;
	private Vector3 orgionalScale;
	private float size;
	// private float endLinkBuffer = 0.2f;

	// get the needed components
	void Awake () {
		// TODO: look into whether this is needed or not, and if it is better for GetComponent
	}

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.color = Color.red;
		distanceLimiter = GetComponent<DistanceJoint2D>();
		hingeHitbox =  GetComponent<Collider2D>();
		orgionalScale = transform.localScale;
		size = (transform.TransformPoint(Vector3.up) - transform.position).magnitude;
		// now set the anchor point
		targetA = distanceLimiter.connectedBody;
		LinkTargets();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// UpdateLink();
		if(Input.GetKey("down")) {
			LinkTargets();
		}
	}

	// When an object enters the collider, the object will "break" into 2 conneceted by a hinge joint
	void OnTriggerEnter2D(Collider2D other)
	{
		// get the direction of the angular monentum of the hinge joint.
		
		// use a ray from the collision point in the direction opposing the momentum to get the hinge point
		
		// instantiate a new DynamicLink object as a child of this one positioned at the hinge point.
		
		// set the targetA of the new object to this object
		
		// set the new object's targetB to this object's targetB
		
		// set this targetB to the new object.
	}

	private void LinkTargets()
	{
		Vector2 startPoint;
		Vector2 endPoint;
		Vector2 distance;
		Vector2 anchorDirection;
		Vector2 scaleVector;
		float newScale;
		
		// TODO: debug everything.
		// print("link started");
		startPoint = targetB ? targetB.gameObject.transform.TransformPoint(anchorPointB) : transform.TransformPoint(distanceLimiter.anchor);
		// transform.position = (targetB.transform.position + anchorPointB) - distanceLimiter.anchor;
		if (targetA) {
			endPoint = distanceLimiter.connectedBody.gameObject.transform.TransformPoint(distanceLimiter.connectedAnchor);
			distance = (startPoint - endPoint);
			anchorDirection = distanceLimiter.anchor == Vector2.zero ? new Vector2(.5f,0f) : distanceLimiter.anchor.normalized;
			scaleVector = new Vector2(Mathf.Abs(anchorDirection.x), Mathf.Abs(anchorDirection.y));
			// print(scaleVector);
			// TODO: LEFT OFF HERE: might have fixed the following, but check: i think i have to round the size to a reasonable amount... the end scale isn't turning out correctly.
			// TODO: Also fix scale vector... i think i just need logic if its 1, maybe.
			// TODO: at this point we are assuming the object's size in global space 1, this could be improved.
			newScale = distance.magnitude/size;
			// print(newScale);
			transform.localScale = new Vector3((scaleVector.x * newScale)+(1 - scaleVector.x), (scaleVector.y * newScale)+(1 - scaleVector.y), 1);
			// set the position to the midpoint in the distance
			transform.position = Vector3.Lerp(startPoint, endPoint, 0.5f);
			transform.rotation = Quaternion.FromToRotation(anchorDirection, new Vector3(distance.normalized.x, distance.normalized.y, 0));
			// TODO: test and adapt for sizes that would be different from 1.
		}
	}
}

