using System;
using UnityEngine;
using System.Collections;
using System.Text;
using Unity.Profiling;
using UnityEditor;

public class ShowFPS : MonoBehaviour
{
	private float _deltaTime = 0.0f;
	ProfilerRecorder setPassCallsRecorder;
	ProfilerRecorder drawCallsRecorder;
	ProfilerRecorder verticesRecorder;
	ProfilerRecorder totalReservedMemoryRecorder;
	ProfilerRecorder gcReservedMemoryRecorder;
	ProfilerRecorder systemUsedMemoryRecorder;
	string statsText;
	private int _trisCount;

	void OnEnable()
	{
		setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
		drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
		verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
		// totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
		// gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
		// systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
	}


	private void Update()
	{
		_deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
		var sb = new StringBuilder(500);
		// if (totalReservedMemoryRecorder.Valid)
		// 	sb.AppendLine($"Total Reserved Memory: {totalReservedMemoryRecorder.LastValue} - " +
		// 	              $"{Mathf.Round(totalReservedMemoryRecorder.LastValue / Mathf.Pow(1024, 2))} MB");
		// if (gcReservedMemoryRecorder.Valid)
		// 	sb.AppendLine($"GC Reserved Memory: {gcReservedMemoryRecorder.LastValue} - " +
		// 	              $"{Mathf.Round(gcReservedMemoryRecorder.LastValue / Mathf.Pow(1024, 2))} MB");
		// if (systemUsedMemoryRecorder.Valid)
		// 	sb.AppendLine($"System Used Memory: {systemUsedMemoryRecorder.LastValue} - " +
		// 	              $"{Mathf.Round(systemUsedMemoryRecorder.LastValue / Mathf.Pow(1024, 2))} MB");
		if (verticesRecorder.Valid)
			sb.AppendLine($"Vertices: {verticesRecorder.LastValue} - Tris: {Mathf.RoundToInt((float)verticesRecorder.LastValue / 3)}");
		if (setPassCallsRecorder.Valid)
			sb.AppendLine($"SetPass Calls: {setPassCallsRecorder.LastValue}");
		if (drawCallsRecorder.Valid)
			sb.AppendLine($"Draw Calls: {drawCallsRecorder.LastValue}");
		statsText = sb.ToString();
	}

	private void OnGUI()
	{
		int w = Screen.width, h = Screen.height;
		const float baseSize = 2.5f / 100f;
		var fontSize = (int) (h * baseSize);

		var style = new GUIStyle
		{
			alignment = TextAnchor.UpperRight,
			fontSize = fontSize,
			normal = {textColor = new Color(0.0f, 1f, 0f, 1.0f)}
		};
		var rect = new Rect(0, 80, w, fontSize);
		var text = string.Format("{0}", statsText);
		GUI.Label(rect, text, style);
	}
}
