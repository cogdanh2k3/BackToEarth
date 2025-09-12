using Unity.Cinemachine;
using UnityEngine;

public class CineCameraZoom2D : MonoBehaviour
{
    private const float NORMAL_ORTHO_SIZE = 10f;

    public static CineCameraZoom2D instance;

    [SerializeField] private CinemachineCamera cinemachineCamera;

    private float targetOrthoSize = 10f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        float zoomSpeed = 2f;
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize,targetOrthoSize, Time.deltaTime * zoomSpeed);
    }

    public void SetTargetOrthoSize(float targetOrthoSize)
    {
        this.targetOrthoSize = targetOrthoSize;
    }

    public void SetNormalOrthoSize()
    {
        SetTargetOrthoSize(NORMAL_ORTHO_SIZE);
    }
}
