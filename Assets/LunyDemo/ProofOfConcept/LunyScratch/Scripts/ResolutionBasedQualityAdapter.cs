using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public sealed class ResolutionBasedQualityAdapter : MonoBehaviour
{
	[SerializeField] private Single _defaultRenderScaleAtFullHD = 0.2f;

	private Single _dofFocusDistance;
	private Single _dofFocalLength;
	private Single _dofAperture;

	private void Awake()
	{
		var height = Screen.height;
		var rpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
		if (rpAsset != null)
		{
			var resolutionScaleFactor = 1080f / height;
			var scaledRenderScale = _defaultRenderScaleAtFullHD * resolutionScaleFactor;
			rpAsset.renderScale = scaledRenderScale;
			var actualScaleFactor = rpAsset.renderScale / scaledRenderScale;

			var volume = GetComponent<Volume>();
			if (volume.sharedProfile.TryGet<DepthOfField>(out var sharedDof) && volume.profile.TryGet<DepthOfField>(out var dof))
			{
				_dofFocusDistance = sharedDof.focusDistance.value;
				_dofFocalLength = sharedDof.focusDistance.value;
				_dofAperture = sharedDof.aperture.value;

				dof.focusDistance.value = _dofFocusDistance * actualScaleFactor;
				dof.focusDistance.value = _dofFocalLength * actualScaleFactor;
				dof.aperture.value = _dofAperture * actualScaleFactor;
			}
		}
	}
}
