using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float touchTime;

    [SerializeField] private Text countSignText;

    [SerializeField] private GameObject winningPanel;
    [SerializeField] private GameObject losingPanel;

    [SerializeField] private Button playAgainBtn;
    [SerializeField] private Button playAgainBtn2;

    public GameObject WinningPanel { get => winningPanel; set => winningPanel = value; }
    public GameObject LosingPanel { get => losingPanel; set => losingPanel = value; }
    public float TouchTime { get => touchTime; set => touchTime = value; }

    private void Awake()
    {
        Input.multiTouchEnabled = false;

        //DOTween.SetTweensCapacity(500, 125);

        //target frame rate ve 60 fps
        Application.targetFrameRate = 60;
        //tranh viec tat man hinh
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //xu tai tho
        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }

    }

    private void Start()
    {
        playAgainBtn.onClick.AddListener(ClickPlayAgain);
        playAgainBtn2.onClick.AddListener(ClickPlayAgain);
    }

    private void Update()
    {
        touchTime = Time.time;
    }

    public void ClickPlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Losing()
    {
        LosingPanel.gameObject.SetActive(true);
    }

    public void UpdateCountSignText()
    {
        countSignText.text = "0" + LevelManager.Instance.PassenCount.ToString();
    }
}
