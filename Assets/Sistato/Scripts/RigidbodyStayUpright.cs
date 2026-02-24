using Luny;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class RigidbodyStayUpright : MonoBehaviour
{
	// to clamp insane (explosive) torque force
	private const Single MaxUprightTorque = 1000f;
	private const Single MaxUprightTorqueSquared = MaxUprightTorque * MaxUprightTorque;

	[Tooltip("How hard it pulls back up.")]
	[SerializeField] [Range(0f, 30f)] private Single _pullUpStrength = 1f;

	[Tooltip("How much it resists 'wobbling' when almost upright.")]
	[SerializeField] [Range(0f, 30f)] private Single _uprightSteadiness = 1f;

	[Tooltip("Ignoring mass makes tuning behaviour easier as a body's mass won't affect its 'uprighteousness' behaviour.")]
	[SerializeField] private Boolean _ignoreMass = true;

	[Header("Ground Check")]
	[Tooltip("When checked, stay upgright torque is only applied while in contact with a ground layer.")]
	[SerializeField]
	private Boolean _uprightOnlyWhenGrounded = true; // BUTTERS!

	[Tooltip("Which layers will be considered 'ground'.")]
	[SerializeField] private LayerMask _groundLayer = ~(1 << 2 | 1 << 4 | 1 << 5); // excluded layers: Ignore Raycast, Water, UI

	[Tooltip("Slopes steeper than this are not considered as 'ground'.")]
	[SerializeField] private Single _maxAngleConsideredGroundable = 35f;

	[Tooltip("When hitting ground after being in air: determines how long for upright torque to reach its full strength. " +
	         "This prevents the body from suddenly 'flipping' (and going airborne again) when landing on ground.")]
	[SerializeField] private Single _touchedGroundUprightDampDuration = 3f;

	private Rigidbody _rigidbody;
	private Single _groundCheckRadius;
	private Single _groundCastMaxDistance;
	private Single _currentUprightDampFactor;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		InferGroundCheckRadiusFromCollider();
	}

	private void FixedUpdate()
	{
		var isGrounded = IsGrounded();
		UpdateUprightDampFactor(isGrounded);

		if (!_uprightOnlyWhenGrounded || isGrounded)
			MakeBodyUprightAgain(); // MBUA! :)
	}

	private void OnDrawGizmos()
	{
		var origin = GetGroundCastOrigin();
		var distance = GetGroundCastDistance();
		var isHit = GroundCast(origin, distance, out var hit);
		var isGroundableAngle = isHit && IsGroundableAngle(hit);

		// Set color: Green = Grounded, Yellow = Touching something steep, Red = Air
		if (isGroundableAngle)
			Gizmos.color = Color.green;
		else if (isHit)
			Gizmos.color = Color.yellow;
		else
			Gizmos.color = Color.red;

		Gizmos.DrawWireSphere(origin, _groundCheckRadius);

		// If we hit something, draw the line only to the hit point
		var targetPos = origin + Vector3.down * distance;
		var lineEnd = isHit ? hit.point + Vector3.up * _groundCheckRadius : targetPos;
		Gizmos.DrawLine(origin, lineEnd);
		Gizmos.DrawLine(origin, targetPos);
		//Gizmos.DrawRay(origin, targetPos);

		// Draw the End Sphere
		Gizmos.DrawSphere(lineEnd, _groundCheckRadius);
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(lineEnd, _groundCheckRadius);

		// Optional: Draw the Normal of the surface we hit
		if (isHit)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawRay(hit.point, hit.normal);
		}
	}

	private void UpdateUprightDampFactor(Boolean isGrounded)
	{
		if (!isGrounded)
			_currentUprightDampFactor = 0.001f; // Avoid division by zero

		// Smoothly increase the factor toward 1
		var speed = Time.fixedDeltaTime * (1f / _touchedGroundUprightDampDuration);
		_currentUprightDampFactor = Mathf.MoveTowards(_currentUprightDampFactor, 1f, speed);

		if (_currentUprightDampFactor < 1f)
			LunyLogger.LogInfo(_currentUprightDampFactor);
	}

	private void InferGroundCheckRadiusFromCollider()
	{
		var col = GetComponent<Collider>();
		if (col is SphereCollider sphere)
		{
			_groundCheckRadius = sphere.radius;
			_groundCastMaxDistance = _groundCheckRadius + 0.1f;
		}
		else
		{
			_groundCheckRadius = (col.bounds.extents.x + col.bounds.extents.z) / 2f;
			_groundCastMaxDistance = _groundCheckRadius;

			if (col is CapsuleCollider capsule)
				_groundCastMaxDistance = capsule.height / 2f + 0.1f;
		}

		// Optional: Shrink it slightly (e.g., 90%) to prevent it from
		// catching walls that are perfectly flush with the sides.
		_groundCheckRadius *= 0.9f;

		//LunyLogger.LogWarning($"Ground check radius: {_groundCheckRadius}, max distance: {_groundCastMaxDistance}", this);
	}

	private void MakeBodyUprightAgain()
	{
		var selfRightingRotation = Quaternion.FromToRotation(transform.up, Vector3.up);
		selfRightingRotation.ToAngleAxis(out var toUprightAngle, out var uprightAxis);

		// Apply the corrective torque (P) minus the current velocity (D)
		var toUprightTorque = uprightAxis * (toUprightAngle * _pullUpStrength);
		var scaledAngularVelocity = _rigidbody.angularVelocity * _uprightSteadiness;
		var uprightTorque = toUprightTorque - scaledAngularVelocity;
		if (uprightTorque.sqrMagnitude > MaxUprightTorqueSquared)
		{
			//LunyLogger.LogWarning($"Clamping {uprightTorque.magnitude} to max {MaxUprightTorque}", this);
			uprightTorque = uprightTorque.normalized * MaxUprightTorque;
		}

		if (!_ignoreMass)
		{
			// This scales the force to the actual weight/shape distribution of the body
			var inertia = _rigidbody.inertiaTensorRotation;
			var inverseInertia = Quaternion.Inverse(inertia);
			uprightTorque = inertia * Vector3.Scale(_rigidbody.inertiaTensor, inverseInertia * uprightTorque);
		}

		// Apply the 'soft-start' factor so the character won't immediately propel upwards when touching ground
		uprightTorque *= _currentUprightDampFactor * _currentUprightDampFactor; // Squared => Ease-In

		_rigidbody.AddTorque(uprightTorque, _ignoreMass ? ForceMode.Acceleration : ForceMode.Force);
	}

	// Shoot a sphere down from the center of the object
	private Boolean IsGrounded()
	{
		// Start the sphere slightly inside the object to avoid starting 'already colliding'
		var origin = GetGroundCastOrigin();
		var distance = GetGroundCastDistance();
		return GroundCast(origin, distance, out var hit) && IsGroundableAngle(hit);
	}

	private Boolean IsGroundableAngle(RaycastHit hit) => Vector3.Angle(Vector3.up, hit.normal) <= _maxAngleConsideredGroundable;

	private Boolean GroundCast(Vector3 origin, Single distance, out RaycastHit hit) =>
		Physics.SphereCast(origin, _groundCheckRadius, Vector3.down, out hit, distance, _groundLayer);

	private Vector3 GetGroundCastOrigin() => transform.position;

	private Single GetGroundCastDistance() => _groundCastMaxDistance;
}
