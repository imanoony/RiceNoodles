using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBowl : MonoBehaviour
{
    public GameObject[] target;
    public ServeControl serveControl;
    private RaycastHit2D ray;
    void OnMouseDown() {
        if (UIManager.CurrentState != "InGame") { return; }
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0;
        gameObject.transform.position = newPosition;
    }
    void OnMouseDrag() {
        if (UIManager.CurrentState != "InGame") { return; }
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0;
        gameObject.gameObject.transform.position = newPosition;
    }
    void OnMouseUp() {
        if (UIManager.CurrentState != "InGame") { return; }
        ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, 15f, 
        LayerMask.GetMask("Default"));
        if (ray) {
            if (ray.transform.name == target[0].transform.name || ray.transform.name == target[1].transform.name) {
                serveControl.resetResult();
                for (int i = 1; i <= 6; i++) { gameObject.transform.GetChild(i).gameObject.SetActive(false); }
                TargetMix.score = 0;
            }
            else if (ray.transform.name == target[2].transform.name) {
                serveControl.checkResult();
                for (int i = 1; i <= 6; i++) { gameObject.transform.GetChild(i).gameObject.SetActive(false); }
                TargetMix.score = 0;
            }
        }
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }
}
