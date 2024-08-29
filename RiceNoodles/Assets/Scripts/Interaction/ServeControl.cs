using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ServeControl : MonoBehaviour
{
    public TargetMix targetMix;
    public float orderSpeed;
    public float orderAccel;
    public ServeReceipt[] receipts;
    public static List<int> orders;
    void Start() {
        orders = new List<int>(5) { -1, -1, -1, -1, -1 };
        currentStage = StageManager.CurrentStage;
        currentTime = 0f;
        currentOrderSpeed = orderSpeed;
        newOrder();
        //plates[currentStage - 2].unlockPlate();
        MoneyManager.SetTargetMoney(MoneyManager.TargetMoneyList[currentStage]);
    }
    private Dictionary<int, String> dicStr = new Dictionary<int, String>(){
        {0, "기본"}, {1, "숙주"}, {2, "라임"}, {3, "숙주\n라임"},
        {4, "고수"}, {5, "숙주\n고수"}, {6, "라임\n고수"}, {7, "숙주\n라임\n고수"}
    };
    private Dictionary<int, int> dicLine = new Dictionary<int, int>(){
        {0, 1}, {1, 1}, {2, 1}, {3, 2},
        {4, 1}, {5, 2}, {6, 2}, {7, 3}
    };
    private float currentTime, currentOrderSpeed;
    private int currentStage;
    public DragPlate[] plates;
    void Update() {
        if (StageManager.CurrentStage != currentStage || StageManager.Reset) {
            StageManager.Reset = false;
            currentStage = StageManager.CurrentStage;
            currentTime = 0f;
            currentOrderSpeed = orderSpeed - orderAccel * (StageManager.CurrentStage - 1);
            TargetMix.score = 0;
            resetServe();
            if (currentStage > 1) plates[currentStage - 2].unlockPlate();
            MoneyManager.SetTargetMoney(MoneyManager.TargetMoneyList[currentStage]);
            newOrder();
        }
        if (UIManager.CurrentState != "InGame") return;
        if (currentTime >= currentOrderSpeed + UnityEngine.Random.Range(0.0f, 5.0f)) {
            newOrder();
            currentTime = 0f;
        }
        currentTime += Time.deltaTime;
    }
    public void checkResult() {
        int result = targetMix.lastCheck();
        for (int i = 0; i < 5; i++) {
            if (orders[i] == result) {
                receipts[i].closeReceipt();
                MoneyManager.AddMoney(50 * (TargetMix.score + 1) * (result / 3 + 1) * EnchantManager.calculateEnchant(orders[i]) / 10);
                TargetMix.score = 0;
                return;
            }
        }
    }
    private void newOrder() {
        for (int i = 0; i < 5; i++) {
            if (orders[i] == -1) {
                int num = generateOrder();
                orders[i] = num;
                receipts[i].openReceipt(dicStr[num], dicLine[num]);
                return;
            }
        }
    }
    private int generateOrder() {
        int r = UnityEngine.Random.Range(0, 8);
        switch (StageManager.CurrentStage) {
            case 1:
                return 0;
            case 2:
                return (int)(r/4);
            case 3:
                return (int)(r/2);
            default:
                return r;
        }
    }
    public void resetServe() {
        for (int i = 0; i < 5; i++) {
            orders[i] = -1;
            receipts[i].closeReceipt();
        }
    }
}
