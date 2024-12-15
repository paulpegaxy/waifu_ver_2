using System.Collections;
using System.Collections.Generic;
using Game.Defines;
using UnityEngine;

public class ItemBarTag : MonoBehaviour
{
    public void SetData(List<TypeStoryGenres> listType)
    {
        transform.FillData<TypeStoryGenres, ItemUITag>(listType, (data, view, index) =>
        {
            view.SetData(data);
        });
    }
}
