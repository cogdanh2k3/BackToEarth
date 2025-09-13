using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private TextMeshProUGUI buttonTextMesh;
    [SerializeField] private Button nextButton;

    private Action nextButtonClickAction;

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            nextButtonClickAction();
        });
    }

    public void Start()
    {
        Lander.instance.OnLanded += Lander_OnLanded;
        Hide();
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if(e.landingType == Lander.LandingType.Success)
        {
            titleTextMesh.text = "SUCCESSFUL LANDING!";
            buttonTextMesh.text = "CONTINUE";
            nextButtonClickAction = GameManager.instance.GoToNextLevel;
        }
        else
        {
            titleTextMesh.text = "<color=#ff0000>CRASH!BUMMM</color>";
            buttonTextMesh.text = "RESTART";
            nextButtonClickAction = GameManager.instance.RetryLevel;

        }

        statsTextMesh.text =
            e.score + "\n" +
            "x" + e.scoreMultiplier + "\n" +
            Mathf.Round(e.landingSpeed * 2f) + "\n" +
            Mathf.Round(e.landingAngle*100f)+ "\n";

        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        nextButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
