using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEVObject : MonoBehaviour
{
    private void Awake()
    {
#if PRODUCTION_BUILD
        gameObject.SetActive(false);
        return;
#endif
        gameObject.SetActive(true);
    }
}
