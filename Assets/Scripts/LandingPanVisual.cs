using TMPro;
using UnityEngine;

public class LandingPanVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreMultiplier;

    private void Awake()
    {
        LandingPan landingPan = GetComponent<LandingPan>();
        scoreMultiplier.text = "x" + landingPan.GetScoreMultiplier();
    }
}
