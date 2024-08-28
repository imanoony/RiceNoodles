using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBowl : MonoBehaviour
{
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
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }
}
