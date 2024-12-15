using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Color = System.Drawing.Color;

public static class ExtensionString
{
	public static string ToUpperCase(this string str)
	{
		return str.ToUpper(new CultureInfo("en-US"));
	}

	public static string ToLowerCase(this string str)
	{
		return str.ToLower(new CultureInfo("en-US"));
	}
	
	public static string Truncate(this string value, int maxLength, string truncationSuffix = "…")
	{
		return value?.Length > maxLength
			? value[..maxLength] + truncationSuffix
			: value;
	}
		
	public static string AddSpace(this string text)
	{
		return Regex.Replace(text, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
	}

	public static string SetHighlightStringGreen_5AFF0E(this string value)
	{
		return $@"<color=#5AFF0E>{value}</color>";
	}
	
	
	public static string SetHighlightStringGreen2_60FF4B(this string value)
	{
		return $@"<color=#60FF4B>{value}</color>";
	}
	
	public static string SetHighlightStringGreen3_3FA509(this string value)
	{
		return $@"<color=#3FA509>{value}</color>";
	}
	
	public static string SetHighlightStringGreen4_40FF1A(this string value)
	{
		return $@"<color=#40FF1A>{value}</color>";
	}
	
	public static string SetHighlightStringPink(this string value)
	{
		return $@"<color=#E56467>{value}</color>";
	}

	public static string SetHighlightStringRed(this string value)
	{
		return $@"<color=#FF7669>{value}</color>";
	}
	
	//F16034
	public static string SetHighlightStringOrange(this string value)
	{
		return $@"<color=#F16034>{value}</color>";
	}
	
	public static string SetHighlightString(this string value,UnityEngine.Color color)
	{
		return $@"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{value}</color>";
	}

	public static string ToPercentString<T>(this T value)
	{
		return value + "%";
	}
	
	public static string ToTitleCase(this string str)
	{
		var firstWord = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.Split(' ')[0].ToLowerCase());
		str = str.Replace(str.Split(' ')[0],firstWord);
		return str;
	}
	
	public static string ToCamelCase(string input)
	{
		return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
	}

	public static string SnakeToPascal(this string str)
	{
		return Regex.Replace(str, "(^|_)([a-z\\d])", m => m.Groups[2].Value.ToUpperCase(), RegexOptions.Compiled);
	}
	
	public static string GetOrdinalNumber(int index)
	{
		return index switch
		{
			1 => "ST",
			2 => "ND",
			3 => "RD",
			_ => "TH"
		};
	}

	public static string PascalToSnake(this string str)
	{
		var convertedStr = new StringBuilder();

		for (var index = 0; index < str.Length; index++)
		{
			var c = str[index];
			var preC = index > 0 ? str[index - 1] : ' ';
			if ((index > 0) && (preC != '_') && (char.IsUpper(c) || (char.IsNumber(c) && !char.IsNumber(preC))))
			{
				convertedStr.Append("_");
			}

			convertedStr.Append(c);
		}

		return convertedStr.ToString().ToLowerCase();
	}

	public static string PascalToSpace(this string str)
	{
		return Regex.Replace(str, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
	}

	public static string SpaceToPascal(this string str)
	{
		return Regex.Replace(str, @"\s+", "", RegexOptions.Compiled);
	}
	
	public static string Right(this string str, int length)
	{
		return str.Substring(str.Length - length, length);
	}
	
	public static string ToShortName(this string str)
	{
		try
		{
			var words = str.Trim().Split(' ');
			var maxWords = 3;
			var shortName = "";

			for (var i = 0; i < words.Length; i++)
			{
				if (i >= maxWords) break;
				var c = words[i].ToUpperCase().First();

				if (c >= 'A' && c <= 'Z')
				{
					shortName += c;
				}
				else
				{
					shortName += words[i];
				}
			}
			return shortName.ToUpperCase();
		}
		catch
		{
			return "UNK";
		}
	}

	public static string ToTime(this int duration)
	{
		var days = duration / 86400;
		var hours = (duration % 86400) / 3600;
		var minutes = (duration % 3600) / 60;
		var seconds = duration % 60;

		if (days > 0)
		{
			return $"{days}d {hours:D2}:{minutes:D2}:{seconds:D2}";
		}
		return $"{hours:D2}:{minutes:D2}:{seconds:D2}";

	}

	public static string ToTime(this string str)
	{
		var time = int.Parse(str);
		var hours = time / 3600;
		var minutes = time % 3600 / 60;
		var seconds = time % 60;

		return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
	}
	
	public static string ToShortAddress(this string str)
	{
		var shortAddress = str.Substring(0, 6) + "..." + str.Substring(str.Length - 7);
		return shortAddress;
	}

	public static byte[] ToBytes(this string str)
	{
		var length = str.Length / 2;
		var bytes = new byte[length];

		for (int i = 0; i < length; i++)
		{
			bytes[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
		}

		return bytes;
	}

	public static Dictionary<string, string> ToParams(this string url)
	{
		var param = url.Split('?');
		if (param.Length < 2)
		{
			return null;
		}

		var keyValue = param[1].Split('&');
		var linkParams = new Dictionary<string, string>();

		foreach (var item in keyValue)
		{
			var keyValuePair = item.Split('=');
			if (keyValuePair.Length < 2)
			{
				continue;
			}

			if (!linkParams.ContainsKey(keyValuePair[0]))
			{
				linkParams.Add(keyValuePair[0], keyValuePair[1]);
			}
		}

		return linkParams;
	}
	
	public static string InsertLineBreaks(this string text, int lineLength)
	{
		if (text.Length <= lineLength)
			return text;
		
		StringBuilder sb = new StringBuilder();
		int currentPosition = 0;

		while (currentPosition < text.Length)
		{
			if (currentPosition + lineLength < text.Length)
				sb.Append(text.Substring(currentPosition, lineLength) + "\n");
			else
				sb.Append(text.Substring(currentPosition));

			currentPosition += lineLength;
		}

		return sb.ToString();
	}
	
	// Insertline break after 5 words
	public static string InsertLineBreaksAfterWords(this string text, int wordsPerLine)
	{
		if (string.IsNullOrEmpty(text) || wordsPerLine <= 0)
			return text;

		var words = text.Split(' ');
		var sb = new StringBuilder();
		for (int i = 0; i < words.Length; i++)
		{
			if (i > 0 && i % wordsPerLine == 0)
				sb.Append("\n");
			sb.Append(words[i] + " ");
		}

		return sb.ToString().TrimEnd();
	}

	public static KeyValuePair<int, string> GetInsertLineBreaksAfterWords(this string text, int wordsPerLine)
	{
		KeyValuePair<int, string> result = new();
		if (string.IsNullOrEmpty(text) || wordsPerLine <= 0)
			return result;

		var words = text.Split(' ');
		var sb = new StringBuilder();
		int totalLine = 0;
		for (int i = 0; i < words.Length; i++)
		{
			if (i > 0 && i % wordsPerLine == 0)
			{
				sb.Append("\n");
				totalLine++;
			}

			sb.Append(words[i] + " ");
		}

		result = new KeyValuePair<int, string>(totalLine, sb.ToString().TrimEnd());
		
		
		return result;
	}
	
	public static  KeyValuePair<int, string>  InsertLineBreaksByCharacters(this string text, int maxCharsPerLine)
	{
		KeyValuePair<int, string> result = new();
		if (string.IsNullOrEmpty(text) || maxCharsPerLine <= 0)
			return result;

		var words = text.Split(' ');
		var sb = new StringBuilder();
		var currentLine = new StringBuilder();
		int totalLine = 0;
		if (words.Length == 1)
		{
			int currentPosition = 0;

			while (currentPosition < text.Length)
			{
				if (currentPosition + maxCharsPerLine < text.Length)
					sb.Append(text.Substring(currentPosition, maxCharsPerLine) + "\n");
				else
					sb.Append(text.Substring(currentPosition));

				totalLine++;
				currentPosition += maxCharsPerLine;
			}
			
			result = new KeyValuePair<int, string>(totalLine, sb.ToString().TrimEnd());
			return result;
		}

		foreach (var word in words)
		{
			if (currentLine.Length + word.Length + 1 > maxCharsPerLine)
			{
				totalLine++;
				sb.AppendLine(currentLine.ToString().TrimEnd());
				currentLine.Clear();
			}
			currentLine.Append(word + " ");
		}

		if (currentLine.Length > 0)
			sb.AppendLine(currentLine.ToString().TrimEnd());
		
		result = new KeyValuePair<int, string>(totalLine, sb.ToString().TrimEnd());

		return result;
	}
	
	public static string ToBold(this string value)
	{
		return $"<b>{value}</b>";
	}
}