using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public float allCarTouchTime;

    [SerializeField] private float touchTime;

    [SerializeField] private Text countSignText;

    [SerializeField] private GameObject winningPanel;
    [SerializeField] private GameObject losingPanel;
    [SerializeField] private GameObject replayPanel;

    [SerializeField] private Button replayPanelBtn;
    [SerializeField] private Button exitReplayBtn;
    [SerializeField] private Button replayBtn;
    [SerializeField] private Button replay2Btn;

    [SerializeField] private Button nextLevelBtn;

    [SerializeField] private GameObject map1;
    [SerializeField] private GameObject map2;
    [SerializeField] private GameObject map3;
    [SerializeField] private GameObject map4;
    [SerializeField] private GameObject map5;

    public GameObject WinningPanel { get => winningPanel; set => winningPanel = value; }
    public GameObject LosingPanel { get => losingPanel; set => losingPanel = value; }
    public float TouchTime { get => touchTime; set => touchTime = value; }
    public GameObject Map1 { get => map1; set => map1 = value; }
    public GameObject Map2 { get => map2; set => map2 = value; }
    public GameObject Map3 { get => map3; set => map3 = value; }
    public GameObject Map4 { get => map4; set => map4 = value; }
    public GameObject Map5 { get => map5; set => map5 = value; }
    private void Awake()
    {
        Input.multiTouchEnabled = false;

        DOTween.SetTweensCapacity(500, 50);

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
        LevelManager.Instance.LoadLevel();

        nextLevelBtn.onClick.AddListener(ClickNextLevel);

        replayPanelBtn.onClick.AddListener(ClickReplayPanel);
        exitReplayBtn.onClick.AddListener(BackAll);
        replayBtn.onClick.AddListener(ClickReplayBtn);
        replay2Btn.onClick.AddListener(ClickReplayBtn);
    }

    private void Update()
    {
        touchTime = Time.time;
    }

    public void ClickReplayPanel()
    {
        replayPanel.SetActive(true);
    }

    public void ClickReplayBtn()
    {
        //LevelManager.Instance.LoadLevel();

        GameManager.Instance.Replay();
        replayPanel.SetActive(false);
        losingPanel.SetActive(false);
        SoundManager.Instance.Play("BG");
    }

    public void ClickNextLevel()
    {
        //LevelManager.Instance.NextLevel();

        DataManager.Instance.CurrentLevel++;
        LevelManager.Instance.LoadLevel();

        winningPanel.SetActive(false);
        SoundManager.Instance.Play("BG");
    }

    public void Losing()
    {
        SoundManager.Instance.Stop("BG");
        SoundManager.Instance.Play("Lose");
        LosingPanel.gameObject.SetActive(true);
    }

    public void Winning()
    {
        SoundManager.Instance.Stop("BG");
        SoundManager.Instance.Play("Win");
        winningPanel.gameObject.SetActive(true);
    }

    public void UpdateCountSignText()
    {
        countSignText.text = LevelManager.Instance.PassenCount.ToString();
    }

    public void BackAll()
    {
        replayPanel.SetActive(false);
    }

    public void Replay()
    {
        SceneManager.LoadScene("SampleScene");
        LevelManager.Instance.LoadLevel();
    }
}
