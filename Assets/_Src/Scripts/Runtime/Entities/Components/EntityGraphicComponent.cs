using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Slime
{
    public class EntityGraphicComponentData
    {
        public string baseSpritePath;

        public EntityGraphicComponentData(string baseSpritePath)
        {
            this.baseSpritePath = baseSpritePath;
        }
    }

    public class EntityGraphicComponent : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer baseSpriteRender;
        protected bool isInitialized;
        public async void ApplyData(EntityGraphicComponentData data)
        {
            Sprite sprite = await ServiceLocator.GetService<AssetLoader>().LoadSprite(data.baseSpritePath);
            if (baseSpriteRender == null)
            {
                throw new ArgumentException("Graphic is not initialize the right way");
            }
            baseSpriteRender.sprite = sprite;
        }

        // public void ApplyData(Sprite sprite)
        // {
        //     baseSpriteRender.sprite = sprite;
        // }
    }
}