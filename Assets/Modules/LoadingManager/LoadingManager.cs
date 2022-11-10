using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    //[Header("ProgressBar")]
    [SerializeField] private bool enableProgressBar;
    [SerializeField] private Sprite progressBarBackground;
    [SerializeField] private Color progressBarBackgroundColor;
    [SerializeField] private Sprite progressBarFilled;
    [SerializeField] private Color progressBarFilledColor;

    //[Header("Fade In/Out")]
    [SerializeField] private bool enableFadeInOut;
    [SerializeField] private bool fadesIn;
    [SerializeField] private float fadeInTime;
    [SerializeField] private bool fadesOut;
    [SerializeField] private float fadeOutTime;

    //[Header("Text")]
    [SerializeField] private bool enableText;
    [SerializeField] private string text;
    [SerializeField] private float textSize;
    [SerializeField] private Color textColor;
    [SerializeField] private TMP_FontAsset textFont;

    //[Header("Background")]
    [SerializeField] private bool enableBackgroundImage;
    [SerializeField] private Sprite backgroundImage;
    [SerializeField] private Color backgroundColor;

    //////////
    CanvasGroup canvasGroup;
    Image background;
    TextMeshProUGUI label;
    Image progressBackground;
    Image progressFilled;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one LoadingManager in the scene! " + transform.position + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        canvasGroup = GetComponentInChildren<CanvasGroup>();
        background = canvasGroup.transform.Find("Background").GetComponent<Image>();
        label = canvasGroup.transform.Find("Label").GetComponent<TextMeshProUGUI>();
        progressBackground = canvasGroup.transform.Find("ProgressBackground").GetComponent<Image>();
        progressFilled = progressBackground.transform.Find("ProgressFilled").GetComponent<Image>();
    }

    private void StartLoading()
    {
        HandleTextSetup();
        HandleProgressBarSetup();
        HandleBackgroundSetup();
        HandleFadeIn();

        progressFilled.fillAmount = 0f;
    }

    private void HandleTextSetup()
    {
        if (enableText)
        {
            label.text = text;
            label.fontSize = textSize;
            label.color = textColor;
            label.font = textFont;
        }
    }

    private void HandleProgressBarSetup()
    {
        if (enableProgressBar)
        {
            progressBackground.sprite = progressBarBackground;
            progressBackground.color = progressBarBackgroundColor;
            progressFilled.sprite = progressBarFilled;
            progressFilled.color = progressBarFilledColor;
        }
    }

    private void HandleBackgroundSetup()
    {
        if (enableBackgroundImage)
            background.sprite = backgroundImage;

        background.color = backgroundColor;
    }

    private void HandleFadeIn()
    {
        canvasGroup.alpha = 0f;
        if (enableFadeInOut && fadesIn)
        {
            canvasGroup.LeanAlpha(1f, fadeInTime);
            return;
        }
        canvasGroup.alpha = 1f;
    }

    private void HandleFadeOut()
    {
        canvasGroup.alpha = 1f;
        if (enableFadeInOut && fadesOut)
        {
            canvasGroup.LeanAlpha(0f, fadeInTime);
            return;
        }
        canvasGroup.alpha = 0f;
    }

    private int GetFadeInTimeInMilliseconds()
    {
        if (enableFadeInOut && fadesIn)
            return Mathf.RoundToInt(fadeInTime * 1000);

        return 0;
    }

    public async void LoadScene(string sceneName)
    {
        StartLoading();
        await Task.Delay(GetFadeInTimeInMilliseconds());

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        while (!scene.isDone)
        {
            progressFilled.fillAmount = scene.progress;
            if (Mathf.Approximately(scene.progress, 0.9f))
            {
                scene.allowSceneActivation = true;
            }
            await Task.Delay(10);
        }

        HandleFadeOut();
    }
}
