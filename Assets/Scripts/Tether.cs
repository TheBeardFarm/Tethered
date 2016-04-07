using System;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Tether : MonoBehaviour
{
	private bool _localReady = false;
	private bool _otherReady = false;
	private Player _localPlayer;
	private Player _otherPlayer;

	[SerializeField]
	private GameObject _tetherTarget;

	[SerializeField, Range(0, 1000)]
	private int _chainLength;

	[SerializeField]
	private GameObject _chainLinkPrefab;

	private void Start()
	{
		_localPlayer = GetComponent<Player>();
		_otherPlayer = _tetherTarget.GetComponent<Player>();
		CreateChain();
	}

	private void CreateChain()
	{
		GameObject previous = _localPlayer.TetherAnchor;
		for (int i = 0; i < _chainLength; i++)
		{
			GameObject current = Instantiate(_chainLinkPrefab);
			Link(previous, current);
			previous = current;
		}
		LinkEnd(previous);
	}

	private HingeJoint2D Link(GameObject start, GameObject end)
	{
		HingeJoint2D hinge = start.GetComponent<HingeJoint2D>();
		hinge.connectedBody = end.GetComponent<Rigidbody2D>();
		return hinge;
	}

	private void LinkEnd(GameObject last)
	{
		var hinge = Link(last, _otherPlayer.TetherAnchor);
		hinge.autoConfigureConnectedAnchor = false;
		hinge.anchor = Vector2.zero;
		hinge.connectedAnchor = Vector2.zero;
	}

	private void Update()
	{
	}

	internal void MarkReady(Player player)
	{
		if (player == _localPlayer)
		{

		}
	}

	public Player GetOpposite(Player player)
	{
		if (player == _otherPlayer)
		{
			return _localPlayer;
		}
		else
		{
			return _otherPlayer;
		}
	}
}