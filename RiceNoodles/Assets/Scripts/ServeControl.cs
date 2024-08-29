using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeControl : MonoBehaviour
{
    public float orderSpeed;
    public float orderAccel;
    public ServeReceipt[] receipts;
    public static List<int> orders;
    void Start() {
        orders = new List<int>(5) { -1, -1, -1, -1, -1 };
    }
    private Dictionary<int, String> dicStr = new Dictionary<int, String>(){
        {0, "기본"}, {1, "숙주"}, {2, "라임"}, {3, "숙주\n라임"},
        {4, "고수"}, {5, "숙주\n고수"}, {6, "라임\n고수"}, {7, "숙주\n라임\n고수"}
    };
    private Dictionary<int, int> dicLine = new Dictionary<int, int>(){
        {0, 1}, {1, 1}, {2, 1}, {3, 2},
        {4, 1}, {5, 2}, {6, 2}, {7, 3}
    };
    void Update() { // for test
        if (Input.GetKeyDown(KeyCode.A)) { newOrder(); }
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
    private float currentOrderSpeed() {
        return orderSpeed - orderAccel * StageManager.CurrentStage;
    }
    public void resetServe() {
        for (int i = 0; i < 5; i++) orders[i] = -1;
    }
}
