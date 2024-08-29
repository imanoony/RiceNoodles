using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIcon : MonoBehaviour
{
    public void moveIcon(float x, float y) {
        if (UIManager.CurrentState != "InGame") { return; }
        gameObject.transform.position = new Vector3(x, y, 0);
    }
    public void changeIcon(Sprite sprite) {
        if (UIManager.CurrentState != "InGame") { return; }
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public void resetIcon() {
        if (UIManager.CurrentState != "InGame") { return; }
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }
}
