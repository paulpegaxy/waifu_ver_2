using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GalleryItemIndicator : MonoBehaviour
{
    [SerializeField] private GameObject objActive;
    [SerializeField] private GameObject objSelected;
    

    public void LoadData(bool isActive)
    {
        objSelected.transform.DOScale(0, 0);
        objActive.SetActive(isActive);
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
            objSelected.transform.DOScale(1, GameConsts.ANIM_NORMAL_DURATION_TIME);
        else
            objSelected.transform.DOScale(0, GameConsts.ANIM_NORMAL_DURATION_TIME);
    }
}
