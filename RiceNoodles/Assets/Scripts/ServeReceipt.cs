using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ServeReceipt : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private String order;
    private int line;
    public void openReceipt(String str, int num) {
        order = str;
        line = num;
        StopAllCoroutines();
        StartCoroutine(openReceiptTransition());
    }
    public void closeReceipt() {
        StopAllCoroutines();
        StartCoroutine(closeReceiptTransition());
    }
    private void changeText() {
        tmp = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tmp.text = order;
        tmp.fontSize = (line == 1) ? 35 : (line == 2) ? 30 : 25;
    }
    private float startY = -50f, endY = -100f;
    private float startA = 0f, endA = 1f;
    IEnumerator openReceiptTransition() {
        changeText();
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        Image image = gameObject.GetComponent<Image>();
        Color color = image.color;
        tmp = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        while (rect.anchoredPosition.y >= endY) {
            float y = rect.anchoredPosition.y;
            y -= Time.deltaTime * 80f;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
            color.a += Time.deltaTime * 1.5f;
            image.color = color;
            tmp.color = new Color(0, 0, 0, color.a);
            yield return null;
        }
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, endY);
        color.a = endA;
        image.color = color;
        tmp.color = new Color(0, 0, 0, color.a);
        yield break;
    }
    IEnumerator closeReceiptTransition() {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        Image image = gameObject.GetComponent<Image>();
        Color color = image.color;
        tmp = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        while (rect.anchoredPosition.y <= startY) {
            float y = rect.anchoredPosition.y;
            y += Time.deltaTime * 80f;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
            color.a -= Time.deltaTime * 1.5f;
            image.color = color;
            tmp.color = new Color(0, 0, 0, color.a);
            yield return null;
        }
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, startY);
        color.a = startA;
        image.color = color;
        tmp.color = new Color(0, 0, 0, color.a);

        int num = int.Parse(gameObject.transform.name[7].ToString());
        ServeControl.orders[num] = -1;
        yield break;
    }
}
