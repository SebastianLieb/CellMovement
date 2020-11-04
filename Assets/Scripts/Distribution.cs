using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IDistribution<T>
{
    T Eval();
    T Eval(int seed);
    T Eval(int seed, ref List<IDistribution<T>> path);
    float GetWeight();
}

[Serializable]
public class DistributionNode<T> : IDistribution<T>
{
    public List<IDistribution<T>> distributions = new List<IDistribution<T>>();
    public float weight = 1.0f;

    public T Eval()
    {
        return Eval(Random.Range(0,1048576));
    }

    List<IDistribution<T>> dummyPath = new List<IDistribution<T>>();
    public T Eval(int seed)
    {
        dummyPath.Clear();
        return Eval(seed, ref dummyPath);
    }

    public T Eval(int seed,ref List<IDistribution<T>> path)
    {
        float weightSum = 0.0f;
        foreach (IDistribution<T> d in distributions)
        {
            weightSum += d.GetWeight();
        }
        Random.InitState(seed);
        int nextSeed = Random.Range(0, int.MaxValue);
        float random = Random.Range(0, weightSum);
        foreach (IDistribution<T> d in distributions)
        {
            if (d.GetWeight()>random){
                path.Add(d);
                return d.Eval(nextSeed, ref path);
            }
            else
            {
                random -= d.GetWeight();
            }
        }
        return default;
    }

    public float GetWeight()
    {
        return weight;
    }
}

[Serializable]
public class DistributionLeaf<T> : IDistribution<T>
{
    public float weight = 1.0f;
    public T value;

    public DistributionLeaf(T value, float weight = 1.0f){
        this.value = value;
        this.weight = weight;
    }

    public T Eval()
    {
        return value;
    }

    public T Eval(int seed)
    {
        return value;
    }

    public T Eval(int seed, ref List<IDistribution<T>> path)
    {
        return value;
    }

    public float GetWeight()
    {
        return weight;
    }
}
