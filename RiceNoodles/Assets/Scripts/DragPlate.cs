using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPlate : MonoBehaviour
{
    public Sprite dragIconSprite;
    public DragIcon dragIcon;
    void OnMouseDown() {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragIcon.moveIcon(newPosition.x, newPosition.y);
        dragIcon.changeIcon(dragIconSprite);
    }
    void OnMouseDrag() {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragIcon.moveIcon(newPosition.x, newPosition.y);
    }
    void OnMouseUp() {
        dragIcon.resetIcon();
    }
}
