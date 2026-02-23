using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class RigidbodyStayUpright : MonoBehaviour
{
	[SerializeField] [Range(0f, 30f)] [Tooltip("How hard it pulls back up.")] private Single _pullUpStrength = 1f;
	[SerializeField] [Range(0f, 30f)] [Tooltip("How much it resists 'wobbling' when almost upright.")] private Single _uprightSteadiness = 1f;
	[SerializeField] [Range(0.1f, 10f)] [Tooltip("Force limiter, in thousands torque vector magnitude")] private Single _maxTorque = 20f;

	private Rigidbody _rigidbody;

	private void Awake() => _rigidbody = GetComponent<Rigidbody>();

	private void FixedUpdate()
	{
		var selfRighting = Quaternion.FromToRotation(transform.up, Vector3.up);
		selfRighting.ToAngleAxis(out var angle, out var axis);

		// Apply the corrective torque (P) minus the current velocity (D)
		// We use ForceMode.Acceleration to ignore mass for easier tuning
		var maxTorque = _maxTorque * 1000f;
		var uprightTorque = axis * (angle * _pullUpStrength) - _rigidbody.angularVelocity * _uprightSteadiness;
		if (uprightTorque.sqrMagnitude > maxTorque * maxTorque)
		{
			Debug.Log($"Clamping {uprightTorque.magnitude} to {maxTorque}");
			uprightTorque = uprightTorque.normalized * maxTorque;
		}

		_rigidbody.AddTorque(uprightTorque, ForceMode.Acceleration);
	}
}
