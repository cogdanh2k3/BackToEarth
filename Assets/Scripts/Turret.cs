using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private GameObject alarmLight;
    [SerializeField] private GameObject turretGun;
    [SerializeField] private GameObject turretBullet;
    [SerializeField] private GameObject shootPoint;


    private bool isDetected = false;
    private SpriteRenderer alarmLightRenderer;
    private Vector2 turretDirection;

    private float fireRate = 0.5f;
    private float reloadBullet = 1f;
    private float forceShoot = 300f;

    private void Awake()
    {
        alarmLightRenderer = alarmLight.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, Lander.instance.transform.position) <= range) 
        {
            turretDirection = Lander.instance.transform.position - transform.position;
            turretGun.transform.up = turretDirection;
            RaycastHit2D raycast2D = Physics2D.Raycast(transform.position, turretDirection, range);

            if (raycast2D.collider != null && raycast2D.collider.gameObject == Lander.instance.transform.gameObject)
            {
                if(isDetected == false)
                {
                    isDetected = true;
                    alarmLightRenderer.color = Color.red;
                }

                if (Time.time >= reloadBullet)
                {
                    float oneSecond = 1f;
                    Shoot();
                    reloadBullet = Time.time + oneSecond / fireRate;
                }

            }
            else
            {
                if(isDetected == true)
                {
                    isDetected = false;
                    alarmLightRenderer.color = Color.green;

                }
            }

        }
        else
        {
            if(isDetected == true)
            {
                isDetected = false;
                alarmLight.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletShot = Instantiate(turretBullet, shootPoint.transform.position, Quaternion.identity);
        Vector2 normalizedDirection = turretDirection.normalized;
        bulletShot.GetComponent<Rigidbody2D>().AddForce(normalizedDirection * forceShoot);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
