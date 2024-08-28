using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas todayResultCanvas;
    [SerializeField] private Canvas MoneyCanvas;

    private TextMeshProUGUI moneyText;
    private TextMeshProUGUI todayResultText;

    private void Awake(){
        DontDestroyOnLoad(gameObject);

        moneyText = MoneyCanvas.transform.Find("Money/Amount").GetComponent<TextMeshProUGUI>();
        todayResultText = todayResultCanvas.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        MoneyCanvas.gameObject.SetActive(false);
        todayResultCanvas.gameObject.SetActive(false);
    }

    private void Update(){
        moneyText.text = MoneyManager.CurrentMoney.ToString() + "$";
    }

    private void Start(){
        MoneyCanvas.gameObject.SetActive(true);
    }

    public static void ShowTodayResult(){
        
    }
}
