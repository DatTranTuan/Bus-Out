using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> 
{
    [SerializeField] private Text countSignText;

    [SerializeField] private GameObject winningPanel;
    [SerializeField] private GameObject losingPanel;

    [SerializeField] private Button playAgainBtn;
    [SerializeField] private Button playAgainBtn2;

    public GameObject WinningPanel { get => winningPanel; set => winningPanel = value; }

    private void Awake()
    {
        Input.multiTouchEnabled = true;

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

    public void ClickPlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void CheckLosing()
    {
        int index = 0;

        for (int i = 0; i < LevelManager.Instance.ListParkSlot.Count; i++)
        {
            if (!LevelManager.Instance.ListParkSlot[i].IsEmpty)
            {
                index++;
                
                if (index == LevelManager.Instance.ListParkSlot.Count)
                {
                    losingPanel.gameObject.SetActive(true);
                }
            }
        }
    }

    public void UpdateCountSignText()
    {
        countSignText.text = "0" + LevelManager.Instance.PassenCount.ToString();
    }
}
