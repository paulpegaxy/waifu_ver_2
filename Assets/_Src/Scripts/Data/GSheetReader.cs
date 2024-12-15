using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ExcelDataReader;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;


[Serializable]
public static class GSheetReader
{
    private const BindingFlags AllFieldBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy;
    public static Dictionary<string,List<Dictionary<string,string>>> DownloadSheet(string sheetKey)
    {
        var url = $"https://spreadsheets.google.com/feeds/download/spreadsheets/Export?key={sheetKey}&exportFormat=xlsx";
        var req = UnityWebRequest.Get(url);
        req.SendWebRequest();
        while (!req.isDone){}
        using var stream = new MemoryStream(req.downloadHandler.data);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var dataset = reader.AsDataSet(new ExcelDataSetConfiguration());
        var res = new Dictionary<string, List<Dictionary<string, string>>>();
        for (var i = 0; i < dataset.Tables.Count; ++i)
        {
            var table = dataset.Tables[i];
            res.Add(table.TableName, new List<Dictionary<string, string>>());
            var headers = new List<string>();
            for (var row = 0; row < table.Rows.Count; row++)
            {
                if (row == 0)
                    headers = table.Rows[row].ItemArray.Select(cell => cell.ToString()).ToList();
                else
                {
                    var rowDict = new Dictionary<string, string>();
                    for (var j = 0; j < table.Rows[row].ItemArray.Length; j++)
                        rowDict[headers[j]] = table.Rows[row].ItemArray[j].ToString();
                    res[table.TableName].Add(rowDict);
                }
            }
        }
        return res;
    }

    public static void ParseSheet(this Dictionary<string, List<Dictionary<string, string>>> sheet, object obj)
    {
        var type = obj.GetType();
        foreach (var field in type.GetFields(AllFieldBindingFlags))
        {
            if (!sheet.ContainsKey(field.Name)) continue;
            sheet.ParseTable(field.Name, type.GetField(field.Name, AllFieldBindingFlags)?.GetValue(obj));
        }
    }

    public static void ParseTable(this Dictionary<string,List<Dictionary<string, string>>> sheet,string tableName, object obj)
    {
        if (obj is IList list)
        {
            list.Clear();
            for (var i = 0; i < sheet[tableName].Count; i++)
            {
                var elementType = sheet[tableName][i].ContainsKey("$type")
                    ? Type.GetType(sheet[tableName][i]["$type"])
                    : obj.GetType().GetGenericArguments()[0];
                try
                {
                    var element = Activator.CreateInstance(elementType ?? typeof(object));
                    ParseRow(sheet, tableName, i, element);
                    // try
                    // {
                    //     ParseRow(sheet, tableName, i, element);
                    // }
                    // catch (Exception e)
                    // {
                    //     Debug.LogError($@"Error at {tableName} - index: {i}");
                    //     Debug.LogError(e);
                    //     var rowTable = sheet[tableName][i];
                    //     foreach (var itemRow in rowTable)
                    //     {
                    //         if (!string.IsNullOrEmpty(itemRow.Value))
                    //             Debug.Log($@"[{element.GetType().Name}]  {itemRow.Key}: {itemRow.Value}");
                    //     }
                    // }
                    list.Add(element);
                }
                catch (Exception e)
                {
                   Debug.LogException(e);
                }
            }
        }
        else
        {
            ParseRow(sheet, tableName, 0, obj);
        }
    }

    public static T ParseRow<T>(this Dictionary<string, List<Dictionary<string, string>>> sheet, string tableName, int idxRow, T obj)
    {
        var valueReturn = obj;
        var row = sheet[tableName][idxRow];
          var type = valueReturn.GetType();
          foreach (var field in type.GetFields(AllFieldBindingFlags))
          {
              if (!row.ContainsKey(field.Name)) continue;
              if (sheet.ContainsKey(row[field.Name]))
              {
                  sheet.ParseTable(row[field.Name],
                      type.GetField(field.Name, AllFieldBindingFlags)?.GetValue(valueReturn));
                  continue;
              }

              object value = null;
              var parseMethod = field.FieldType.GetMethod("Parse", new[] {typeof(string)});
              if (parseMethod != null)
              {
                  try
                  {
                      value = parseMethod.Invoke(null, new object[] {row[field.Name]});
                  }
                  catch (Exception e)
                  {
                      Debug.Log("Error at : " + type.Name + ", " + field.Name);
                      Console.WriteLine(e);
                      throw;
                  }
              }
              else if (field.FieldType.IsEnum)
                  value = Enum.Parse(field.FieldType, row[field.Name]);

#if UNITY_EDITOR
              else if (field.FieldType.IsSubclassOf(typeof(Object)))
                  value = UnityEditor.AssetDatabase.LoadAssetAtPath(row[field.Name], field.FieldType);
#endif
              else if (field.FieldType == typeof(string))
                  value = row[field.Name];
              else if (field.FieldType == typeof(Vector3))
              {
                  var sVector = row[field.Name];
                  if (sVector.StartsWith("(") && sVector.EndsWith(")"))
                  {
                      sVector = sVector.Substring(1, sVector.Length - 2);
                  }

                  string[] sArray = sVector.Split(',');
                  value = new Vector3(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]));
              }
              else
                  value = JsonConvert.DeserializeObject(row[field.Name], field.FieldType);

              type.GetField(field.Name).SetValue(valueReturn, value);
          }

          return valueReturn;
    }

    public static void ParseRow(this Dictionary<string, List<Dictionary<string, string>>> sheet, string tableName,
        int idxRow, object obj)
    {
        var row = sheet[tableName][idxRow];
        var type = obj.GetType();
        foreach (var field in type.GetFields(AllFieldBindingFlags))
        {
            if (!row.ContainsKey(field.Name)) continue;
            if (sheet.ContainsKey(row[field.Name]))
            {
                sheet.ParseTable(row[field.Name], type.GetField(field.Name, AllFieldBindingFlags)?.GetValue(obj));
                continue;
            }

          
         
            
            // try
            // {
                object value = null;
                var parseMethod = field.FieldType.GetMethod("Parse", new[] {typeof(string)});
                if (parseMethod != null)
                {
                    try
                    {
                        value = parseMethod.Invoke(null, new object[] {row[field.Name]});
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error at : "+type.Name+", "+field.Name);
                        Console.WriteLine(e);
                        throw;
                    }
                }
                else if (field.FieldType.IsEnum)
                    value = Enum.Parse(field.FieldType, row[field.Name]);
                
#if UNITY_EDITOR
                else if (field.FieldType.IsSubclassOf(typeof(Object)))
                    value = UnityEditor.AssetDatabase.LoadAssetAtPath(row[field.Name], field.FieldType);
#endif
                else if (field.FieldType == typeof(string)) 
                    value = row[field.Name];
                else if (field.FieldType == typeof(Vector3))
                {
                    var sVector = row[field.Name];
                    if (sVector.StartsWith("(") && sVector.EndsWith(")"))
                    {
                        sVector = sVector.Substring(1, sVector.Length - 2);
                    }

                    string[] sArray = sVector.Split(',');
                    value = new Vector3(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]));
                }
                else
                    value = JsonConvert.DeserializeObject(row[field.Name], field.FieldType);
                type.GetField(field.Name).SetValue(obj, value);
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     throw;
            // }
        }
    }
    //public static bool IsArrayOrList(this Type type)=>type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
}
