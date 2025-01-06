using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> 
{
    [SerializeField] private GameObject winningPanel;
    [SerializeField] private GameObject losingPanel;

    [SerializeField] private Button playAgainBtn;
    [SerializeField] private Button playAgainBtn2;

    private void Start()
    {
        playAgainBtn.onClick.AddListener(ClickPlayAgain);
        playAgainBtn2.onClick.AddListener(ClickPlayAgain);
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
}
