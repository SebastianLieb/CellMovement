using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    public List<string> trackingFile = new List<string>();

    int time = 0;
    public int frameSkip = 2;

    // Start is called before the first frame update
    public void Track(CellSimulation sim)
    {
        if (time % frameSkip == 0)
        {
            foreach (Cell c in sim.cells)
            {
                trackingFile.Add(time + ";" + c.id + ";" + c.center.x.ToString("0.00", CultureInfo.InvariantCulture) + ";" + c.center.y.ToString("0.00", CultureInfo.InvariantCulture));
            }
        }
        ++time;
    }

    public void SaveToFile()
    {
        int i;
        for(i = 0; i < 100; ++i)
        {
            if (!File.Exists("tracking" + i + ".txt"))
                break;
        }
        File.WriteAllLines("tracking"+i+".txt", trackingFile.ToArray());
        trackingFile.Clear();
    }
}
