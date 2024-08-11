using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HUDBar : MonoBehaviour
{
    [SerializeField]
    [Range(0f,1f)]
    float percent;
    RectTransform rt;

    private void Awake() {
        rt = GetComponent<RectTransform>();
    }

    public void SetBar(float p) {
        rt.offsetMax = new((p - 1) * transform.parent.GetComponent<RectTransform>().rect.width, rt.sizeDelta.y);
    }
}
