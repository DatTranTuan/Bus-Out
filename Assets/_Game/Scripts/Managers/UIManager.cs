using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Button restartBtn;

    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject settingPanel;

    [SerializeField] private Button settingBtn;
    [SerializeField] private Button exitSettingBtn;

    [SerializeField] private Slider loadingSlider;

    [SerializeField] private Text sliderText;

    [SerializeField] private Toggle toggle;
    [SerializeField] private Image toggleOn;
    [SerializeField] private Image toggleOff;

    private float value;

    void Start()
    {
        SoundManager.Instance.Play("BG");

        settingBtn.onClick.AddListener(ClickSettingBtn);
        exitSettingBtn.onClick.AddListener(BackAll);

        restartBtn.onClick.AddListener(GameManager.Instance.ClickReplayPanel);

        toggle.onValueChanged.AddListener(CheckToggle);
    }

    private void Update()
    {
        if (loadingSlider.value <1.1f)
        {
            loadingSlider.value += Time.deltaTime * 0.5f;
            value = loadingSlider.value;
            sliderText.text = Mathf.RoundToInt(value * 100).ToString() + "%";

            if (loadingSlider.value >= 1)
            {
                loadingPanel.SetActive(false);
            }
        }
    }

    public void CheckToggle(bool isOn)
    {
        if (isOn)
        {
            toggleOff.gameObject.SetActive(false);
            toggleOn.gameObject.SetActive(true);
            SoundManager.Instance.TurnOnVolume();
        }
        else
        {
            toggleOff.gameObject.SetActive(true);
            toggleOn.gameObject.SetActive(false);
            SoundManager.Instance.TurnOffVolume();
        }
    }

    public void ClickSettingBtn()
    {
        settingPanel.SetActive(true);
        BuyingManager.Instance.IsBuying = true;
    }

    public void BackAll()
    {
        settingPanel.SetActive(false);
        BuyingManager.Instance.IsBuying = false;
    }
}
