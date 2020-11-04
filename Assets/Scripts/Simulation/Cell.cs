using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public List<OriginPoint> originPoints = new List<OriginPoint>();
    public GameObject originPointPrefab;

    //the cell radius. Used to calculate the membran resting state and membrane force.
    public float r = 4.0f;

    public Vector3 center;
    public float surfaceArea;
    public float summedDistancesToCenter;

    //coefficient that determines the force acted on origin points by the membrane
    public static float membraneStiffness = 0.04f;

    //the current time until next originPoint is spawned.
    public float originPointSpawnTimer = 0;

    //after a originPoint is spawned the originPointSpawnTimer will be set to a random value between originPointSpawnTimeMin and originPointSpawnTimeMax
    public static float originPointSpawnTimeMin = 0.5f;
    public static float originPointSpawnTimeMax = 0.8f;

    //if this limit is reached no more originPoints are spawned until some die again.
    public static int maxNumberOfOriginPoints = 64;

    //this is the maximum number of connection that can be build between two cells, if a cell is connected to e.g. two other cells it can have max maxNumberOfConnecetions*2 connections
    public static int maxNumberOfConnections = 64;

    public int id;
    public static int idGen = 0;

    public void UpdateCenter()
    {
        center = Vector3.zero;
        foreach(OriginPoint p in originPoints)
        {
            center += p.p;
        }
        if(originPoints.Count>0)
            center /= originPoints.Count;
    }

    public float UpdateSurfaceArea()
    {
        /*  this is just assuming that the surface around the origin points spans like a 4 sided quad
         *
         *          surfaceAreaWeight
         *          <--->
         *          _____
         *          | x |       x:originPoint
         *          |   |
         *      ____|   |____   <- surface
         */

        surfaceArea = 0f;
        float surfaceAreaWeight = 1f;
        foreach (OriginPoint p in originPoints)
        {
            float extrusionDistance = (p.p - center).magnitude - r;
            surfaceArea += 4 * surfaceAreaWeight * extrusionDistance;
        }
        return 1;
    }

    public void UpdateGripPointFlow(float dt)
    {
        UpdateCenter();
        foreach (OriginPoint op in originPoints)
        {
            op.UpdateGripPointFlow(dt);
        }
    }

    const int numberOfAreas = 32;
    const float scoringAngle = Mathf.PI * 2.0f / 4.0f; // 45 degree
    float GetSpawnScore(float angle)
    {
        float score = 0;
        foreach (OriginPoint op in originPoints)
        {
            Vector2 opDir = (op.p-center).normalized;
            float opAngle = Mathf.Atan2(opDir.y,opDir.x);
            float angleDiff = Mathf.Abs(opAngle - angle);
            if (angleDiff > Mathf.PI)
                angleDiff = Mathf.Abs(angleDiff-Mathf.PI * 2.0f);

            score -= Mathf.Pow(Mathf.Cos(Mathf.Min(angleDiff*Mathf.PI/scoringAngle,Mathf.PI*0.5f)),4.0f);
        }
        return score;
    }

    public OriginPoint SpawnOriginPoint()
    {
        float bestAngle = 0f;
        float bestAngleScore = float.MinValue;
        float randomness = Mathf.PI * 2.0f / numberOfAreas;

        for (int i = 0; i < numberOfAreas; ++i)
        {
            float angle = Mathf.PI * 2.0f / numberOfAreas * i + Random.Range(0f, randomness);
            float score = GetSpawnScore(angle);

            if (score > bestAngleScore)
            {
                bestAngleScore = score;
                bestAngle = angle;
            }
        }

        return SpawnOriginPoint(bestAngle);
    }

    public OriginPoint SpawnOriginPoint(float angle)
    {
        OriginPoint originPoint = Instantiate(originPointPrefab).GetComponent<OriginPoint>();
        originPoint.parentCell = this;
        originPoint.transform.SetParent(transform);
        originPoint.InitLifeTime();
        originPoint.p = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * r;
        originPoint.transform.position = originPoint.p;
        originPoints.Add(originPoint);
        return originPoint;
    }

    public void UpdateOriginPointRelationToCenter()
    {
        summedDistancesToCenter = 0;
        foreach (OriginPoint c in originPoints)
        {
            c.distanceToCellCenter = (c.p - center).magnitude;
            summedDistancesToCenter += c.distanceToCellCenter;
        }
    }

    public void UpdateMembraneForce()
    {
        //calculate force of membrane stretching back. This force is applyed to each (originpoint+grippoints)
        UpdateCenter();
        UpdateSurfaceArea();
        UpdateOriginPointRelationToCenter();
        foreach (OriginPoint o in originPoints)
        {
            o.UpdateMembraneForce();
        }
    }

    List<OriginPoint> remove = new List<OriginPoint>();
    public void UpdateOriginPointSpawning(float dt)
    {
        remove.Clear();
        foreach(OriginPoint op in originPoints)
        {
            if (op.lifeTime < 0)
            {
                if (op.connected != null) //destroy connection
                {
                    op.connected.lifeTime = -1;
                    op.connected.connected = null;

                }
                remove.Add(op);
            }
        }
        foreach(OriginPoint r in remove)
        {
            originPoints.Remove(r);
            Destroy(r.gameObject);
        }

        originPointSpawnTimer -= dt;
        if(originPointSpawnTimer < 0){
            if (maxNumberOfOriginPoints > originPoints.Count)
                SpawnOriginPoint();
            originPointSpawnTimer += Random.Range(originPointSpawnTimeMin, originPointSpawnTimeMax);
        }
    }

    public int GetNumberOfConnections(Cell other)
    {
        int connections = 0;
        foreach (OriginPoint op in originPoints)
        {
            if (!op.connected)
                continue;
            if (op.connected.parentCell == other)
            {
                ++connections;
            }
        }
        return connections;
    }

    public void SimulationUpdate(float dt)
    {
        UpdateOriginPointSpawning(dt);
        UpdateGripPointFlow(dt);
        UpdateMembraneForce();
        foreach(OriginPoint op in originPoints)
        {
            op.SimulationUpdate(dt);
        }
    }
}
