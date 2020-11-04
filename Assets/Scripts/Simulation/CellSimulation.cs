using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSimulation : MonoBehaviour
{
    [HideInInspector]
    public List<Cell> cells = new List<Cell>();
    List<CellBlocker> cellBlockers = new List<CellBlocker>();
    public GameObject cellPrefab;
    public GameObject cellBlockerPrefab;

    public bool track = false;
    public Tracking tracking;

    public List<Boundary> boundaries = new List<Boundary>();

    public bool simulating = true;
    

    public void AddCell(Vector3 p)
    {
        int numberOfOriginPoints = Mathf.RoundToInt(OriginPoint.initialLifeTime / (Cell.originPointSpawnTimeMax+Cell.originPointSpawnTimeMin)*2.0f);
        Cell cell = Instantiate(cellPrefab,p,Quaternion.identity).GetComponent<Cell>();
        cell.id = ++Cell.idGen;
        cell.transform.position = p;
        cell.center = p;
        for (int i = 0; i < numberOfOriginPoints; ++i)
        {
            OriginPoint op = cell.SpawnOriginPoint(Mathf.PI*2/numberOfOriginPoints*i);
            op.lifeTime *= Random.Range(0f, 1f);
        }
        cell.originPointSpawnTimer += Random.Range(Cell.originPointSpawnTimeMin, Cell.originPointSpawnTimeMax);
        cell.SimulationUpdate(0.001f);
        cells.Add(cell);
    }

    void AddCellBlocker(Vector3 p, float radius = 10)
    {
        CellBlocker cellBlocker = Instantiate(cellBlockerPrefab).GetComponent<CellBlocker>();
        cellBlocker.r = radius;
        cellBlockers.Add(cellBlocker);
    }

    void Apply2DConstraint()
    {
        foreach (Cell cell in cells)
        {
            foreach(OriginPoint op in cell.originPoints)
            {
                op.p.z = 0;
                foreach(GripPoint gp in op.gripPoints)
                {
                    gp.p.z = 0;
                }
            }
        }
    }

    void ApplyCellBlockers()
    {
        foreach(CellBlocker cb in cellBlockers)
        {
            foreach(Cell c in cells)
            {
                cb.Block(c);
            }
        }
    }

    public void UpdateConnectionsAndSolveCollisions (Cell c0)
    {
        List<OriginPoint> ops0 = new List<OriginPoint>();
        ops0.AddRange(c0.originPoints);
        Collider[] colliders;
        Collider myCollider = c0.GetComponent<Collider>();
        foreach (OriginPoint op in ops0)
        {
            if (op.connected!=null)
                continue;
            colliders = Physics.OverlapSphere(op.p, 0.001f);
            foreach (Collider collider in colliders)
            {
                if (collider == myCollider)
                    continue;
                Cell c1 = collider.GetComponent<Cell>();
                if (c0.GetNumberOfConnections(c1) >= Cell.maxNumberOfConnections || c1.originPoints.Count >= Cell.maxNumberOfOriginPoints)
                {
                    continue;
                }
                if(c1)
                    ConnectOriginPointToCell(op, c1);
            }
        }
    }

    public void ConnectOriginPointToCell(OriginPoint op, Cell c)
    {
        OriginPoint originPoint = c.SpawnOriginPoint();
        originPoint.lifeTime = op.lifeTime*2;//originPoint will be deleted anyways when the connector is deleted
        op.lifeTime *= OriginPoint.lifeTimeConnected / OriginPoint.initialLifeTime;
        originPoint.p = op.p;
        originPoint.connected = op;
        op.connected = originPoint;
        foreach (GripPoint gp in op.gripPoints)
            gp.lifeTime *= GripPoint.lifeTimeConnected / GripPoint.initialLifeTime;
        foreach (GripPoint gp in originPoint.gripPoints)
            gp.lifeTime *= GripPoint.lifeTimeConnected / GripPoint.initialLifeTime;
    }

    public void UpdateCellCellInteractions()
    {
        foreach (Cell c0 in cells)
        {
            UpdateConnectionsAndSolveCollisions(c0);
        }
    }

    void ApplyBoundaries()
    {
        foreach(Boundary b in boundaries)
        {
            foreach (Cell cell in cells)
            {
                foreach (OriginPoint op in cell.originPoints)
                {
                    if (!b.IsInside(op.p))
                    {
                        op.p = b.SolveBoundary(op.p);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!simulating)
            return;
        ApplyBoundaries();
        foreach (Cell cell in cells)
        {
            cell.SimulationUpdate(Time.fixedDeltaTime);
        }
        Apply2DConstraint();
        ApplyCellBlockers();
        UpdateCellCellInteractions();
        if(tracking!=null)
            tracking.Track(this);
    }

    public void ResetSimulation()
    {
        foreach (Cell c in cells)
        {
            Destroy(c.gameObject);
        }
        cells.Clear();
    }
}
