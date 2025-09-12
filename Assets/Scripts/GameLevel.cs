using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private Transform landerSpawnPosition;
    [SerializeField] private Transform cameraStartTargetPosition;
    [SerializeField] private float zoomedOutOrthSize;

    public int GetLevel()
    {
        return levelNumber;
    }

    public Vector3 GetLanderSpawnPosition()
    {
        return landerSpawnPosition.position;
    }

    public Transform GetCameraStartTargetPosition()
    {
        return cameraStartTargetPosition;
    }

    public float GetZoomedOutOrthSize()
    {
        return zoomedOutOrthSize;
    }
}
