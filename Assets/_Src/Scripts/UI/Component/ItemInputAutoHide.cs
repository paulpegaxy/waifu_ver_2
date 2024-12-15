using System;
using System.Collections;
using System.Collections.Generic;
using AOT;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using WebGLSupport;
using WebGLInput = WebGLSupport.WebGLInput;

[RequireComponent(typeof(TMP_InputField))]
public class ItemInputAutoHide : MonoBehaviour
{
    private TMP_InputField _ipf;
    
    private TMP_InputField Input => _ipf ??= GetComponent<TMP_InputField>();


    private void OnEnable()
    {

        // Input.onDeselect.AddListener(OnDeselect);   
        // Input.onEndEdit.AddListener(OnEndEdit);
        // WebGLWindowPlugin.WebGLWindowOnFocus(OnWindowFocus);
        // WebGLWindowPlugin.WebGLWindowOnBlur(OnWindowBlur);
        // #if UNITY_EDITOR
        //
        // Input.onSelect.AddListener(OnSelect);
        // #endif
        // WebGLWindow.OnFocusEvent += OnWindowFocus;
        // WebGLWindow.OnBlurEvent += OnWindowBlur;
        // WebGLWindow.OnResizeEvent += OnWindowResize;
        

        // WebGLInput.OnShowInput += OnShowInput;
        // WebGLInput.OnHideInput += OnHideInput;
        
    }

    private void OnDisable()
    {
        // WebGLInput.OnShowInput -= OnShowInput;
        // WebGLInput.OnHideInput -= OnHideInput;

 

// #if UNITY_EDITOR
//       
//         Input.onSelect.RemoveListener(OnSelect);
// #endif
  
        // WebGLWindow.OnFocusEvent -= OnWindowFocus;
        // WebGLWindow.OnBlurEvent -= OnWindowBlur;
        // WebGLWindow.OnResizeEvent -= OnWindowResize;
        // Input.onDeselect.RemoveListener(OnDeselect);
        // Input.onEndEdit.RemoveListener(OnEndEdit);
    }
    
    private void OnShowInput()
    {
        // Debug.Log("OnShowInput Input");
        // ControllerPopup.ShowToastError("Is resizing");
        ControllerPopup.SetProtectInput(true);
        // ControllerPopup.ShowToast("OnShowInput Input");
        // this.PostEvent(TypeGameEvent.ShowInputField);
    }
    
    private void OnHideInput()
    {
        // Debug.Log("OnHideInput Input");
        // ControllerPopup.ShowToastSuccess("Out resize");
        // ControllerPopup.HideProtectInput();
        // this.PostEvent(TypeGameEvent.HideInputField);
    }
    
    private void OnWindowBlur()
    {
        // Debug.Log("OnWindowBlur Input");
        ControllerPopup.ShowToastSuccess("OnWindow Blur ");
        // ControllerPopup.HideProtectInput();
    }
    
    private void OnWindowFocus()
    {
        // Debug.Log("OnWindowFocus Input");
        ControllerPopup.ShowToast("OnWindow Focus");
    }

    private void OnWindowResize(bool status)
    {
        ControllerPopup.HideProtectInput();

        Debug.Log("OnWindowResize Input");
        // if (status)
        // {
        //     // this.ShowProcessing();
        //     // ControllerPopup.SetProtectInput(true);
        //     ControllerPopup.ShowToastError("Is resizing");
        // }
        // else
        // {
        //     // ControllerPopup.ShowToastSuccess("Out resize");
        //     // ControllerPopup.SetProtectInput(false);
        //     // this.HideProcessing();
        //     ControllerPopup.ShowToastSuccess("Out resize");
        // }
    }

    private void OnSelect(string value)
    {
        // Debug.Log("OnSelect: " + value);
        // EventSystem.current.SetSelectedGameObject(null); // Hủy chọn InputField
   
        ControllerPopup.SetProtectInput(true);
        // this.ShowPopup(UIId.UIPopupName.PopupProtectInput);
    }

    private void OnDeselect(string value)
    {
        // Debug.Log("OnDeselect: " + value);
        // this.PostEvent(TypeGameEvent.HideInputField);
        
        // ControllerPopup.SetProtectInput(false);
    }
    
    private void OnEndEdit(string value)
    {
        // Debug.LogError("OnEndEdit: " + value);
    }
    
   
    // [MonoPInvokeCallback(typeof(Action))]
    // void OnWindowFocus()
    // {
    //     Debug.Log("OnWindowFocus Input");
    // }

    // [MonoPInvokeCallback(typeof(Action))]
    // void OnWindowBlur()
    // {
    //     Debug.Log("OnWindowBlur Input");
    //     this.PostEvent(TypeGameEvent.HideInputField);
    // }
}
