using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnchantManager : MonoBehaviour
{
    [SerializeField] private Canvas EnchantCanvas;
    [SerializeField] private Sprite[] ItemIcons;
    [SerializeField] private Sprite[] EnchantButtonSprites;

    private Button noodleButton;
    private Button gravyButton;
    private Button beefButton;
    private Button sukjuButton;
    private Button limeButton;
    private Button gosuButton;

    private TextMeshProUGUI noodleText;
    private TextMeshProUGUI gravyText;
    private TextMeshProUGUI beefText;
    private TextMeshProUGUI sukjuText;
    private TextMeshProUGUI limeText;
    private TextMeshProUGUI gosuText;

    private TextMeshProUGUI currentMoneyText;
    private TextMeshProUGUI needMoneyText;

    private Image itemIconImage;
    private Image successChanceImage;

    private Button enchantButton;
    private Image enchantButtonImage;

    private TextMeshProUGUI successChanceText;

    private static String currentEnchant;


    private static Dictionary<int, int> enchantChance = new Dictionary<int, int>(){
        {1, 80},
        {2, 60},
        {3, 40},
        {4, 30},
        {5, 20},
        {6, 10}
    };
    
    private static Dictionary<string, int> enchantCost = new Dictionary<string, int>(){
        {"Noodle", 150},
        {"Gravy", 180},
        {"Beef", 200},
        {"Sukju", 70},
        {"Lime", 80},
        {"Gosu", 90}
    };

    private static List<int> sukjuUsed = new List<int>(){0, 1, 0, 1, 0, 1, 0, 1};
    private static List<int> limeUsed = new List<int>(){0, 0, 1, 1, 0, 0, 1, 1};
    private static List<int> gosuUsed = new List<int>(){0, 0, 0, 0, 1, 1, 1, 1};

    private static Dictionary<string, int> enchantLevel;
    public static int calculateEnchant(int receiptType) {
        int result = 0;
        result += EnchantManager.enchantLevel["Noodle"];
        result += EnchantManager.enchantLevel["Gravy"];
        result += EnchantManager.enchantLevel["Beef"];
        result += (int)(EnchantManager.enchantLevel["Sukju"] * 0.6) * sukjuUsed[receiptType];
        result += (int)(EnchantManager.enchantLevel["Lime"] * 0.6) * limeUsed[receiptType];
        result += (int)(EnchantManager.enchantLevel["Gosu"] * 0.6) * gosuUsed[receiptType];
        return result;
    }
    public void Awake(){
        DontDestroyOnLoad(gameObject);

        enchantLevel = new Dictionary<string, int>(){
            {"Noodle", 1},
            {"Gravy", 1},
            {"Beef", 1},
            {"Sukju", 1},
            {"Lime", 1},
            {"Gosu", 1}
        };

        noodleButton = EnchantCanvas.transform.Find("Buttons/Noodle").GetComponent<Button>();
        gravyButton = EnchantCanvas.transform.Find("Buttons/Gravy").GetComponent<Button>();
        beefButton = EnchantCanvas.transform.Find("Buttons/Beef").GetComponent<Button>();
        sukjuButton = EnchantCanvas.transform.Find("Buttons/Sukju").GetComponent<Button>();
        limeButton = EnchantCanvas.transform.Find("Buttons/Lime").GetComponent<Button>();
        gosuButton = EnchantCanvas.transform.Find("Buttons/Gosu").GetComponent<Button>();

        noodleButton.onClick.AddListener(() => OnClickSetEnchant(noodleButton.gameObject));
        gravyButton.onClick.AddListener(() => OnClickSetEnchant(gravyButton.gameObject));
        beefButton.onClick.AddListener(() => OnClickSetEnchant(beefButton.gameObject));
        sukjuButton.onClick.AddListener(() => OnClickSetEnchant(sukjuButton.gameObject));
        limeButton.onClick.AddListener(() => OnClickSetEnchant(limeButton.gameObject));
        gosuButton.onClick.AddListener(() => OnClickSetEnchant(gosuButton.gameObject));

        noodleText = EnchantCanvas.transform.Find("Descriptions/Noodle").GetComponent<TextMeshProUGUI>();
        gravyText = EnchantCanvas.transform.Find("Descriptions/Gravy").GetComponent<TextMeshProUGUI>();
        beefText = EnchantCanvas.transform.Find("Descriptions/Beef").GetComponent<TextMeshProUGUI>();
        sukjuText = EnchantCanvas.transform.Find("Descriptions/Sukju").GetComponent<TextMeshProUGUI>();
        limeText = EnchantCanvas.transform.Find("Descriptions/Lime").GetComponent<TextMeshProUGUI>();
        gosuText = EnchantCanvas.transform.Find("Descriptions/Gosu").GetComponent<TextMeshProUGUI>();

        currentMoneyText = EnchantCanvas.transform.Find("Money/Current Money Text").GetComponent<TextMeshProUGUI>();
        needMoneyText = EnchantCanvas.transform.Find("Money/Need Money Text").GetComponent<TextMeshProUGUI>();

        itemIconImage = EnchantCanvas.transform.Find("Item").GetComponent<Image>();
        successChanceImage = EnchantCanvas.transform.Find("Success").GetComponent<Image>();

        successChanceText = EnchantCanvas.transform.Find("Success Chance").GetComponent<TextMeshProUGUI>();

        enchantButton = EnchantCanvas.transform.Find("Enchant Button").GetComponent<Button>();
        enchantButtonImage = enchantButton.GetComponent<Image>();

        enchantButton.onClick.AddListener(() => OnClickEnchant());

        currentEnchant = "";
    }

    private void Update(){
        currentMoneyText.text = MoneyManager.CurrentMoney + "$";

        if(currentEnchant != "" && enchantLevel[currentEnchant] == 6){
            currentEnchant = "";
        }

        if(currentEnchant != ""){
            successChanceText.text = "성공 확률\n" + enchantChance[enchantLevel[currentEnchant]] + "%";
            successChanceImage.fillAmount = (float)enchantChance[enchantLevel[currentEnchant]] / 100;
            enchantButton.gameObject.SetActive(true);
            needMoneyText.text = enchantCost[currentEnchant] * enchantLevel[currentEnchant] + "$";
        }
        else{
            successChanceText.text = "";
            successChanceImage.fillAmount = 0;
            enchantButton.gameObject.SetActive(false);
            needMoneyText.text = "";
        }

        switch(currentEnchant){
            case "":
                itemIconImage.sprite = ItemIcons[6];
                break;
            case "Noodle":
                itemIconImage.sprite = ItemIcons[0];
                break;
            case "Gravy":
                itemIconImage.sprite = ItemIcons[1];
                break;
            case "Beef":
                itemIconImage.sprite = ItemIcons[2];
                break;
            case "Sukju":
                itemIconImage.sprite = ItemIcons[3];
                break;
            case "Lime":
                itemIconImage.sprite = ItemIcons[4];
                break;
            case "Gosu":
                itemIconImage.sprite = ItemIcons[5];
                break;
            default:
                break;
        }

        noodleText.text = "면 Lv." + enchantLevel["Noodle"];
        gravyText.text = "육수 Lv." + enchantLevel["Gravy"];
        beefText.text = "고기 Lv." + enchantLevel["Beef"];
        sukjuText.text = "숙주 Lv." + enchantLevel["Sukju"];
        limeText.text = "라임 Lv." + enchantLevel["Lime"];
        gosuText.text = "고수 Lv." + enchantLevel["Gosu"];
    }

    public static void InitiateEnchant(){
        currentEnchant = "";
    }

    private void OnClickSetEnchant(GameObject clickedButton){
        currentEnchant = clickedButton.name;
    }

    private void OnClickEnchant(){
        StartCoroutine(ButtonClickCoroutine());

        if(MoneyManager.CurrentMoney >= enchantCost[currentEnchant] * enchantLevel[currentEnchant]){
            MoneyManager.SpendMoney(enchantCost[currentEnchant] * enchantLevel[currentEnchant]);
            if(enchantChance[enchantLevel[currentEnchant]] >= (UnityEngine.Random.Range(0f, 1f) * 100)) enchantLevel[currentEnchant]++;
        }
    }

    private IEnumerator ButtonClickCoroutine(){
        enchantButtonImage.sprite = EnchantButtonSprites[1];
        yield return new WaitForSecondsRealtime(0.3f);
        enchantButtonImage.sprite = EnchantButtonSprites[0];
    }

}
