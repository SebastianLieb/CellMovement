using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OriginPointConnectionModel
{
    ToCenterOfCell,
    ToSurfaceOfCell,
    ToMidPoint,
}

public class OriginPoint : MonoBehaviour //Filopodia
{
    public GameObject gripPointPrefab;

    public Vector3 p;
    public float distanceToCellCenter;
    public float flowForce = 0.02f;
    public List<GripPoint> gripPoints = new List<GripPoint>();
    public OriginPoint connected = null;

    public float gripPointSpawnTimer = 0.1f;
    public float maxGripPoints = 16;
    public float currentgGripPointSpawnTimer = 0;

    public static float initialLifeTime = 15.0f;
    public static float lifeTimeConnected = 15.0f;
    public float lifeTime;

    public bool exceededMaxLength = false;
    public static float distanceCutOff = 6;

    [HideInInspector]
    public Cell parentCell;
    public OriginPointConnectionModel connectionModel = OriginPointConnectionModel.ToSurfaceOfCell;

    public void InitLifeTime()
    {
        lifeTime = initialLifeTime+Random.Range(-1f, 1f); //randomize lifetime
    }

    public float GetFocalAdhesion() //the sum over all grip points
    {
        float fa = 0f;
        foreach(GripPoint gp in gripPoints)
        {
            fa += gp.focalAdhesion;
        }
        return fa;
    }

    public void UpdateGripPointFlow(float dt)
    {
        //create and destroy grippoints
        currentgGripPointSpawnTimer -= dt;
        if (currentgGripPointSpawnTimer < 0)
        {
            if (connected==null || GripPoint.lifeTimeConnected > 0.01)
            {
                GripPoint gp = Instantiate(gripPointPrefab).GetComponent<GripPoint>();
                gp.InitLifeTime();
                gp.transform.SetParent(transform);
                gp.p = p;
                gripPoints.Add(gp);
                if (connected)
                {
                    gp.lifeTime *= GripPoint.lifeTimeConnected;
                }
            }
            currentgGripPointSpawnTimer = gripPointSpawnTimer;
        }

        List<GripPoint> remove = new List<GripPoint>();
        int gps = gripPoints.Count;
        foreach(GripPoint gp in gripPoints)
        {
            if (gp.lifeTime < 0 || gps > maxGripPoints)
            {
                remove.Add(gp);
                --gps;
            }
        }
        foreach (GripPoint r in remove)
        {
            gripPoints.Remove(r);
            Destroy(r.gameObject);
        }

        //add flow force
        Vector3 flowDir = p - parentCell.center;
        flowDir.Normalize();

        if(connected==null)
            p += flowDir * flowForce;
        else
        {
            if (connectionModel == OriginPointConnectionModel.ToMidPoint)
            {
                Vector3 c = (p + connected.p) * 0.5f;
                Vector3 diff = c - p;
                p += diff;
                connected.p -= diff;
            }
            else if (connectionModel == OriginPointConnectionModel.ToSurfaceOfCell)
            {
                Vector3 diff = p - connected.parentCell.center;
                float distance = diff.magnitude;
                if (distance > 0.001f)
                {
                    p -= diff*(distance - connected.parentCell.r) / distance;
                }
            }
            else
            {
                p = connected.parentCell.center;
                connected.p = parentCell.center;
            }
        }
    }

    public void UpdateMembraneForce()
    {
        //calculate force, calculate focal adhesion, apply forces to originPoint + all gripPoints
        Vector3 membraneForce = parentCell.center - p;
        float distanceToCenter = membraneForce.magnitude;
        float restSurfaceArea = 4 * Mathf.PI * parentCell.r * parentCell.r;
        if (parentCell.summedDistancesToCenter < 0.001f)
            return;
        float m = distanceToCenter / parentCell.summedDistancesToCenter * Cell.membraneStiffness * (parentCell.surfaceArea-restSurfaceArea);
        membraneForce.Normalize();

        // force has to overcome grip
        float focalAdhesion = GetFocalAdhesion(); 
        m = Mathf.Max(0f, m - focalAdhesion);

        membraneForce *= m;

        // add movement to origian point and all gripPoints
        p += membraneForce;
        if (connected == null)
        {
            foreach (GripPoint gp in gripPoints)
            {
                gp.p += membraneForce;
            }
        }
    }

    public void SimulationUpdate(float dt)
    {
        lifeTime -= Time.fixedDeltaTime;
        transform.position = p;

        Vector3 diff = p - parentCell.center;
        if (!exceededMaxLength && Mathf.Sqrt(Vector3.Dot(diff, diff)) > parentCell.r + distanceCutOff / Mathf.Sqrt(Cell.membraneStiffness))
        {
            exceededMaxLength = true;
            lifeTime = 1f;
        }
        foreach(GripPoint gp in gripPoints)
        {
            gp.SimulationUpdate(dt);
        }
    }
}
