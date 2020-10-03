using System.Collections.Generic;
using Assets.Scripts.ProceduralSceneGeneration;
using UnityEngine;

public abstract class WallGenerationWidget : MonoBehaviour
{
    public abstract void AddWalls(WallsSpecification specification, FloorSpecification floorSpecification);
}