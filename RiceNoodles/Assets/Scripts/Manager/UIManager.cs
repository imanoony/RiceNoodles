using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas todayResultCanvas;
    [SerializeField] private Canvas MoneyCanvas;
    [SerializeField] private Canvas ButtonCanvas;
    [SerializeField] private Canvas EnchantCanvas;
    [SerializeField] private Canvas SettingsCanvas;
    [SerializeField] private Canvas TimeCanvas;

    [SerializeField] private Sprite[] ClockSprites;

    private TextMeshProUGUI moneyText;
    private TextMeshProUGUI todayResultText;

    private Button settingButton;
    private Button enchantButton;

    private Image clockImage;

    private static string previousState;
    private static string currentState;
    public static string CurrentState => currentState;

    private bool successDay;


    private void Awake(){
        DontDestroyOnLoad(gameObject);

        moneyText = MoneyCanvas.transform.Find("Money/Amount").GetComponent<TextMeshProUGUI>();
        todayResultText = todayResultCanvas.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        settingButton = ButtonCanvas.transform.Find("Settings").GetComponent<Button>();
        enchantButton = ButtonCanvas.transform.Find("Enchant").GetComponent<Button>();
        settingButton.onClick.AddListener(SettingsButtonClick);
        enchantButton.onClick.AddListener(EnchantButtonClick);

        clockImage = TimeCanvas.transform.Find("Clock").GetComponent<Image>();

        MoneyCanvas.gameObject.SetActive(false);
        todayResultCanvas.gameObject.SetActive(false);
        ButtonCanvas.gameObject.SetActive(false);
        EnchantCanvas.gameObject.SetActive(false);
        SettingsCanvas.gameObject.SetActive(false);
        TimeCanvas.gameObject.SetActive(false);
    }

    private void Start(){
        currentState = "InGame";
    }


    private void Update(){
        moneyText.text = MoneyManager.CurrentMoney.ToString() + "$";

        if(Input.GetKeyDown(KeyCode.Escape)){
            if(currentState == "Settings"){
                currentState = previousState;
                Time.timeScale = 1f;
            }
            else if(currentState != "TodayResult"){
                SettingsButtonClick();
            }
        }

        if(Input.GetKeyDown(KeyCode.E)){
            if(currentState == "Enchant"){
                currentState = "InGame";
                Time.timeScale = 1f;
            }
            else{
                EnchantButtonClick();
            }
        }

        switch(currentState){
            case "InGame":
                MoneyCanvas.gameObject.SetActive(true);
                todayResultCanvas.gameObject.SetActive(false);
                ButtonCanvas.gameObject.SetActive(true);
                EnchantCanvas.gameObject.SetActive(false);
                SettingsCanvas.gameObject.SetActive(false);
                TimeCanvas.gameObject.SetActive(true);

                //Debug.Log(TimeManager.CurrentHour);
                clockImage.sprite = ClockSprites[TimeManager.CurrentHour-10];
                break;
            case "TodayResult":
                MoneyCanvas.gameObject.SetActive(false);
                todayResultCanvas.gameObject.SetActive(true);
                ButtonCanvas.gameObject.SetActive(false);
                EnchantCanvas.gameObject.SetActive(false);
                SettingsCanvas.gameObject.SetActive(false);
                TimeCanvas.gameObject.SetActive(false);
                break;
            case "Settings":
                MoneyCanvas.gameObject.SetActive(false);
                todayResultCanvas.gameObject.SetActive(false);
                ButtonCanvas.gameObject.SetActive(false);
                EnchantCanvas.gameObject.SetActive(false);
                SettingsCanvas.gameObject.SetActive(true);
                TimeCanvas.gameObject.SetActive(false);
                break;
            case "Enchant":
                MoneyCanvas.gameObject.SetActive(false);
                todayResultCanvas.gameObject.SetActive(false);
                ButtonCanvas.gameObject.SetActive(false);
                EnchantCanvas.gameObject.SetActive(true);
                SettingsCanvas.gameObject.SetActive(false);
                TimeCanvas.gameObject.SetActive(false);
                break;
        }
    }

    

    public void ShowTodayResult(){
        currentState = "TodayResult";
        string TodayResultText = "오늘의 매출\n" + MoneyManager.TodaySales + "$\n\n오늘의 지출\n" + MoneyManager.TodaySpending + "$";
        if(MoneyManager.TargetMoney <= MoneyManager.CurrentMoney){
            TodayResultText += "\n\n성공!";
            successDay = true;
        }
        else{
            TodayResultText += "\n\n실패...";
            successDay = false;
        }
        StartCoroutine(ShowTodayResultCoroutine(TodayResultText));
    }

    IEnumerator ShowTodayResultCoroutine(string text){
        todayResultText.text = "";
        for (int i = 0; i <= text.Length; i++)
        {
            todayResultText.text = text.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }

        while(!Input.GetKeyDown(KeyCode.Escape)){
            yield return null;
        }

        if(successDay){
            StageManager.NextStage();
            currentState = "InGame";
        }
        else{
            // 메인 화면으로
        }
    }


    private void SettingsButtonClick(){
        previousState = currentState;
        currentState = "Settings";
        Time.timeScale = 0f;
    }

    private void EnchantButtonClick(){
        if(currentState == "InGame"){
            currentState = "Enchant";
            EnchantManager.InitiateEnchant();
            Time.timeScale = 0f;
        }
    }

}
