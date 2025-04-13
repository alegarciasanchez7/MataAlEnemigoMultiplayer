using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuMuerte : MonoBehaviour
{
    public string gameScene = "Game";
    public string mainScene = "MainMenu";

    [Header("Start Button Settings")]
    public Button startButton;
    public Image startButtonImage;
    public Sprite startNormalSprite;
    public Sprite startPressedSprite;

    [Header("Exit Button Settings")]
    public Button exitButton;
    public Image exitButtonImage;
    public Sprite exitNormalSprite;
    public Sprite exitPressedSprite;

    public AudioSource backgroundMusic;

    void Start()
    {
        // Iniciar la música si está asignada
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }

        // Asignar eventos a los botones
        if (startButton != null)
        {
            startButton.onClick.AddListener(() => StartGame());
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(() => ExitToMainMenu());
        }
    }

    void StartGame()
    {
        Debug.Log("Start button clicked");
        ChangeButtonSprite(startButtonImage, startPressedSprite);
        Invoke(nameof(LoadGameScene), 0.2f);
    }

    void LoadGameScene()
    {
        Debug.Log("Loading game scene");
        ChangeButtonSprite(startButtonImage, startNormalSprite);
        SceneManager.LoadScene(gameScene);
    }

    void ExitToMainMenu()
    {
        Debug.Log("Exit button clicked");
        ChangeButtonSprite(exitButtonImage, exitPressedSprite);
        Invoke(nameof(LoadMainMenuScene), 0.2f);
    }

    void LoadMainMenuScene()
    {
        Debug.Log("Loading main menu scene");
        ChangeButtonSprite(exitButtonImage, exitNormalSprite);
        SceneManager.LoadScene(mainScene);
    }

    void ChangeButtonSprite(Image buttonImage, Sprite newSprite)
    {
        if (buttonImage != null && newSprite != null)
        {
            buttonImage.sprite = newSprite;
        }
    }
}
