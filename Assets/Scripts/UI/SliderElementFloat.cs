using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderElementFloat : UIElement
{
    public Text fieldLabel;
    public Slider slider;
    public InputField inputField;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        slider.value = (float)value;
        inputField.text = slider.value.ToString("0.00");
        //fieldLabel.text = fieldName;
    }

    public void OnChangeInputField()
    {
        try
        {
            slider.value = float.Parse(inputField.text);
            value = slider.value;
            PushValue();
        }
        catch
        {

        }
    }

    public void OnChangeSlider()
    {
        value = slider.value;
        PushValue();
        inputField.text = slider.value.ToString("0.00");
    }
}
