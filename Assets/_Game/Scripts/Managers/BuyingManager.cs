using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BuyingManager : Singleton<BuyingManager>
{
    [SerializeField] private GameObject buyingParkSlotPanel;

    [SerializeField] private GameObject arrageSkillTut;
    [SerializeField] private GameObject jumbleSkillTut;
    [SerializeField] private GameObject vIPSkillTut;
    [SerializeField] private GameObject turboSkillTut;

    [SerializeField] private GameObject timingClock;

    [SerializeField] private Button buyParkSlotBtn;
    [SerializeField] private Button backBuyParkSlotBtn;

    [SerializeField] private Button arrangeBtn;
    [SerializeField] private Button buyArrangeBtn;
    [SerializeField] private Button backBuyArrangeBtn;
    [SerializeField] private Button jumbleBtn;
    [SerializeField] private Button buyJumbleBtn;
    [SerializeField] private Button backBuyJumbleBtn;
    [SerializeField] private Button vIPBtn;
    [SerializeField] private Button buyVIPBtn;
    [SerializeField] private Button backBuyVIPBtn;
    [SerializeField] private Button turboBtn;
    [SerializeField] private Button buyTurboBtn;

    [SerializeField] private Image arrangeNotifi;
    [SerializeField] private Image vIPNotifi;

    [SerializeField] private Text coinText;

    private ParkSlot parkSlot;

    private bool isBuying = false;

    private int coin = 10;

    public ParkSlot ParkSlot { get => parkSlot; set => parkSlot = value; }
    public int Coin { get => coin; set => coin = value; }
    public Image VIPNotifi { get => vIPNotifi; set => vIPNotifi = value; }
    public bool IsBuying { get => isBuying; set => isBuying = value; }

    void Start()
    {
        UpdateCoin();

        buyParkSlotBtn.onClick.AddListener(ClickBuyParkSlotBtn);
        backBuyParkSlotBtn.onClick.AddListener(BackAllTut);

        arrangeBtn.onClick.AddListener(ArrangeSkillBtn);
        buyArrangeBtn.onClick.AddListener(ClickBuyArrangeBtn);
        backBuyArrangeBtn.onClick.AddListener(BackAllTut);

        jumbleBtn.onClick.AddListener(JubmleSkillBtn);
        buyJumbleBtn.onClick.AddListener(ClickBuyJumbleSkill);
        backBuyJumbleBtn.onClick.AddListener(BackAllTut);

        vIPBtn.onClick.AddListener(VIPSkillBtn);
        buyVIPBtn.onClick.AddListener(ClickBuyVIPBtn);
        backBuyVIPBtn.onClick.AddListener(BackAllTut);

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
        IsBuying = true;
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
        IsBuying = false;
    }

    public void ArrangeSkillBtn()
    {
        if (!LevelManager.Instance.IsAllEmpty())
        {
            arrageSkillTut.SetActive(true);
            IsBuying = true;
        }
        else
        {
            // show text notifacation
            arrangeNotifi.gameObject.SetActive(true);

            arrangeNotifi.DOFade(1f, 0.5f).OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    arrangeNotifi.DOFade(0, 0.5f).OnComplete(() =>
                    {
                        arrangeNotifi.gameObject.SetActive(false);
                    });
                });
            });
        }
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
        IsBuying = false;
    }

    public void JubmleSkillBtn()
    {
        jumbleSkillTut.SetActive(true);
        IsBuying = true;
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
        IsBuying = false;
    }

    public void VIPSkillBtn()
    {
        vIPSkillTut.SetActive(true);
        IsBuying = true;
    }

    public void ClickBuyVIPBtn()
    {
        if (coin >= 5)
        {
            vIPNotifi.gameObject.SetActive(true);
            vIPNotifi.DOFade(1f, 0.5f);

            coin -= 5;
            UpdateCoin();
            LevelManager.Instance.IsVIP = true;
        }

        vIPSkillTut.SetActive(false);
        IsBuying = false;
    }

    public void TurboSkillBtn()
    {
        turboSkillTut.SetActive(true);
        IsBuying = true;
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
        IsBuying = false;
    }

    public void BackAllTut()
    {
        arrageSkillTut.SetActive(false);
        jumbleSkillTut.SetActive(false);
        vIPSkillTut.SetActive(false);
        buyingParkSlotPanel.SetActive(false);
        IsBuying = false;
    }
}
