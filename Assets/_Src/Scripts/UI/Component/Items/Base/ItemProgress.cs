using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game.UI
{
    public abstract class ItemProgress : MonoBehaviour
    {
        [SerializeField] protected Image imageFill;

        private float _progress;
        private float _width;

        public float Process => _progress;

        private void Awake()
        {
            _width = imageFill.rectTransform.rect.width;
        }

        public virtual void SetProgress(float progress, float time = 0f, bool isOverHeat = false)
        {
            if (imageFill.type == Image.Type.Filled)
            {
                imageFill.DOFillAmount(progress, time);
            }
            else
            {
                float percent = progress - 1f;
                percent = Mathf.Clamp(percent, -1f, 0f);

                if (time == 0)
                {
                    imageFill.rectTransform.anchoredPosition = new Vector2(_width * percent, 0);
                }
                else
                {
                    if (!isOverHeat)
                    {
                        imageFill.rectTransform.DOAnchorPosX(_width * percent, time);
                    }
                    else
                    {
                        imageFill.rectTransform.DOAnchorPosX(0, time / 2f).OnComplete(() =>
                        {
                            imageFill.rectTransform.DOAnchorPosX(_width * percent, time / 2f);
                        });
                    }
                }
            }

            _progress = progress;
        }
    }
}