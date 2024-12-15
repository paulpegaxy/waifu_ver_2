using System.Collections.Generic;
using UnityEngine;

namespace _Src.Scripts.Data.DBM.Configs
{
    public partial class Config
    {
        [field: SerializeField] public MessageGirlSOData messageConfig;
    }


    [CreateAssetMenu(fileName = "MessageGirlSOData", menuName = "SO/Config/MessageGirlSOData", order = 0)]
    public class MessageGirlSOData : ScriptableObject
    {
        
    }
}