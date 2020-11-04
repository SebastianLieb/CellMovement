using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class SpawnCellsRandomly : MonoBehaviour
{
    public int numberOfCells = 10;
    public CellSimulation cellSimulation;

    public Vector2 areaMin;
    public Vector2 areaMax;

    void Start()
    {
        for (int i = 0; i < numberOfCells; ++i)
        {
            cellSimulation.AddCell(new Vector3(Random.Range(areaMin.x, areaMax.y), Random.Range(areaMin.x, areaMax.y), 0));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cellSimulation.AddCell(new Vector3(Random.Range(areaMin.x, areaMax.y), Random.Range(areaMin.x, areaMax.y), 0));
        }
    }
}
