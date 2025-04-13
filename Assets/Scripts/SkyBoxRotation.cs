using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkyBoxRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public string gameScene = "Game";

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
            exitButton.onClick.AddListener(() => ExitGame());
        }
    }

    void Update()
    {
        // Rotar el Skybox
        float rotation = RenderSettings.skybox.GetFloat("_Rotation") + rotationSpeed * Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }

    void StartGame()
    {
        ChangeButtonSprite(startButtonImage, startPressedSprite);
        Invoke(nameof(LoadGameScene), 0.2f);
    }

    void LoadGameScene()
    {
        ChangeButtonSprite(startButtonImage, startNormalSprite);
        SceneManager.LoadScene(gameScene);
    }

    void ExitGame()
    {
        ChangeButtonSprite(exitButtonImage, exitPressedSprite);
        Invoke(nameof(QuitApplication), 0.2f);
    }

    void QuitApplication()
    {
        ChangeButtonSprite(exitButtonImage, exitNormalSprite);
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void ChangeButtonSprite(Image buttonImage, Sprite newSprite)
    {
        if (buttonImage != null && newSprite != null)
        {
            buttonImage.sprite = newSprite;
        }
    }
}
