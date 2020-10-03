using UnityEngine;

public abstract class FloorGenerationWidget : MonoBehaviour
{
    public abstract bool? FloorIsPresent(Vector2Int queriedPosition, Vector2Int roomSize);
}