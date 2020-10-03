using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomGeneratorSingleton : MonoBehaviour
{
    private Random _random;

    void Awake()
    {
        _random = new Random(0);
    }

    public int RandomInt(int max)
    {
        return _random.Next(max);
    }

    public T RandomElement<T>(List<T> elementsToChooseFrom)
    {
        return elementsToChooseFrom[RandomInt(elementsToChooseFrom.Count)];
    }
}