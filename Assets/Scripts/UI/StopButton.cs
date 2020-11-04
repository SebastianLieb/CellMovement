using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopButton : MonoBehaviour
{
    public CellSimulation sim;
    public Image image;

    public Sprite play;
    public Sprite pause;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        sim.simulating = !sim.simulating;
        image.sprite = sim.simulating ? pause : play;
    }
}
