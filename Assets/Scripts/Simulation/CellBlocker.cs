using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBlocker : MonoBehaviour
{
    public float r = 5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(r*2f, r* 2f, r * 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Block(Cell c)
    {
        foreach(OriginPoint op in c.originPoints)
        {
            Vector3 diff = op.p - transform.position;
            float distance = diff.magnitude;
            if (distance < r)
            {
                diff *= (r - distance) / distance;
                op.p += diff;
                op.lifeTime -= 1f;
            }
        }
    }
}
