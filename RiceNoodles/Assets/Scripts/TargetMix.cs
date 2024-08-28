using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TargetMix : MonoBehaviour
{
    public static int score;
    void Awake() { score = 0; }
    public void startMixing() {
        Debug.Log("Mix");
    }
}
