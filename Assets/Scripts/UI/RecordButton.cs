using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordButton : MonoBehaviour
{
    public CellSimulation simulation;
    public Image recordIcon;
    public InputField frameSkipInputField;

    // Update is called once per frame
    void Update()
    {
        try { 
            int frameSkip = int.Parse(frameSkipInputField.text);
            if (frameSkip < 1) {
                frameSkip = 1;
                frameSkipInputField.text = "1";
            }
            simulation.tracking.frameSkip = frameSkip;
        }
        catch
        {

        }
        if (simulation.track)
        {
            recordIcon.enabled = ((int)(Time.time * 8)) % 2 == 0 ? true:false;
        }
        else
        {
            recordIcon.enabled = true;
        }
    }

    public void OnPress()
    {
        simulation.track = !simulation.track;
        if (!simulation.track)
            simulation.tracking.SaveToFile();
    }
}
