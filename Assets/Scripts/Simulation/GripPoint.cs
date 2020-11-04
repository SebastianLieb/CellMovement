using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripPoint : MonoBehaviour //Focal adhesions
{
    public static float initialLifeTime = 1.0f;
    public static float lifeTimeConnected = 0.0f;

    public Vector3 p;
    public float focalAdhesion = 1f;
    public float lifeTime = initialLifeTime;

    public void SimulationUpdate(float dt)
    {
        lifeTime -= dt;
        transform.position = p;
    }

    public void InitLifeTime()
    {
        lifeTime = initialLifeTime;
    }
}
