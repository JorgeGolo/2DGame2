using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public bool active;
    public GameObject go;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public void Show()
    {
        active = true;
        lastShown = Time.time; // now1
        go.SetActive(active);

    }
    public void Hide()
    {
        active = false;
        go.SetActive(active);

    }

    public void UpdateFloatingText()
    {
        if(!active)
        return;

        // 10 - 7 = 2
        // If now minus "last showed" is equal to duration...
        if(Time.time - lastShown > duration)
        Hide();

        // moving
        go.transform.position += motion * Time.deltaTime;
    }
}
