using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Button restartBtn;

    void Start()
    {
        restartBtn.onClick.AddListener(GameManager.Instance.ClickPlayAgain);
    }
}
