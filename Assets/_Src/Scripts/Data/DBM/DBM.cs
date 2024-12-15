using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "DBM", menuName = "SO/DBM")]
[PreferBinarySerialization]
public class DBM : SingletonScriptableObject<DBM>
{
    [SerializeField] private Config config;

    public static Config Config => GetInstance().config;
}

[Serializable]
public partial class Config
{
}