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
	private Vector3 _initialScale;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_initialScale = transform.localScale;

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
		var forceDirectionSqrMagnitude = forceDirection.sqrMagnitude;
		if (forceDirectionSqrMagnitude > 0.01f)
			_rigidbody.AddForce(forceDirection.normalized * _focusStrength, ForceMode.Acceleration);

		if (forceDirectionSqrMagnitude < 12f)
			transform.localScale = new Vector3(_initialScale.x * 1.4f, _initialScale.y, _initialScale.z * 1.4f);
		else
			transform.localScale = _initialScale;
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
