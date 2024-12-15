using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ExtensionUI
{
    public static List<T> FillData<TD, T>(this Component component, IEnumerable<TD> data, Action<TD, T, int> itemAction = null) where T : Component
    {
        var res = new List<T>();
        var listData = data.ToList();
        var transform = component.transform;

        for (var i = 0; i < Mathf.Max(listData.Count, transform.childCount); i++)
        {
            if (i == transform.childCount) Object.Instantiate(transform.GetChild(0), transform);
            transform.GetChild(i).gameObject.SetActive(i < listData.Count);
            if (i < listData.Count)
            {
                var view = transform.GetChild(i).GetComponent<T>();
                var tdView = view as IItemView<TD>;
                if (tdView != null) tdView.Setup(listData[i]);
                res.Add(view);
                itemAction?.Invoke(listData[i], view, i);
            }
        }

        return res;
    }
    
    public static void ReloadAnScrollToElement<T>(this EnhancedScroller scroller,List<T> listData,Predicate<T> conditionCheck)
    {
        scroller.ReloadData();
        int lastIndex = listData.FindLastIndex(conditionCheck);
        scroller.JumpToDataIndex(lastIndex);
    }
    
    public interface IItemView<T>
    {
        void Setup(T data);
    }
}

