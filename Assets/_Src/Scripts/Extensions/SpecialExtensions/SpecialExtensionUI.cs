using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Nody;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using UnityEngine;

public static class SpecialExtensionUI
{
    private static FlowController _flowController;
    
    public static string GetCurrentNode()
    {
        if (_flowController == null)
        {
            _flowController = GameObject.FindObjectOfType<FlowController>();
        }

        return _flowController.flow.activeNode.nodeName.SpaceToPascal();
    }

    public static string GetPreviousNode()
    {
        if (_flowController == null)
        {
            _flowController = GameObject.FindObjectOfType<FlowController>();
        }

        return _flowController.flow.previousActiveNode.nodeName.SpaceToPascal();
    }
    
    public static void ShowPopup(this object source,UIId.UIPopupName typePopupName)
    {
        var popup = UIPopup.Get(typePopupName.ToString());
        popup.Show();
    }

    public static T ShowPopup<T>(this object source,UIId.UIPopupName typePopupName)
    {
        var popup = UIPopup.Get(typePopupName.ToString());
        popup.Show();
        return popup.GetComponent<T>();
    }

    public static void ShowProcessing(this object source)
    {
        if (source != null)
        {
            // Debug.Log("ShowProcessing: " + source.GetType().Name);
        }
        ControllerPopup.SetApiLoading(true);
    }

    public static void HideProcessing(this object source)
    {
        ControllerPopup.SetApiLoading(false);
    }
}