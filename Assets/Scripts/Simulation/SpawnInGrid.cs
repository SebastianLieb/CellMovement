using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInGrid : MonoBehaviour
{
    public CellSimulation cellSimulation;

    public int cellCountX;
    public int cellCountY;

    public float cellDistance;
    public bool honeyComb = true;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Spawn()
    {
        for(int y = 0; y < cellCountY; ++y)
        {
            for(int x = 0; x < cellCountX; ++x)
            {
                float px = cellDistance * x;
                float py = cellDistance * y;
                if (honeyComb)
                {
                    py /= Mathf.Sqrt(2);
                    if (y % 2 == 0)
                    {
                        px += cellDistance / Mathf.Sqrt(2);
                    }
                }
                cellSimulation.AddCell(transform.position+new Vector3(px,py, 0));
            }
        }
    }
}
