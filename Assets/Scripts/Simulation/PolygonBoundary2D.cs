using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonBoundary2D : Boundary
{
    public bool copyFromLineRenderer = true;
    public List<Vector2> points = new List<Vector2>();

    // Start is called before the first frame update
    public void Start()
    {
        if (copyFromLineRenderer)
        {
            LineRenderer lr = GetComponent<LineRenderer>();
            Vector3[] points = new Vector3[lr.positionCount];
            lr.GetPositions(points);
            foreach(Vector3 p in points)
            {
                this.points.Add(p);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePoints = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,Camera.main.nearClipPlane));
    }

    public float GetWindingNumber(Vector2 p)
    {
        if (points.Count < 3)
            return 0;
        float angle = 0; 
        for(int i = 0;i<points.Count-1;++i)
        {
            angle += Vector2.SignedAngle(points[i] - p, points[i+1] - p);
        }
        angle += Vector2.SignedAngle(points[points.Count-1] - p, points[0] - p);
        return angle;
    }

    public override bool IsInside(Vector3 p)
    {
        if (points.Count < 3)
            return true;
        return GetWindingNumber(p)>180;
    }

    public static float PointToLineDistance(Vector2 p ,Vector2 l0, Vector2 l1)
    {
        return (p-ClosestPointToLine(p,l0,l1)).magnitude;
    }

    public static Vector2 ClosestPointToLine(Vector2 p, Vector2 l0, Vector2 l1)
    {
        Vector2 d = l1 - l0;

        if (Vector2.Dot(p - l1, d) > 0)
            return l1;

        if (Vector2.Dot(p - l0, -d) > 0)
            return l0;

        d.Normalize();
        float t = Vector2.Dot(d, (p - l0));
        return l0 + d*t;
    }

    public override Vector3 SolveBoundary(Vector3 p)
    {
        float minDistance = float.MaxValue;
        Vector2 l0 = Vector2.zero;
        Vector2 l1 = Vector2.zero;
        for(int i = 0; i < points.Count; ++i)
        {
            float d = PointToLineDistance(p, points[i], points[(i + 1) % points.Count]);
            if (d < minDistance)
            {
                minDistance = d;
                l0 = points[i];
                l1 = points[(i + 1) % points.Count];
            }
        }
        return ClosestPointToLine(p, l0, l1);
    }
}
