using UnityEngine;

public abstract class FloorGenerationWidget : MonoBehaviour
{
    public abstract FloorGenerationOutcome FloorIsPresent(Vector2Int queriedPosition, Vector2Int roomSize);

    void Update(){}

    public enum FloorGenerationOutcome
    {
        None, On, Off, Flip
    }
}