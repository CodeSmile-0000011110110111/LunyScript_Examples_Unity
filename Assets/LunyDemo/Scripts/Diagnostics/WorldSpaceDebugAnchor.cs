using TMPro;
using UnityEngine;

namespace LunyScript
{
	public sealed class WorldSpaceDebugAnchor : MonoBehaviour
	{
		private Camera _mainCam;
		private RectTransform _rectTransform;

		[field: SerializeField] public TMP_Text Label { get; set; }
		[field: SerializeField] public Transform Target { get; set; }
		[field: SerializeField] public Vector3 Offset { get; set; }

		private void Awake()
		{
			if (Label == null)
				Label = GetComponentInChildren<TMP_Text>();

			_mainCam = Camera.main;
			_rectTransform = transform as RectTransform;
		}

		private void LateUpdate()
		{
			if (Target == null || _mainCam == null)
				return;

			var screenPos = _mainCam.WorldToScreenPoint(Target.position + Offset);
			if (screenPos.z >= 0)
			{
				_rectTransform.localScale = Vector3.one;
				transform.position = screenPos;
			}
			else
				_rectTransform.localScale = Vector3.zero;
		}
	}
}
