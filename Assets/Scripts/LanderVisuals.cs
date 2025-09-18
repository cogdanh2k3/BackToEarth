using UnityEngine;

public class LanderVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterPS;
    [SerializeField] private ParticleSystem middleThrusterPS;
    [SerializeField] private ParticleSystem rightThrusterPS;
    [SerializeField] private GameObject landerExplosionVFX;

    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();

        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;

        SetEnabledThrusterPS(leftThrusterPS, false);
        SetEnabledThrusterPS(middleThrusterPS, false);
        SetEnabledThrusterPS(rightThrusterPS, false);
    }

    private void Start()
    {
        lander.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.BeShot:
            case Lander.LandingType.TooFast:
            case Lander.LandingType.TooSteep:
            case Lander.LandingType.WrongArea:
                Instantiate(landerExplosionVFX, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
        }
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterPS(leftThrusterPS, false);
        SetEnabledThrusterPS(middleThrusterPS, false);
        SetEnabledThrusterPS(rightThrusterPS, false);
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterPS(leftThrusterPS, true);
        SetEnabledThrusterPS(middleThrusterPS, false);
        SetEnabledThrusterPS(rightThrusterPS, false);
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterPS(leftThrusterPS, true);
        SetEnabledThrusterPS(middleThrusterPS, true);
        SetEnabledThrusterPS(rightThrusterPS, true);
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterPS(rightThrusterPS, true);
        SetEnabledThrusterPS(middleThrusterPS, false);
        SetEnabledThrusterPS(leftThrusterPS, false);
    }

    private void SetEnabledThrusterPS(ParticleSystem particleSystem, bool isEnabled)
    {
        ParticleSystem.EmissionModule emissonModule = particleSystem.emission;
        emissonModule.enabled = isEnabled;
    }
}
