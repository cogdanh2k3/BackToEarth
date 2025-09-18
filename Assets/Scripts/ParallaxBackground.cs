using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxMultipler = .1f;

    private Vector2 lastCameraPosition;

    private void Start()
    {
        lastCameraPosition = Camera.main.transform.position;
    }

    private void LateUpdate()
    {
        Vector2 newCameraPosition = Camera.main.transform.position;

        Vector2 positionDelta = newCameraPosition - lastCameraPosition;

        transform.position += (Vector3)positionDelta * parallaxMultipler;

        lastCameraPosition = newCameraPosition;
    }
}
