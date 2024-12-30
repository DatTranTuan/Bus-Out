using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> 
{
    [SerializeField] private GameObject winningPanel;

    [SerializeField] private Button playAgainBtn;

    private void Start()
    {
        playAgainBtn.onClick.AddListener(ClickPlayAgain);
    }

    private void Update()
    {
        if (LevelManager.Instance.ListPassen.Count <= 0)
        {
            winningPanel.SetActive(true);
        }
    }

    public void ClickPlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
