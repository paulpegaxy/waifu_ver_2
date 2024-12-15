using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Game.Model;
using Newtonsoft.Json.Linq;
using System.Text;

public static class ExtensionApi
{
	public static string ToParams(this object data)
	{
		if (data == null) return $"t={DateTime.Now.Ticks}";

		var jObject = JObject.FromObject(data);
		var sb = new StringBuilder();

		foreach (var prop in jObject.Properties())
		{
			if (sb.Length > 0) sb.Append("&");
			sb.Append($"{prop.Name}={prop.Value}");
		}

		return $"{sb}&t={DateTime.Now.Ticks}";
	}

	public static T Parse<T>(this string data, string selectToken = "")
	{
		var jObject = JObject.Parse(data);
		var jToken = string.IsNullOrEmpty(selectToken) ? jObject : jObject.SelectToken(selectToken);

		return jToken != null ? jToken.ToObject<T>() : default;
	}
}