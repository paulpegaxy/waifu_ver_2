using Game.Model;
using TMPro;
using UnityEngine;

namespace Game.UI.Ver2
{
    public class ItemUICharTagInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtCharName;
        [SerializeField] private TMP_Text txtCharYearOld;
        [SerializeField] private TMP_Text txtDescription;
        [SerializeField] private ItemBarTag itemBarTag;
        [SerializeField] private TMP_Text txtRate;

        public void SetData(ModelApiEntityConfig data)
        {
            txtCharName.text = data.name;
            txtCharYearOld.text = data.year_old.ToString();
            txtDescription.text = data.bio;
            itemBarTag.SetData(data.GetGenresList());
            txtRate.text = data.GetPercentMatchRateString();
        }
    }
}