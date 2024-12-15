// Author: 
// Created Date: 26/07/2024
// Update Time: 26/07

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class GirlSpineView : ASpineView
{
    [ReadOnly] [SpineSkin] public string currentSkin;
    
    [TabGroup("TAB Anim")]
    [SpineAnimation] public string animIdle;
    [TabGroup("TAB Anim")]
    [SpineAnimation] public string animReactHead;
    [TabGroup("TAB Anim")]
    [SpineAnimation] public string animReactBoob;
    [TabGroup("TAB Anim")]
    [SpineAnimation] public string animReactPussy;
    
    [TabGroup("TAB Skin")]
    [SpineSkin] public List<string> listSkin;

    [TabGroup("TAB Event")]
    [SpineEvent] public string testEvent;
}