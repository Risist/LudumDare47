using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Ai.SenseSight.PerformSearch_Coroutine(0.25f));
    }
}
