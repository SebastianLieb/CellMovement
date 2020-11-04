using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBoundary2D : Boundary
{
    public Vector2 min;
    public Vector2 max;

    public override bool IsInside(Vector3 p)
    {
        return p.x > min.x && p.x < max.x && p.y > min.y && p.y < max.y;
    }

    public override Vector3 SolveBoundary(Vector3 p)
    {
        Vector2 correctedP = p;
        if (correctedP.x < min.x)
            correctedP.x = min.x;
        if (correctedP.x > max.x)
            correctedP.x = max.x;
        if (correctedP.y < min.y)
            correctedP.y = min.y;
        if (correctedP.y > max.y)
            correctedP.y = max.y;
        return correctedP;
    }
}
