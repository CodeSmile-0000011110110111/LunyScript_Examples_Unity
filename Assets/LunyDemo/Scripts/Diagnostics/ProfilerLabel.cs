using Luny;
using Luny.Engine;
using Luny.Engine.Diagnostics;
using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public sealed class ProfilerLabel : MonoBehaviour
{
	[SerializeField] private LunyEngineLifecycleEvents m_ShowCategories = LunyEngineLifecycleEvents.OnEngineStartup | LunyEngineLifecycleEvents.OnEngineHeartbeat |
	                                                                      LunyEngineLifecycleEvents.OnEngineUpdate | LunyEngineLifecycleEvents.OnEngineLateUpdate;
	[SerializeField] [Range(1, 120)] private Int32 m_UpdateIntervalInFrames = 20;

	private TMP_Text m_Text;
	private ILunyEngineProfiler m_Profiler;
	private StringBuilder m_StringBuilder = new();

	private void OnValidate()
	{
		if (Application.isPlaying && m_Profiler != null)
			StartSnapshotUpdates();
	}

	private void Awake()
	{
		m_Text = GetComponent<TMP_Text>();
		m_Profiler = LunyEngine.Instance.Profiler;
	}

	private void OnEnable() => StartSnapshotUpdates();
	private void OnDisable() => StopAllCoroutines();

	private void StartSnapshotUpdates()
	{
		StopAllCoroutines();
		StartCoroutine(UpdateProfilerSnapshot());
	}

	private IEnumerator UpdateProfilerSnapshot()
	{
		var nextIntervalFrame = Time.frameCount + m_UpdateIntervalInFrames;
		var waitInterval = new WaitUntil(() =>
		{
			if (Time.frameCount < nextIntervalFrame)
				return false;

			nextIntervalFrame = Time.frameCount + m_UpdateIntervalInFrames;
			return true;
		});

		var waitForEndOfFrame = new WaitForEndOfFrame();

		while (true)
		{
			UpdateLabel();

			yield return waitInterval;
			yield return waitForEndOfFrame;
		}
	}

	private void UpdateLabel()
	{
		var snapshot = m_Profiler.TakeSnapshot();
		m_StringBuilder.AppendLine(snapshot.ToString());

		foreach (var category in snapshot.CategorizedMetrics)
		{
			if ((m_ShowCategories & category.Key) == 0)
				continue;

			m_StringBuilder.AppendLine($"[{category.Key}]");
			foreach (var metrics in category.Value)
			{
				m_StringBuilder.AppendLine($"    {metrics.ObserverName} Ø {metrics.AverageMs:F2} ms " +
				                           $"({metrics.MinMs:F2}—{metrics.MaxMs:F2} ms)");
			}
		}

		m_Text.text = m_StringBuilder.ToString();
		m_StringBuilder.Clear();
	}
}
