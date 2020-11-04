using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginPointRenderer : MonoBehaviour
{
    public Color color;
    public Color connectedColor;
    public Color exceedMaxLengthColor;

    OriginPoint op;
    LineRenderer lr;
    SpriteRenderer sr;

    Vector3[] positions = new Vector3[] {Vector3.zero,Vector3.zero};

    // Start is called before the first frame update
    void Start()
    {
        op = GetComponent<OriginPoint>();
        lr = GetComponent<LineRenderer>();
        sr = GetComponent<SpriteRenderer>();
        lr.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = op.p;
        if (lr)
        {
            positions[0] = transform.position;
            positions[1] = op.parentCell.center;
            lr.SetPositions(positions);
            lr.startColor = color;
            lr.endColor = lr.startColor;

            if (op.exceededMaxLength)
            {
                if (((int)(Time.time * 8)) % 2 == 0)
                {
                    color = exceedMaxLengthColor;
                    connectedColor = exceedMaxLengthColor;
                }
                else
                {
                    color.a = 0.2f;
                    connectedColor.a = 0.5f;
                }
            }
        }
        if (op.connected)
        {
            sr.color = connectedColor;
        }
        else
        {
            sr.color = color;
        }
    }
}
  