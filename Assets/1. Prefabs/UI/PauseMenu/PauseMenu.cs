using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    
    [SerializeField] private  UIDocument uiDocument;
    private VisualElement root;

    private VisualElement pauseMenuElement;
    private VisualElement settingsMenuElement;
    
    private Button resumeButton;
    private Button settingsButton;
    private Button exitButton;
    private Button quitButton;

    //private Button backToMenuButton;
    //private Button saveSettingsButton;

    private bool isGamePaused = false;

    /*Slider volumeSliderMaster;
    Slider volumeSliderMusic;
    Slider volumeSliderSFX;
    Slider volumeSliderEnvironment;*/


    private void Awake()
    {
        root = uiDocument.rootVisualElement;
        pauseMenuElement = root.MQ("PauseMenu");
        settingsMenuElement = root.MQ("SettingsMenu");
        
        /*volumeSliderMaster = settingsMenuElement.MQ<Slider>("Slider_Volume_Master");
        volumeSliderMusic = settingsMenuElement.MQ<Slider>("Slider_Volume_Music");
        volumeSliderSFX = settingsMenuElement.MQ<Slider>("Slider_Volume_SFX");
        volumeSliderEnvironment = settingsMenuElement.MQ<Slider>("Slider_Volume_Environment");*/
    }

    private void OnEnable()
    {
        BindEvents();
    }
    
    private void Start()
    {
        OnResumeButtonClicked();
        /*playerSettingsService = ServiceLocator.Instance.Get<IPlayerSettingsService>();
        playerSettingsService.FloatSettingChanged += OnPlayerSettingsFloatChanged;
        
        volumeSliderMaster.value = playerSettingsService.GetFloat(PlayerSettingsKeys.VolumeMaster, 0.5f);
        volumeSliderMusic.value = playerSettingsService.GetFloat(PlayerSettingsKeys.VolumeMusic, 1f);
        volumeSliderSFX.value = playerSettingsService.GetFloat(PlayerSettingsKeys.VolumeSFX, 1f);
        volumeSliderEnvironment.value = playerSettingsService.GetFloat(PlayerSettingsKeys.VolumeEnvironment, 1f);*/
    }

    /*private void OnPlayerSettingsFloatChanged(string key, float value)
    {
        switch (key)
        {
            case PlayerSettingsKeys.VolumeMaster:
                volumeSliderMaster.value = value;
                break;
            case PlayerSettingsKeys.VolumeMusic:
                volumeSliderMusic.value = value;
                break;
            case PlayerSettingsKeys.VolumeSFX:
                volumeSliderSFX.value = value;
                break;
            case PlayerSettingsKeys.VolumeEnvironment:
                volumeSliderEnvironment.value = value;
                break;
        }
    }*/

    private void BindEvents()
    {
        BindPauseMenuEvents();
        //BindSettingsMenuEvents();
    }

    private void PausePerformed()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isGamePaused = true;
        ShowUI();
        PauseTime();
    }

    private void ResumeGame()
    {
        isGamePaused = false;
        HideUI();
        UnpauseTime();
    }

    private void RestartLevel()
    {
        HideUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ExitToStartScreen()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UnpauseTime();
    }

    private void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void UnlockCursor()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    private void LockCursor()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    private void PauseTime()
    {
        Time.timeScale = 0;
    }

    private void UnpauseTime()
    {
        Time.timeScale = 1;
    }

    public bool GetIsGamePaused()
    {
        return isGamePaused;
    }


    private void ExitPressed(InputAction.CallbackContext context)
    {
        PausePerformed();
    }

    private void BindPauseMenuEvents()
    {
        inputManager.Exit_Performed += ExitPressed;

        resumeButton = pauseMenuElement.MQ<Button>("Resume");
        resumeButton.clicked += OnResumeButtonClicked;

        settingsButton = pauseMenuElement.MQ<Button>("Settings");
        settingsButton.clicked += OnSettingsButtonClicked;
        
        exitButton = pauseMenuElement.MQ<Button>("Exit", "Button");
        exitButton.clicked += OnExitButtonClicked;

        quitButton = pauseMenuElement.MQ<Button>("Quit", "Button");
        quitButton.clicked += OnQuitButtonClicked;
    }

    /*private void BindSettingsMenuEvents()
    {
        backToMenuButton = settingsMenuElement.MQ<Button>("Btn_Back");
        backToMenuButton.clicked += OnBackButtonClicked;

        saveSettingsButton = settingsMenuElement.MQ<Button>("Btn_Save");
        saveSettingsButton.clicked += OnSaveSettingsButtonClicked;
    }*/

    private void OnSaveSettingsButtonClicked()
    {
        /*playerSettingsService.SetFloat(PlayerSettingsKeys.VolumeMaster, volumeSliderMaster.value);
        playerSettingsService.SetFloat(PlayerSettingsKeys.VolumeMusic, volumeSliderMusic.value);
        playerSettingsService.SetFloat(PlayerSettingsKeys.VolumeSFX, volumeSliderSFX.value);
        playerSettingsService.SetFloat(PlayerSettingsKeys.VolumeEnvironment, volumeSliderEnvironment.value);*/
    }

    private void OnBackButtonClicked()
    {
        pauseMenuElement.style.display = DisplayStyle.Flex;
        settingsMenuElement.style.display = DisplayStyle.None;
    }

    public void ShowUI()
    {
        pauseMenuElement.style.display = DisplayStyle.Flex;
        settingsMenuElement.style.display = DisplayStyle.None;
        
        root.AddToClassList("ScaleStart");
        root.RemoveFromClassList("ScaleEnd");
    }

    public void HideUI()
    {
        pauseMenuElement.style.display = DisplayStyle.None;
        settingsMenuElement.style.display = DisplayStyle.None;

        root.RemoveFromClassList("ScaleStart");
        root.AddToClassList("ScaleEnd");
    }

    private void OnResumeButtonClicked()
    {
        ResumeGame();
    }

    private void OnSettingsButtonClicked()
    {
        pauseMenuElement.style.display = DisplayStyle.None;
        settingsMenuElement.style.display = DisplayStyle.Flex;
    }

    private void OnExitButtonClicked()
    {
        ExitToStartScreen();
    }

    private void OnQuitButtonClicked()
    {
        ExitGame();
    }

    private void OnDisable()
    {
        resumeButton.clicked -= OnResumeButtonClicked;
        settingsButton.clicked -= OnSettingsButtonClicked;
        exitButton.clicked -= OnExitButtonClicked;
        quitButton.clicked -= OnQuitButtonClicked;
    }
}
