using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBowl : MonoBehaviour
{
    public GameObject[] target;
    private RaycastHit2D ray;
    void OnMouseDown() {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0;
        gameObject.transform.position = newPosition;
    }
    void OnMouseDrag() {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0;
        gameObject.gameObject.transform.position = newPosition;
    }
    void OnMouseUp() {
        ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, 15f, 
        LayerMask.GetMask("Default"));
        if (ray) {
            if (ray.transform.name == target[0].transform.name || ray.transform.name == target[1].transform.name) {
                // Throw away
                for (int i = 1; i <= 6; i++) { gameObject.transform.GetChild(i).gameObject.SetActive(false); }
                TargetMix.score = 0;
            }
            else if (ray.transform.name == target[2].transform.name) {
                // Serve
                for (int i = 1; i <= 6; i++) { gameObject.transform.GetChild(i).gameObject.SetActive(false); }
                TargetMix.score = 0;
            }
        }
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }
}
