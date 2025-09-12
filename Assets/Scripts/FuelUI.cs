using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour
{
    [SerializeField] private Image fuelImage;

    private void Update()
    {
        UpdateFuelUI();
    }
    private void UpdateFuelUI()
    {
        fuelImage.fillAmount = Lander.instance.GetFuelAmountNormalized();
    }
}
