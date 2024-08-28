using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TargetCook : MonoBehaviour
{
    public Sprite dragIconSprite;
    public DragIcon dragIcon;
    public GameObject target;
    private RaycastHit2D ray;
    public float cookingTime = 10f;
    private float currentTime = 0f;
    private int status = 0; // 0: Bad, 1: Mid, 2: Good
    public Sprite[] utensilSprites;
    private SpriteRenderer spriteRenderer;
    private bool isCooking = false;
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void startCooking() {
        spriteRenderer.sprite = utensilSprites[1];
        StartCoroutine(cooking());
        isCooking = true;
    }
    IEnumerator cooking() {
        while (currentTime <= cookingTime) {
            // change sprite, change status
            currentTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.sprite = utensilSprites[3];
        status = 0;
    }
    void OnMouseDown() {
        if (!isCooking) { return; }
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragIcon.moveIcon(newPosition.x, newPosition.y);
        dragIcon.changeIcon(dragIconSprite);
        StopCoroutine(cooking());
    }
    void OnMouseDrag() {
        if (!isCooking) { return; }
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragIcon.moveIcon(newPosition.x, newPosition.y);
    }
    void OnMouseUp() {
        if (!isCooking) { return; }
        ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, 15f);
        if (ray) {
            if (ray.transform.name == target.transform.name) {
                spriteRenderer.sprite = utensilSprites[0];
                status = 0;
                TargetMix targetMix = target.transform.GetComponent<TargetMix>();
                int num = int.Parse(dragIcon.GetComponent<SpriteRenderer>().sprite.name[4].ToString());
                targetMix.startMixing(num + 1);
                TargetMix.score += status;
            }
        }
        else {
            StartCoroutine(cooking());
        }
        dragIcon.resetIcon();
        isCooking = false;
    }
}
