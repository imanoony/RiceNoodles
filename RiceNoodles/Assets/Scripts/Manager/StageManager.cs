using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static int currentStage;
    public static int CurrentStage => currentStage;

    private static Dictionary<string, bool> availableTopping; 
    public static Dictionary<string, bool> AvailableTopping => availableTopping;

    private void Awake(){
        DontDestroyOnLoad(gameObject);

        currentStage = 1;
        availableTopping = new Dictionary<string, bool>() {
            {"Lime", false},
            {"Gosu", false},
            {"Sukju", false}
        };
    }



    public static void NextStage(){
        currentStage++;
        MoneyManager.ResetMoney();
        MoneyManager.ResetToday();
        UpdateAvailableTopping();
    }

    public static bool Reset = false;
    public static void ResetStage(){
        currentStage = 1;
        MoneyManager.ResetMoney();
        MoneyManager.ResetToday();
        Reset = true;
    }



    public static void UpdateAvailableTopping(){
        if(currentStage == 1){
            availableTopping["Lime"] = false;
            availableTopping["Gosu"] = false;
            availableTopping["Sukju"] = false;
        }
        else if(currentStage == 2){
            availableTopping["Lime"] = true;
            availableTopping["Gosu"] = false;
            availableTopping["Sukju"] = false;
        }
        else if(currentStage == 3){
            availableTopping["Lime"] = true;
            availableTopping["Gosu"] = true;
            availableTopping["Sukju"] = false;
        }
        else if(currentStage == 4 || currentStage == 5){
            availableTopping["Lime"] = true;
            availableTopping["Gosu"] = true;
            availableTopping["Sukju"] = true;
        }
    }

}
