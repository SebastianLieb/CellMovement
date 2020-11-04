using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripPointRenderer : MonoBehaviour
{
    GripPoint gp;
    // Start is called before the first frame update
    void Start()
    {
        gp = GetComponent<GripPoint>();
    }
}
