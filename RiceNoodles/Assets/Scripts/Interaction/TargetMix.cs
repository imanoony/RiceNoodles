using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TargetMix : MonoBehaviour
{
    public static int score;
    public int result;
    void Awake() { score = 0; result = -1; }
    public void startMixing(int num) {
        gameObject.transform.GetChild(num).gameObject.SetActive(true);
        if (num == 3) result += 1;
        if (num == 5) result += 2;
        if (num == 6) result += 4;
    }
    public void resetMixing() { result = -1; }
    public int lastCheck() {
        if (!defaultCheck()) result = -1;
        else { result += 1; }
        int tmp = result;
        result = -1;
        return tmp;
    }
    private bool defaultCheck() {
        return gameObject.transform.GetChild(1).gameObject.activeSelf && 
        gameObject.transform.GetChild(2).gameObject.activeSelf &&
        gameObject.transform.GetChild(4).gameObject.activeSelf;
    }
}
