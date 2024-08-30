using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private Canvas todayResultCanvas;
    [SerializeField] private Canvas MoneyCanvas;
    [SerializeField] private Canvas ButtonCanvas;
    [SerializeField] private Canvas EnchantCanvas;
    [SerializeField] private Canvas TimeCanvas;
    [SerializeField] private Canvas InfoCanvas;
    [SerializeField] private Canvas titleCanvas;
    [SerializeField] private Canvas storyCanvas;

    [SerializeField] private Sprite[] ClockSprites;

    private TextMeshProUGUI moneyText;
    private TextMeshProUGUI todayResultText;
    private TextMeshProUGUI dayInfoText;
    private TextMeshProUGUI targetText;
    private TextMeshProUGUI targetMoneyText;
    private TextMeshProUGUI dayText;
    private TextMeshProUGUI storyText;
    private Button enchantButton;

    private Image clockImage;
    private Image targetFrame;
    private Image blackScreen;

    private static string previousState;
    private static string currentState;
    public static string CurrentState => currentState;

    private bool successDay;
    private void Awake(){
        DontDestroyOnLoad(gameObject);

        moneyText = MoneyCanvas.transform.Find("Money/Amount").GetComponent<TextMeshProUGUI>();
        todayResultText = todayResultCanvas.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        dayInfoText = InfoCanvas.transform.Find("Day").GetComponent<TextMeshProUGUI>();
        targetText = InfoCanvas.transform.Find("Target/Target Text").GetComponent<TextMeshProUGUI>();
        targetMoneyText = EnchantCanvas.transform.Find("Target Money").GetComponent<TextMeshProUGUI>();
        dayText = EnchantCanvas.transform.Find("Day").GetComponent<TextMeshProUGUI>();
        storyText = storyCanvas.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        enchantButton = ButtonCanvas.transform.Find("Enchant").GetComponent<Button>();
        enchantButton.onClick.AddListener(EnchantButtonClick);

        clockImage = TimeCanvas.transform.Find("Clock").GetComponent<Image>();
        //targetFrame = InfoCanvas.transform.Find("Target/Target Frame").GetComponent<Image>();
        blackScreen = InfoCanvas.transform.Find("Black Screen").GetComponent<Image>();

        MoneyCanvas.gameObject.SetActive(false);
        todayResultCanvas.gameObject.SetActive(false);
        ButtonCanvas.gameObject.SetActive(false);
        EnchantCanvas.gameObject.SetActive(false);
        TimeCanvas.gameObject.SetActive(false);

        targetText.gameObject.SetActive(false);
        dayInfoText.gameObject.SetActive(false);

        firstPlay = true;
    }

    private void Start(){
        currentState = "Title";
    }
    private void Update(){
        moneyText.text = MoneyManager.CurrentMoney.ToString() + "$";
        targetMoneyText.text = MoneyManager.TargetMoney.ToString() + "$";
        dayText.text = "Day " + StageManager.CurrentStage;

        if(Input.GetKeyDown(KeyCode.E)){
            if(currentState == "Enchant"){
                currentState = "InGame";
                Time.timeScale = 1f;
            }
            else{
                EnchantButtonClick();
            }
        }

        if (Input.anyKeyDown) {
            if (currentState == "Title" && !transitioning) {
                StartCoroutine(closeTitleTransition());
            }
        }

        switch(currentState){
            case "Title":
                MoneyCanvas.gameObject.SetActive(false);
                todayResultCanvas.gameObject.SetActive(false);
                ButtonCanvas.gameObject.SetActive(false);
                EnchantCanvas.gameObject.SetActive(false);
                TimeCanvas.gameObject.SetActive(false);
                InfoCanvas.gameObject.SetActive(false);
                storyCanvas.gameObject.SetActive(true);
                break;
            case "InGame":
                MoneyCanvas.gameObject.SetActive(true);
                todayResultCanvas.gameObject.SetActive(false);
                ButtonCanvas.gameObject.SetActive(true);
                EnchantCanvas.gameObject.SetActive(false);
                TimeCanvas.gameObject.SetActive(true);
                InfoCanvas.gameObject.SetActive(true);
                storyCanvas.gameObject.SetActive(false);
                //Debug.Log(TimeManager.CurrentHour);
                clockImage.sprite = ClockSprites[TimeManager.CurrentHour-10];
                break;
            case "TodayResult":
                MoneyCanvas.gameObject.SetActive(false);
                todayResultCanvas.gameObject.SetActive(true);
                ButtonCanvas.gameObject.SetActive(false);
                EnchantCanvas.gameObject.SetActive(false);
                TimeCanvas.gameObject.SetActive(false);
                InfoCanvas.gameObject.SetActive(false);
                storyCanvas.gameObject.SetActive(false);
                break;
            case "Enchant":
                MoneyCanvas.gameObject.SetActive(false);
                todayResultCanvas.gameObject.SetActive(false);
                ButtonCanvas.gameObject.SetActive(false);
                EnchantCanvas.gameObject.SetActive(true);
                TimeCanvas.gameObject.SetActive(false);
                InfoCanvas.gameObject.SetActive(false);
                storyCanvas.gameObject.SetActive(false);
                break;
            case "Opening":
                MoneyCanvas.gameObject.SetActive(false);
                todayResultCanvas.gameObject.SetActive(false);
                ButtonCanvas.gameObject.SetActive(false);
                EnchantCanvas.gameObject.SetActive(false);
                TimeCanvas.gameObject.SetActive(false);
                InfoCanvas.gameObject.SetActive(false);
                storyCanvas.gameObject.SetActive(true);
                break;
            case "Ending":
                MoneyCanvas.gameObject.SetActive(false);
                todayResultCanvas.gameObject.SetActive(false);
                ButtonCanvas.gameObject.SetActive(false);
                EnchantCanvas.gameObject.SetActive(false);
                TimeCanvas.gameObject.SetActive(false);
                InfoCanvas.gameObject.SetActive(false);
                storyCanvas.gameObject.SetActive(true);
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

        Time.timeScale = 0f;

        while(!Input.GetKeyDown(KeyCode.Escape)){
            yield return null;
        }

        Time.timeScale = 1f;

        if(successDay){
            if(StageManager.CurrentStage < 5){
                StageManager.NextStage();
                ShowInfoUI();
                currentState = "InGame";
            }
            else{
                ShowEnding();
            }
        }
        else{
            StageManager.ResetStage();
            StartCoroutine(openTitleTransition());
        }
    }

    private void ShowInfoUI(){
        StartCoroutine(ShowInfoCoroutine());
    }
    
    private IEnumerator ShowInfoCoroutine(){
        //targetFrame.gameObject.SetActive(false);
        targetText.rectTransform.anchoredPosition = new Vector3(0f, -100f, 0f);
        targetText.rectTransform.localScale = new Vector3(2f, 2f, 2f);
        targetText.color = Color.white;
        dayInfoText.color = Color.white;
        dayInfoText.rectTransform.anchoredPosition = new Vector3(0f, 100f, 0f);
        dayInfoText.fontSize = 70;
        targetText.text = "목표 금액: " + MoneyManager.TargetMoneyList[StageManager.CurrentStage].ToString() + "$";
        dayInfoText.text = "Day " + StageManager.CurrentStage;
        blackScreen.gameObject.SetActive(true);
        targetText.gameObject.SetActive(true);
        dayInfoText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - elapsedTime / 1f);
            SetTextAlpha(dayInfoText, alpha);
            SetTextAlpha(targetText, alpha);

            
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //SetTextAlpha(dayInfoText, 1f);
        //SetTextAlpha(targetText, 1f);

        blackScreen.gameObject.SetActive(false);
        targetText.gameObject.SetActive(false);
        dayInfoText.gameObject.SetActive(false);

        //targetFrame.gameObject.SetActive(true);
        //targetText.rectTransform.anchoredPosition = new Vector3(-300f, -225f, 0f);
        //targetFrame.rectTransform.anchoredPosition = targetText.rectTransform.anchoredPosition;
        //targetText.color = Color.black;
        //targetText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        //dayInfoText.rectTransform.anchoredPosition = new Vector3(150f, -285f, 0f);
        //dayInfoText.fontSize = 36;

    }

    private void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
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
    private float openY = 0f, closeY = 680f;
    IEnumerator openTitleTransition() {
        transitioning = true;
        currentState = "Title";

        RectTransform rect = titleCanvas.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        while (rect.anchoredPosition.y >= openY) {
            float y = rect.anchoredPosition.y;
            y -= Time.deltaTime * 550f;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
            yield return null;
        }
        titleCanvas.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, openY);
        transitioning = false;
        yield break;
    }

    private bool firstPlay;
    private bool transitioning = false;
    IEnumerator closeTitleTransition() {
        transitioning = true;
        titleCanvas.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
        RectTransform rect = titleCanvas.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        while (rect.anchoredPosition.y <= closeY) {
            float y = rect.anchoredPosition.y;
            y += Time.deltaTime * 550f;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
            yield return null;
        }
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, closeY);
        
        if (firstPlay) ShowOpening();
        else {
            currentState = "InGame";
            ShowInfoUI();
        }
        transitioning = false;
        firstPlay = false;
        yield break;
    }

    private void ShowOpening(){
        currentState = "Opening";
        StartCoroutine(ShowOpeningCoroutine());
    }
    private IEnumerator ShowOpeningCoroutine(){
        String[] opening = {
            "2037년 대한민국은 갑작스러운 베이비붐으로 인해 아파트를 지을 곳이 더욱 더 필요해 상가가 들어설 자리는", 
            "줄어들게 되고 그로 인해 상가에 들어서는 음식점들의 자릿세는 하루가 다르게 치솟게 된다.", 
            "자영업자를 꿈꾸는 존은 태국에서 꾸어이띠여우를 맛본 뒤 깊은 감명을 얻고 한국에 태국 쌀국수 집을", 
            "차리기로 결심하고, 자신만의 방법으로 경영하며 늘어나는 자릿세를 감당해보기로 한다."
        };

        storyText.text = "";

        foreach(String line in opening){
            for (int i = 0; i < line.Length; i++)
            {
                storyText.text += line[i];
                yield return new WaitForSeconds(0.05f);
            }
            storyText.text += "\n\n";
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSecondsRealtime(2f);

        storyText.text = "";
        currentState = "InGame";
        ShowInfoUI();
    }

    private void ShowEnding(){
        currentState = "Ending";
        StartCoroutine(ShowEndingCoroutine());
    }
    private IEnumerator ShowEndingCoroutine(){
        String[] ending = {
            "5일 동안 폭발적으로 늘어나는 자릿세를 감당하는",
            "존을 보고 감명받은 건물주는 비전이 있어보이는",
            "존에게 건물을 양도하기로 하고, 1년뒤",
            "존은 이곳을 기점으로 세계적인 쌀국수 프랜차이즈를",
            "만드는데 성공하여 행복하게 살았다고 한다."
        };

        storyText.text = "";

        foreach(String line in ending){
            for (int i = 0; i < line.Length; i++)
            {
                storyText.text += line[i];
                yield return new WaitForSeconds(0.05f);
            }
            storyText.text += "\n\n";
            yield return new WaitForSeconds(1f);
        }

        while(!Input.anyKeyDown){
            yield return null;
        }

        storyText.text = "";
        currentState = "Title";
        StartCoroutine(openTitleTransition());
    }

}
