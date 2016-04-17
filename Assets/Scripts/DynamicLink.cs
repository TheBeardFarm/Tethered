using System;
using UnityEngine;

// TODO: figure out the proper components needed
// [RequireComponent(typeof(BoxCollider2D))]
// [RequireComponent(typeof(DistanceJoint2D))]
public class DynamicLink : MonoBehaviour {

	// object at the "start" of the link
	public Rigidbody2D TargetStart
	{
		get { return targetStart; }
		set
		{
			targetStart = value;
			if (distanceLimiter) {
				distanceLimiter.connectedBody = value;
			}
			if (connectorStart) {
				connectorStart.connectedBody = value;
			}
		}
	}

	// object at the "end" of the link (this will attach to another thing, like a player, or nothing if not set)
	public Rigidbody2D TargetEnd
	{
		get { return targetEnd; }
		set
		{
			targetEnd = value;
			if (connectorEnd) {
				connectorEnd.connectedBody = value;
			}
		}
	}
	// Anchor point for the connection in the starting object's local space.
	public Vector2 AnchorPointStart
	{
		get { return connectorStart ? connectorStart.connectedAnchor : Vector2.zero; }
		set
		{
			if (distanceLimiter) {
				distanceLimiter.connectedAnchor = value;
			}
			if (connectorStart) {
				connectorStart.connectedAnchor = value;
			}
		}
	}
	// Anchor point for the connection in the ending object's local space.
	public Vector2 AnchorPointEnd
	{
		get { return connectorEnd ? connectorEnd.connectedAnchor : distanceLimiter ? distanceLimiter.anchor : Vector2.zero; }
		set
		{
			if (connectorEnd) {
				connectorEnd.connectedAnchor = value;
			}
		}
	}
	// should reflect this rigid body's 
	public bool IsKinematic
	{
		get { return isKinematic; }
		set
		{
			if (thisRigidbody && thisRigidbody.isKinematic != value) {
				thisRigidbody.isKinematic = value;
			}
			// disable the connector component if its kinematic, as it messes with things
			if (value == true && connectorStart && connectorStart != false) {
				connectorStart.enabled = false;
			} else if (connectorStart.enabled != true) {
				connectorStart.enabled = true;
			}
			isKinematic = value;
		}
	}

	private Rigidbody2D thisRigidbody;
	private Rigidbody2D targetStart;
	private Rigidbody2D targetEnd;
	private DistanceJoint2D distanceLimiter;
	private DistanceJoint2D connectorStart;
	private DistanceJoint2D connectorEnd;
	private Collider2D hingeHitbox;
	private bool isKinematic = false;
	private float size;
	// private float endLinkBuffer = 0.2f;

	// get the needed components
	void Awake () {
		// TODO: look into whether this is needed or not, and if it is better for GetComponent
	}

	void Start () {
		// get all the necessary components
		DistanceJoint2D[] connectors = GetComponents<DistanceJoint2D>();
		distanceLimiter = connectors[0];
		connectorStart = connectors[1];
		connectorEnd = connectors[2];
		hingeHitbox = GetComponent<Collider2D>();
		thisRigidbody = GetComponent<Rigidbody2D>();
		IsKinematic = thisRigidbody.isKinematic;
		// set the "size" of the object based on its anchor points. default full size is 1.
		size = sizeFromAnchors();
		// set the anchor points for the "start" of the link
		targetStart = distanceLimiter.connectedBody;
		connectorStart.connectedBody = distanceLimiter.connectedBody;
		// set the anchor points for the "end" of the link
		targetEnd = connectorEnd.connectedBody;
		// last but not least, color it red.
		GetComponent<Renderer>().material.color = Color.red;
		LinkTargets();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!Input.GetKey(KeyCode.Space)) {
			UpdateLink();
		}
	}

	// When an object enters the collider, the object will "break" into 2 conneceted by a hinge joint
	void OnTriggerEnter2D(Collider2D other)
	{
		// get the direction of the angular monentum of the hinge joint.
		
		// use a ray from the collision point in the direction opposing the momentum to get the hinge point
		
		// instantiate a new DynamicLink object as a child of this one positioned at the hinge point.
		
		// set the targetStart of the new object to this object
		
		// set the new object's targetEnd to this object's targetEnd
		
		// set this targetEnd to the new object.
	}

	private float sizeFromAnchors()
	{
		// default to a size of 1;
		if (distanceLimiter.anchor == Vector2.zero && connectorStart.anchor == Vector2.zero) {
			return 1f;
		}
		return (distanceLimiter.anchor - connectorStart.anchor).magnitude;
	}

	private Vector2 directionFromAnchors()
	{
		// default to up;
		if (distanceLimiter.anchor == Vector2.zero && connectorStart.anchor == Vector2.zero) {
			return Vector2.up;
		}
		return (distanceLimiter.anchor - connectorStart.anchor).normalized;
	}

	private void LinkTargets()
	{
		Vector2 startPoint;
		Vector2 endPoint;
		Vector2 distance;
		Vector2 anchorDirection;
		Vector2 scaleVector;
		float newScale;
		
		// TODO: debug everything ever.
		if (targetStart) {
			startPoint = distanceLimiter.connectedBody.gameObject.transform.TransformPoint(AnchorPointStart);
			endPoint = targetEnd ? targetEnd.gameObject.transform.TransformPoint(AnchorPointEnd) : transform.TransformPoint(distanceLimiter.anchor);
			distance = (startPoint - endPoint);
			anchorDirection = directionFromAnchors();
			scaleVector = new Vector2(Mathf.Abs(anchorDirection.x), Mathf.Abs(anchorDirection.y));
			newScale = distance.magnitude/size;
			// TODO: incorperate a "width" or "thinkness" setting and modify scale based on this as well.
			transform.localScale = new Vector3((scaleVector.x * newScale)+(1 - scaleVector.x), (scaleVector.y * newScale)+(1 - scaleVector.y), 1);
			// in kinematic mode set the position to the midpoint in the distance manually
			if (isKinematic) {
				transform.position = Vector3.Lerp(startPoint, endPoint, 0.5f);
				transform.rotation = Quaternion.FromToRotation(-anchorDirection, new Vector3(distance.normalized.x, distance.normalized.y, 0));
			}
			// TODO: test and adapt for sizes that would be different from 1.
		}
	}

	private void UpdateLink()
	{
		// Check and Update variables used to connect the two links
		// TODO: look into a better way to keep the kinematic value sysnced with the rigidbody component's value... pointers maybe? some kind of event when its changed, so I can disable components?
		if (thisRigidbody && thisRigidbody.isKinematic != isKinematic) {
			IsKinematic = thisRigidbody.isKinematic;
		}
		LinkTargets();
	}
}

