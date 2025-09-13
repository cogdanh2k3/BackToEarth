using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.7f;

    public static Lander instance { get; private set; }

    public event EventHandler OnLeftForce;
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs: EventArgs
    {
        public State state;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;

    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public int scoreMultiplier;
        public float landingSpeed;
        public float landingAngle;
    }

    public enum LandingType
    {
        Success,
        WrongArea,
        TooSteep,
        TooFast
    }

    public enum State
    {
        WaitingForStart,
        Normal,
        GameOver
    }

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount;
    private float fuelAmountMax = 10f;
    private State state;

    private void Awake()
    {
        instance = this;

        fuelAmount = fuelAmountMax;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;
    }
    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            default:
            case State.WaitingForStart:
                if (GameInput.instance.isUpActionPressed()||
                    GameInput.instance.isRightActionPressed() ||
                    GameInput.instance.isLeftActionPressed() ||
                    GameInput.instance.GetMovementInputVector2() != Vector2.zero)
                {
                    // Press
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }
                break;
            case State.Normal:
                if (fuelAmount <= 0)
                {
                    return;
                }

                if (GameInput.instance.isUpActionPressed() ||
                    GameInput.instance.isRightActionPressed() ||
                    GameInput.instance.isLeftActionPressed() ||
                    GameInput.instance.GetMovementInputVector2() != Vector2.zero)
                {
                    ConsumeFuel();
                }

                float gamePadDeadZone = .4f;
                if (GameInput.instance.isUpActionPressed() || GameInput.instance.GetMovementInputVector2().y > gamePadDeadZone)
                {
                    float force = 700f;
                    landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }
                if (GameInput.instance.isLeftActionPressed() || GameInput.instance.GetMovementInputVector2().x < -gamePadDeadZone)
                {
                    float turnSpeed = +100f;
                    landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);

                }
                if (GameInput.instance.isRightActionPressed() || GameInput.instance.GetMovementInputVector2().x > gamePadDeadZone)
                {
                    float turnSpeed = -100f;
                    landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);

                }
                break;
            case State.GameOver:
                break;
        }

        

        
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPan landingPan))
        {
            Debug.Log("Crashed on the Terrain!");

            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongArea,
                score = 0,
                scoreMultiplier = 0,
                landingSpeed = 0,
                landingAngle = 0
            });
            SetState(State.GameOver);
            return;

        }

        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            Debug.Log("Landed too hard!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFast,
                score = 0,
                scoreMultiplier = 0,
                landingSpeed = relativeVelocityMagnitude,
                landingAngle = 0
            });
            SetState(State.GameOver);
            return;
        }

        float dotProduct = Vector2.Dot(Vector2.up, transform.up);
        float minDotProduct = 0.9f;

        if (dotProduct < minDotProduct)
        {
            Debug.Log("Landed on a too steep angle!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteep,
                score = 0,
                scoreMultiplier = 0,
                landingSpeed = relativeVelocityMagnitude,
                landingAngle = dotProduct
            });
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
            landingType = LandingType.Success,
            score = score,
            scoreMultiplier = landingPan.GetScoreMultiplier(),
            landingSpeed = relativeVelocityMagnitude,
            landingAngle = dotProduct
        });

        SetState(State.GameOver);

    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            if (fuelAmount >= fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            fuelPickup.DestroySelf();
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });
    }

    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }

    public float GetSpeedX()
    {
        return landerRigidbody2D.linearVelocityX;
    }

    public float GetSpeedY()
    {
        return landerRigidbody2D.linearVelocityY;
    }

}
