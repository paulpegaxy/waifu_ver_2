using UnityEngine;
using TMPro;

namespace Game.UI
{
    public class PopupInformation : MonoBehaviour
    {
        [SerializeField] private TMP_Text textDescription;

        public void SetData(string description)
        {
            textDescription.text = description;
        }
    }
}