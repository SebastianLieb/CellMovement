using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenarios : MonoBehaviour
{
    public CellSimulation sim;

    public List<GameObject> scenario0 = new List<GameObject>();
    public List<GameObject> scenario1 = new List<GameObject>();

    public List<GameObject> loadedObjects = new List<GameObject>();


    public void LoadScenario0()
    {
        LoadScenario(scenario0);
    }

    public void LoadScenario1()
    {
        LoadScenario(scenario1);
    }

    public void LoadScenario(List<GameObject> objects)
    {
        sim.ResetSimulation();
        sim.boundaries.Clear();
        foreach (GameObject obj in loadedObjects)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in objects)
        {
            GameObject instance = Instantiate(obj);
            loadedObjects.Add(instance);
            if (instance.GetComponent<Boundary>())
            {
                sim.boundaries.Add(instance.GetComponent<Boundary>());
            }
            if (instance.GetComponent<SpawnInGrid>())
            {
                instance.GetComponent<SpawnInGrid>().cellSimulation = sim;
            }
        }
    }
}
