using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button instructionButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;


    private enum Sub
    {
        Main,
        HowToPlay
    }

    private void Awake()
    {
        Time.timeScale = 1f;

        playButton.onClick.AddListener(() =>
        {
            GameManager.ResetStaticData();
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        });

        instructionButton.onClick.AddListener(() =>
        {
            ShowSub(Sub.HowToPlay);
        });

        backButton.onClick.AddListener(() =>
        {
            ShowSub(Sub.Main);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        ShowSub(Sub.Main);
    }

    private void ShowSub(Sub sub)
    {
        transform.Find("mainSub").gameObject.SetActive(false);
        transform.Find("howToPlaySub").gameObject.SetActive(false);


        switch (sub)
        {
            case Sub.Main:
                transform.Find("mainSub").gameObject.SetActive(true);
                break;
            case Sub.HowToPlay:
                transform.Find("howToPlaySub").gameObject.SetActive(true);
                break;
        }
    }

}
