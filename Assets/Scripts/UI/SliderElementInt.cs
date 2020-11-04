using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderElementInt : UIElement
{
    public Text fieldLabel;
    public Slider slider;
    public InputField inputField;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        slider.value = (int)value;
        inputField.text = slider.value.ToString();
        //fieldLabel.text = fieldName;
    }

    public void OnChangeInputField()
    {
        try
        {
            slider.value = int.Parse(inputField.text);
            value = (int)slider.value;
            PushValue();
        }
        catch
        {
            Debug.Log("error pasing int");
        }
    }

    public void OnChangeSlider()
    {
        value = (int)slider.value;
        PushValue();
        inputField.text = slider.value.ToString();
    }
}
