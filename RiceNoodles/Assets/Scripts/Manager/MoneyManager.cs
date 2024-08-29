using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private static int currentMoney;
    public static int CurrentMoney => currentMoney;

    private static int todaySales;
    public static int TodaySales => todaySales;

    private static int todaySpending;
    public static int TodaySpending => todaySpending;

    private static int targetMoney;
    public static int TargetMoney => targetMoney;

    public static Dictionary<int, int> TargetMoneyList = new Dictionary<int, int>(){
        {1, 800},
        {2, 2000},
        {3, 4000},
        {4, 6000},
        {5, 8000}
    };

    public void Awake(){
        DontDestroyOnLoad(gameObject);

        currentMoney = 0;
    }

    public static void AddMoney(int money){
        currentMoney += money;
        AddTodaySales(money);
    }
    public static void SpendMoney(int money){
        currentMoney -= money;
        todaySpending += money;
    }
    public static void ResetMoney(){
        currentMoney = 0;
    }

    public static void SetTargetMoney(int money){
        targetMoney = money;
    }

    public static void AddTodaySales(int sales){
        todaySales += sales;
    }

    public static void ResetToday(){
        todaySales = 0;
        todaySpending = 0;
    }

}
