// 12/11/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using System.Collections;
using UnityEngine;

public sealed class RagdollController : MonoBehaviour
{
	[SerializeField] private Boolean _ragdollEnabled;
	private Rigidbody[] _rigidbodies;
	private Animator _animator;
	private Animation _animation;
	private Transform[] _transforms;
	private Vector3[] _initialPositions;
	private Quaternion[] _initialRotations;

	private void Awake()
	{
		_rigidbodies = GetComponentsInChildren<Rigidbody>();
		_transforms = GetComponentsInChildren<Transform>();
		_animator = GetComponent<Animator>();
		_animation = GetComponent<Animation>();

		// Store initial body-part positions and rotations
		_initialPositions = new Vector3[_transforms.Length];
		_initialRotations = new Quaternion[_transforms.Length];
		for (var i = 0; i < _transforms.Length; i++)
		{
			_initialPositions[i] = _transforms[i].localPosition;
			_initialRotations[i] = _transforms[i].localRotation;
		}

		StartCoroutine(SetRagdollState(_ragdollEnabled));
	}

	private void OnValidate()
	{
		if (Application.isPlaying && enabled && gameObject.activeInHierarchy)
			StartCoroutine(SetRagdollState(_ragdollEnabled));
	}

	private IEnumerator SetRagdollState(Boolean isRagdoll)
	{
		// TODO: keep momentum from animation?

		if (_rigidbodies != null)
		{
			foreach (var rb in _rigidbodies)
				rb.isKinematic = !isRagdoll;
		}

		if (!isRagdoll)
			RestoreInitialPose();

		// prevents limbs from being offset when disabling ragdoll mode again
		yield return new WaitForFixedUpdate();

		if (_animator != null)
			_animator.enabled = !isRagdoll;
		if (_animation != null)
			_animation.enabled = !isRagdoll;
	}

	private void RestoreInitialPose()
	{
		if (_transforms != null)
		{
			for (var i = 0; i < _transforms.Length; i++)
			{
				_transforms[i].localPosition = _initialPositions[i];
				_transforms[i].localRotation = _initialRotations[i];
			}
		}
	}

	public void EnableRagdoll() => SetRagdollState(true);

	public void DisableRagdoll() => SetRagdollState(false);
}
