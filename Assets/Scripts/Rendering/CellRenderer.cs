using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cell))]
public class CellRenderer : MonoBehaviour
{
    Cell cell;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        cell = GetComponent<Cell>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cell.originPoints.Count < 2)
            return;

        transform.position = Vector3.Lerp(transform.position,cell.center,Mathf.Min(1.0f,Time.deltaTime*10));
    }
}
