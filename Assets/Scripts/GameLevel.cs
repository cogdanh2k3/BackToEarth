using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private Transform landerSpawnPosition;

    public int GetLevel()
    {
        return levelNumber;
    }

    public Vector3 GetLanderSpawnPosition()
    {
        return landerSpawnPosition.position;
    }
}
