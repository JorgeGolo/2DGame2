using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{

    public GameObject textContainer;
    public GameObject textPrefab;
    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Update()
    {
        foreach (FloatingText txt in floatingTexts)
        {
            txt.UpdateFloatingText();
        }
    }
    public void Show(
        string msg, 
        int fontsize, 
        Color color, 
        Vector3 position, 
        Vector3 motion, 
        float duration)
    {
        FloatingText floatingtext = GetFloatingText();
        floatingtext.txt.text = msg;
        floatingtext.txt.fontSize = fontsize;
        floatingtext.txt.color = color;
        // transform wolrd space to screen space
        // so we can use it in the UI
        floatingtext.txt.transform.position = Camera.main.WorldToScreenPoint(position);
        
        floatingtext.motion = motion;
        floatingtext.duration = duration;
        floatingtext.Show();
    }
    private FloatingText GetFloatingText()
    {
        // find a ft not active
        FloatingText txt = floatingTexts.Find(t => t.active);

        // last line is a resume of:
        /*
        for (var i = 0; i < floatingTexts.Count; i++)
        {
            if(!floatingTexts[i].active)
        }
        */

        if(txt == null)
        {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();

            floatingTexts.Add(txt);
        }
        return txt;
    }
 
}
