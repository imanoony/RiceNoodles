using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPlate : MonoBehaviour
{
    public Sprite dragIconSprite;
    public DragIcon dragIcon;
    public GameObject target;
    private RaycastHit2D ray;
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
        ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, 15f);
        if (ray) {
            if (ray.transform.name == target.transform.name) {
                Debug.Log("target");
                if (target.transform.tag == "Cook") {
                    TargetCook targetCook = target.transform.GetComponent<TargetCook>();
                    targetCook.startCooking();
                }
                else {
                    TargetMix targetMix = target.transform.GetComponent<TargetMix>();
                    targetMix.startMixing();
                }
            }
        }
        dragIcon.resetIcon();
    }
}
