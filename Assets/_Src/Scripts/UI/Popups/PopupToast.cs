using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game.UI
{
    public enum TypeToastStatus
    {
        None,
        Success,
        Error
    }
    
    public class PopupToast : MonoBehaviour
    {
        [SerializeField] private Image imgHolder;
        [SerializeField] private Image iconStatus;
        [SerializeField] private TMP_Text textDescription;
        [SerializeField] private Sprite[] arrIconStatus;
        [SerializeField] private Color[] arrColorStatus;

        public void SetData(string description, TypeToastStatus status = TypeToastStatus.None)
        {
            if (gameObject.activeSelf)
            {
                GetComponent<UIPopup>().Hide();
            }
            
            textDescription.text = description;
            bool isHide = status == TypeToastStatus.None;
            iconStatus.gameObject.SetActive(!isHide);

            if (!isHide)
            {
                imgHolder.color = status == TypeToastStatus.Error ? arrColorStatus[^1] : arrColorStatus[0];
                iconStatus.sprite = status == TypeToastStatus.Error ? arrIconStatus[^1] : arrIconStatus[0];
                // textDescription.color = status == TypeToastStatus.Error ? arrColorStatus[^1] : arrColorStatus[0];
            }
            else
            {
                // textDescription.color = arrColorStatus[0];
                imgHolder.color = arrColorStatus[0];
            }
        }
    }
}