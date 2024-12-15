using UnityEngine;

namespace Game.UI
{
    public class ItemResourceEffectIcon : MonoBehaviour
    {
        [SerializeField] private ParticleSystem effect;
        
        public void SetData(Sprite sprIcon)
        {
            effect.textureSheetAnimation.SetSprite(0, sprIcon);
        }
    }
}