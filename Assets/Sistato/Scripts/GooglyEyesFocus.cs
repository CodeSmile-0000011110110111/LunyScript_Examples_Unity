using Luny.Unity.Engine.Bridge;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public sealed class GooglyEyesFocus : MonoBehaviour
{
	[SerializeField] private String TargetObjectName = "Player";
	[SerializeField] private Single _focusStrength = 5f; // How hard they pull toward the player
	[SerializeField] private Single _focusStrengthRandomRange = 1f;

	private Rigidbody _rigidbody;
	private Transform _lookAtTarget;

	private void Awake()
	{
		TestThrow.Throw(gameObject);

		_rigidbody = GetComponent<Rigidbody>();

		var player = GameObject.FindWithTag(TargetObjectName);
		if (player == null)
			player = GameObject.Find(TargetObjectName);

		if (player != null)
			_lookAtTarget = player.transform;

		_focusStrength += Random.Range(-_focusStrengthRandomRange, _focusStrengthRandomRange);
	}

	private void FixedUpdate()
	{
		if (_lookAtTarget == null)
			return;

		var directionToTarget = _lookAtTarget.position - transform.position;
		var forceDirection = new Vector3(directionToTarget.x, 0f, directionToTarget.z);
		if (forceDirection.sqrMagnitude > 0.01f)
			_rigidbody.AddForce(forceDirection.normalized * _focusStrength, ForceMode.Acceleration);
	}

	private void OnDisable()
	{
		var joints = GetComponents<Joint>();
		foreach (var joint in joints)
			Destroy(joint);

		_rigidbody.isKinematic = true;

		var rotation = transform.localRotation;
		var scale = transform.localScale;
		scale.x = 0.7f;
		scale.z = 0.22f;
		rotation = Quaternion.Euler(0f, Random.Range(0f, 360f) - 180f, 0f);
		transform.localScale = scale;
		transform.localRotation = rotation;

		Destroy(this);
	}
}
