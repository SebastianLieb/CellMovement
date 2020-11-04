using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;

public class LocalizationOptions : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
