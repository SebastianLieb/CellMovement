using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularBoundary2D : PolygonBoundary2D
{
    public int pointCount = 20;
    public float radius = 10;
    // Start is called before the first frame update
    public new void Start()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        Vector3[] p = new Vector3[pointCount];
        for(int i = 0; i < pointCount; ++i)
        {
            float angle = -Mathf.PI*2f / pointCount*i;
            p[i] = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * radius+transform.position;
        }

        lr.positionCount = pointCount;
        lr.SetPositions(p);
        base.Start();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
