using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    private static float currentTime;
    public static float CurrentTime => currentTime;

    private static int currentHour;
    public static int CurrentHour => currentHour;

    private void Awake(){
        DontDestroyOnLoad(gameObject);

        currentTime = 0;
        currentHour = 10;
    }

    private void Update(){
        if(UIManager.CurrentState == "InGame"){
            currentTime += Time.deltaTime;

            if(currentTime >= 1){
                currentHour++;
                currentTime = 0;

                if(currentHour == 21){
                    currentHour = 10;
                    uiManager.ShowTodayResult();
                }
            }
        }
    }
}
