using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyingManager : Singleton<BuyingManager>
{
    [SerializeField] private GameObject buyingParkSlotPanel;

    [SerializeField] private GameObject arrageSkillTut;
    [SerializeField] private GameObject jumbleSkillTut;
    [SerializeField] private GameObject vIPSkillTut;
    [SerializeField] private GameObject turboSkillTut;

    [SerializeField] private GameObject timingClock;

    [SerializeField] private Button buyParkSlotBtn;

    [SerializeField] private Button arrangeBtn;
    [SerializeField] private Button buyArrangeBtn;
    [SerializeField] private Button jumbleBtn;
    [SerializeField] private Button buyJumbleBtn;
    [SerializeField] private Button vIPBtn;
    [SerializeField] private Button buyVIPBtn;
    [SerializeField] private Button turboBtn;
    [SerializeField] private Button buyTurboBtn;

    [SerializeField] private Text coinText;

    private ParkSlot parkSlot;

    private int coin = 10;

    public ParkSlot ParkSlot { get => parkSlot; set => parkSlot = value; }
    public int Coin { get => coin; set => coin = value; }

    void Start()
    {
        UpdateCoin();

        buyParkSlotBtn.onClick.AddListener(ClickBuyParkSlotBtn);

        arrangeBtn.onClick.AddListener(ArrangeSkillBtn);
        buyArrangeBtn.onClick.AddListener(ClickBuyArrangeBtn);

        jumbleBtn.onClick.AddListener(JubmleSkillBtn);
        buyJumbleBtn.onClick.AddListener(ClickBuyJumbleSkill);

        vIPBtn.onClick.AddListener(VIPSkillBtn);
        buyVIPBtn.onClick.AddListener(ClickBuyVIPBtn);

        turboBtn.onClick.AddListener(TurboSkillBtn);
        buyTurboBtn.onClick.AddListener(ClickBuyTurboBtn);

    }

    public void UpdateCoin()
    {
        coinText.text = coin.ToString();
    }

    public void ClickUnlockSlot()
    {
        buyingParkSlotPanel.SetActive(true);
    }

    public void ClickBuyParkSlotBtn()
    {
        if (coin >= 5)
        {
            coin -= 5;
            UpdateCoin();
            LevelManager.Instance.AddUnlockParkSlot(parkSlot);
        }

        buyingParkSlotPanel.SetActive(false);
    }

    public void ArrangeSkillBtn()
    {
        arrageSkillTut.SetActive(true);
    }

    public void ClickBuyArrangeBtn()
    {
        if (coin >= 5)
        {
            coin -= 5;
            UpdateCoin();
            LevelManager.Instance.ArrangeSkill();
        }

        arrageSkillTut.SetActive(false);
    }

    public void JubmleSkillBtn()
    {
        jumbleSkillTut.SetActive(true);
    }

    public void ClickBuyJumbleSkill()
    {
        if (coin >= 5)
        {
            coin -= 5;
            UpdateCoin();
            LevelManager.Instance.JumbleSkill();
        }

        jumbleSkillTut.SetActive(false);
    }

    public void VIPSkillBtn()
    {
        vIPSkillTut.SetActive(true);
    }

    public void ClickBuyVIPBtn()
    {
        if (coin >= 5)
        {
            coin -= 5;
            UpdateCoin();
            LevelManager.Instance.IsVIP = true;
        }

        vIPSkillTut.SetActive(false);
    }

    public void TurboSkillBtn()
    {
        turboSkillTut.SetActive(true);
    }

    public void ClickBuyTurboBtn()
    {
        if (coin >= 5)
        {
            coin -= 5;
            UpdateCoin();
            LevelManager.Instance.TurboSkill();
            timingClock.SetActive(true);
        }

        turboSkillTut.SetActive(false);
    }
}
