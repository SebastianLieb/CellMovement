using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideUIButton : MonoBehaviour
{
    public GameObject panel;
    Vector2 pos;
    bool hidden = false;
    RectTransform rect;
    Text text;

    private void Start()
    {
        rect = transform.GetComponent<RectTransform>();
        text = GetComponentInChildren<Text>();
        pos = rect.anchoredPosition;    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        if (hidden)
        {
            rect.anchoredPosition = pos;
            panel.SetActive(true);
            text.text = "^";
            hidden = false;
        }
        else
        {
            rect.anchoredPosition = new Vector3(rect.anchoredPosition.x,0);
            panel.SetActive(false);
            text.text = "˅";
            hidden = true;
        }
    }
}
