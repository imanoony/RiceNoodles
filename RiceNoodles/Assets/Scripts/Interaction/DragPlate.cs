using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPlate : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite dragIconSprite;
    public DragIcon dragIcon;
    public GameObject target;
    public bool unlocked = true; // connect with other script
    private RaycastHit2D ray;
    void Start() {
        if (!unlocked) gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
    }
    public void unlockPlate() {
        unlocked = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
    }
    void OnMouseDown() {
        if (UIManager.CurrentState != "InGame") { return; }
        if (!unlocked) { return; }
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragIcon.moveIcon(newPosition.x, newPosition.y);
        dragIcon.changeIcon(dragIconSprite);
    }
    void OnMouseDrag() {
        if (UIManager.CurrentState != "InGame") { return; }
        if (!unlocked) { return; }
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragIcon.moveIcon(newPosition.x, newPosition.y);
    }
    void OnMouseUp() {
        if (UIManager.CurrentState != "InGame") { return; }
        if (!unlocked) { return; }
        ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, 15f);
        if (ray) {
            if (ray.transform.name == target.transform.name) {
                if (target.transform.tag == "Cook") {
                    TargetCook targetCook = target.transform.GetComponent<TargetCook>();
                    targetCook.startCooking();
                }
                else {
                    TargetMix targetMix = target.transform.GetComponent<TargetMix>();
                    int num = int.Parse(dragIcon.GetComponent<SpriteRenderer>().sprite.name[4].ToString());
                    targetMix.startMixing(num + 1);
                }
            }
        }
        dragIcon.resetIcon();
    }
}
