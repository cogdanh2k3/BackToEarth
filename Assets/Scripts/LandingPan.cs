using UnityEngine;

public class LandingPan : MonoBehaviour
{
    [SerializeField] private int scoreMultiplier;

    public int GetScoreMultiplier()
    {
        return scoreMultiplier;
    }
}
