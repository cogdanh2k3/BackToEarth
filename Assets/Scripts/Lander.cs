using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    public event EventHandler OnLeftForce;
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;

    private Rigidbody2D landerRigidbody2D;
    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = +100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);

        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(!collision2D.gameObject.TryGetComponent(out LandingPan landingPan))
        {
            Debug.Log("Crashed on the Terrain!");
            return;
        }

        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            Debug.Log("Landed too hard!");
            return;
        }

        float dotProduct = Vector2.Dot(Vector2.up, transform.up);
        float minDotProduct = 0.9f;

        if(dotProduct < minDotProduct)
        {
            Debug.Log("Landed on a too steep angle!");
            return;
        }

        Debug.Log("Successful landed!");

        float maxAngleScore = 100f;
        float scoreDotProductMultiplier = 10f;
        float landingAngleScore = maxAngleScore - (Mathf.Abs(dotProduct - 1f) * scoreDotProductMultiplier * maxAngleScore); ;

        float maxSoftLandingScore = 100f;
        float landingSoftlyScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxSoftLandingScore;

        Debug.Log("Landing Angle Score: " + landingAngleScore);
        Debug.Log("Landing Softly Score: " + landingSoftlyScore);

        int score = Mathf.RoundToInt((landingAngleScore + landingSoftlyScore) * landingPan.GetScoreMultiplier());
        Debug.Log("Score : " + score);
    }
}
