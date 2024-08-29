using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TargetMix : MonoBehaviour
{
    public static int score;
    public static int result;
    void Awake() { score = 0; result = -1; }
    public void startMixing(int num) {
        gameObject.transform.GetChild(num).gameObject.SetActive(true);
    }
}
