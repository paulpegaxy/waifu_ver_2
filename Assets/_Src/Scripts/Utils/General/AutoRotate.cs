using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
   private void OnEnable()
   {
      transform.DORotate(new Vector3(0, -360, 0), 1.05f, RotateMode.FastBeyond360)
         .From(Vector3.zero)
         .SetLoops(-1)
         .SetEase(Ease.Linear);
   }

   private void OnDisable()
   {
      DOTween.Kill(transform);
   }
}
