using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomGeneratorSingleton : MonoBehaviour
{
    private static Random _random;

    public static Random GetRandom()
    {
        if (_random == null)
        {
            _random = new Random(0);
        }

        return _random;
    }

    void Awake()
    {
        if (_random == null)
        {
            _random = new Random(0);
        }
    }

    public int RandomInt(int max)
    {
        return _random.Next(max);
    }

    public int RandomInt(int min, int max)
    {
        return RandomInt(max - min) + min;
    }

    public T RandomElement<T>(List<T> elementsToChooseFrom)
    {
        return elementsToChooseFrom[RandomInt(elementsToChooseFrom.Count)];
    }

    public bool RandomBool(float trueProbability)
    {
        return _random.NextDouble() < trueProbability;
    }

    public float RandomFloat(float max)
    {
        return (float) (_random.NextDouble() * max);
    }

    public float RandomFloat(float min, float max)
    {
        return min + RandomFloat(max - min);
    }
}