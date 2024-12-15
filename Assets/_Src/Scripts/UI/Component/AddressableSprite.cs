using System;
using Game.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class AddressableSprite : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private AnR.SpriteKey key;
        [HideInInspector] public string keyString;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            LoadSprite(AnR.GetKey(key));
        }

        private void LoadSprite(string key)
        {
            _image.LoadSpriteAsync(key);
        }

        public void OnBeforeSerialize()
        {
            keyString = key.ToString();
        }

        public void OnAfterDeserialize()
        {
            key = (AnR.SpriteKey)Enum.Parse(typeof(AnR.SpriteKey), keyString);
        }
    }
}