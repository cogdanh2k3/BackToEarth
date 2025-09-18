using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroySelf();
    }
}
