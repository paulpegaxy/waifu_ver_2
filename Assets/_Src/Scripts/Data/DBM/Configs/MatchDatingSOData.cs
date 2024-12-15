

using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public partial class Config
{
    [field: SerializeField] public MatchDatingSOData matchDatingConfig;
}

[CreateAssetMenu(fileName = "MatchDatingSOData", menuName = "SO/Config/MatchDatingSOData", order = 0)]
public class MatchDatingSOData : ScriptableObject
{
    [field: SerializeField] private string keyGgSheet = "1pR9wc_KYwmalLbhqzO30d1Z3GDHazG7fFfXlAL2XahQ";
    
    [field: SerializeField] private GenericDictionary<string, DataItemMatchDating> dictData;

    private const int MAX_OPTION = 2;

    private List<DataItemMatchDating> ListData => dictData.Values.ToList();


    public DataItemMatchDating GetRandomItem()
    {
        if (dictData.Count == 0)
            return null;

        Random random = new Random();
        int index = random.Next(dictData.Count);
        return ListData[index];
    }

    public DataItemMatchDating GetConfig(string id)
    {
        if (dictData.TryGetValue(id, out DataItemMatchDating config))
            return config;

        return null;
    }

    [Button("Save")]
    public void AutoProcessData()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }

    [Button("Load GGSheet Data")]
    public void LoadData()
    {
#if UNITY_EDITOR
        LoadGgSheetData();
        AutoProcessData();
#endif
    }

    private void LoadGgSheetData()
    {
        var sheet = GSheetReader.DownloadSheet(keyGgSheet);
        foreach (var itemGirlMess in dictData)
        {
            var table = sheet[itemGirlMess.Key];
            if (table == null)
            {
                Debug.LogError("Missing table: " + itemGirlMess.Key);
                continue;
            }

            itemGirlMess.Value.listMessData = new List<DataItemMessageTree>();
            itemGirlMess.Value.listMessData =  ProcessDictMess(table);;
        }
    }

    private  List<DataItemMessageTree> ProcessDictMess(List<Dictionary<string,string>> tableDictList)
    {
        var listDataTree = new List<DataItemMessageTree>();
        var groupList = tableDictList.GroupBy(x => x["TreeIndex"])
            .ToDictionary(x => x.Key, v => v.ToList());
        
        foreach (var group in groupList)
        {
            var treeIndex = int.Parse(group.Key);
            var listDetail = group.Value;

            var listData = new List<DataItemMessageTreeDetail>();

            for (int i = 0; i < listDetail.Count; i++)
            {
                var detail = listDetail[i];
                var dataDetail = new DataItemMessageTreeDetail();
                dataDetail.nodeId = detail["NodeId"];
                dataDetail.treeIndex = treeIndex;
                dataDetail.nodeChildIdInTree = detail["NodeIndexInTree"];
                dataDetail.speaker = detail["Speaker"];
                dataDetail.data = new DataItemMessageTreeDetailItem
                {
                    message = detail["Message"],
                    listOption = new List<DataItemMessageOption>(),
                    nextNodeId = detail["NextNode"]
                };
                
                for (int j = 1; j <= MAX_OPTION; j++)
                {
                    var optionMessage = detail[$"Option{j}"];
                    var nextNodeId = detail["NextNodeOption"].Split(';');

                    if (!string.IsNullOrEmpty(optionMessage))
                    {
                        if (nextNodeId.Length < 2)
                        {
                            Debug.LogError("Missing config next node Id array matching with Options");
                            return null;
                        }

                        var option = new DataItemMessageOption
                        {
                            optionMessage = optionMessage,
                            nextNodeOptionId = nextNodeId[j - 1]
                        };
                        dataDetail.data.listOption.Add(option);
                    }
                }

                listData.Add(dataDetail);
            }

            listDataTree.Add(new DataItemMessageTree(listData));
        }

        return listDataTree;
    }
}


[Serializable]
public class DataItemMatchDating
{
    public int girlId;
    public float rateMatching;
    public List<DataItemMessageTree> listMessData;

    public bool IsMatchingSuccess()
    {
#if UNITY_EDITOR
        return true;
#endif
        
        Random random = new Random();
        return random.NextDouble() <= rateMatching;
    }
}

[Serializable]
public class DataItemMessageTree
{
    public GenericDictionary<string, DataItemMessageTreeDetail> dictTreeChild;

    public DataItemMessageTree(List<DataItemMessageTreeDetail> listData)
    {
        dictTreeChild = new GenericDictionary<string, DataItemMessageTreeDetail>();
        foreach (var item in listData)
        {
            dictTreeChild.Add(item.nodeChildIdInTree, item);
        }
    }

    public DataItemMessageTreeDetail GetFirstData()
    {
        return dictTreeChild.Values.FirstOrDefault();
    }

    public DataItemMessageTreeDetail GetData(string nodeId)
    {
        return dictTreeChild.TryGetValue(nodeId, out DataItemMessageTreeDetail data) ? data : null;
    }
}

[Serializable]
public class DataItemMessageTreeDetail
{
    public string nodeId;
    public int treeIndex;
    public string nodeChildIdInTree;
    public string speaker;
    public DataItemMessageTreeDetailItem data;

    public bool IsWaifuSpeak()
    {
        return speaker.Equals("Waifu");
    }
}


[Serializable]
public class DataItemMessageTreeDetailItem
{
    public string message;
    public List<DataItemMessageOption> listOption;
    public string nextNodeId;
}

[Serializable]
public class DataItemMessageOption
{
    public string optionMessage;
    public string nextNodeOptionId;
}