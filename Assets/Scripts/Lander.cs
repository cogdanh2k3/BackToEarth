using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    public static Lander instance { get; private set; }

    public event EventHandler OnLeftForce;
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnLandedEventArgs> OnLanded;

    public class OnLandedEventArgs : EventArgs
    {
        public int score;
    }

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount = 10f;
    private void Awake()
    {
        instance = this;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        if(fuelAmount <= 0)
        {
            return;
        }

        if (Keyboard.current.upArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            ConsumeFuel();
        }

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

        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            score = score
        });
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            fuelPickup.DestroySelf();
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }



}
