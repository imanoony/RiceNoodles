using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIcon : MonoBehaviour
{
    public void moveIcon(float x, float y) {
        gameObject.transform.position = new Vector3(x, y, 0);
    }
    public void changeIcon(Sprite sprite) {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public void resetIcon() {
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }
}
