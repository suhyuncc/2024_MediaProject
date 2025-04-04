using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_Height : MonoBehaviour
{
    private Text _text;
    private RectTransform _rect;


    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        _rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        SettingTextWidth(_rect, _text, _text.text);
    }

    private void SettingTextWidth(RectTransform rectTrf, Text tmpText, string textValue)
    {
        tmpText.text = textValue;

        Vector2 rectSize = rectTrf.sizeDelta;
        rectSize.y = tmpText.preferredHeight;
        rectTrf.sizeDelta = rectSize;
    }
}
