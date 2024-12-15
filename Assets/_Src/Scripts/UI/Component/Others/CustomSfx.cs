using System;
using UnityEngine;

namespace Template.UI
{
    public class CustomSfx : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] public AnR.AudioKey AudioKey;
        [SerializeField, HideInInspector] private string KeyString;

        public void OnBeforeSerialize()
        {
            KeyString = AudioKey.ToString();
        }

        public void OnAfterDeserialize()
        {
            AudioKey = (AnR.AudioKey)Enum.Parse(typeof(AnR.AudioKey), KeyString);
        }
    }
}