using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedLeftArrowGO;
    [SerializeField] private GameObject speedRightArrowGO;
    [SerializeField] private GameObject speedUpArrowGO;
    [SerializeField] private GameObject speedDownArrowGO;

    private void Update()
    {
        UpdateStatsTextMesh();    
    }

    private void UpdateStatsTextMesh()
    {
        speedUpArrowGO.SetActive(Lander.instance.GetSpeedY() >= 0f); 
        speedDownArrowGO.SetActive(Lander.instance.GetSpeedY() < 0f); 
        speedLeftArrowGO.SetActive(Lander.instance.GetSpeedX() < 0f); 
        speedRightArrowGO.SetActive(Lander.instance.GetSpeedX() >= 0f);

        statsTextMesh.text =
            GameManager.instance.GetScore() + "\n" +
            Mathf.Round(GameManager.instance.GetTime()) + "\n" +
            Mathf.Abs(Mathf.Round(Lander.instance.GetSpeedX()*10f)) + "\n" +
            Mathf.Abs(Mathf.Round(Lander.instance.GetSpeedY()*10f));
            
    }
}
