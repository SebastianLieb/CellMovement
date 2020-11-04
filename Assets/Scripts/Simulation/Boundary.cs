using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boundary : MonoBehaviour
{
    public abstract bool IsInside(Vector3 p);
    public abstract Vector3 SolveBoundary(Vector3 p);
}
