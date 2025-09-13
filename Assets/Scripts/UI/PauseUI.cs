using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button soundVolumeButton;
    [SerializeField] private TextMeshProUGUI soundVolumnBtnTextMesh;
    [SerializeField] private Button musicVolumeButton;
    [SerializeField] private TextMeshProUGUI musicVolumnBtnTextMesh;


    private void Awake()
    {

        soundVolumeButton.onClick.AddListener(() =>
        {
            SoundManager.instance.ChangeSoundVolume();
            soundVolumnBtnTextMesh.text = "SOUND: " + SoundManager.instance.GetSoundVolume();

        });

        musicVolumeButton.onClick.AddListener(() =>
        {
            MusicManager.instance.ChangeMusicVolume();
            musicVolumnBtnTextMesh.text = "MUSIC: " + MusicManager.instance.GetMusicVolume();

        });

        resumeButton.onClick.AddListener(() =>
        {
            GameManager.instance.ResumeGame();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        GameManager.instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.instance.OnGameResume += GameManager_OnGameResume;

        soundVolumnBtnTextMesh.text = "SOUND: " + SoundManager.instance.GetSoundVolume();
        musicVolumnBtnTextMesh.text = "MUSIC: " + MusicManager.instance.GetMusicVolume();
        Hide();
    }

    private void GameManager_OnGameResume(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
