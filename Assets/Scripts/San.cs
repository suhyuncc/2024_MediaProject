using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class San : MonoBehaviour
{
    [SerializeField]
    private Player_stat P_stat;
    [SerializeField]
    private Slider _sanGage;
    [SerializeField]
    private Image _sanFill;
    [SerializeField]
    private RawImage _sanImage;
    [SerializeField]
    private Texture[] _sanImages;
    [SerializeField]
    private Text _sanText;

    // Update is called once per frame
    void Update()
    {
        _sanText.text = $"{P_stat.C_san} / {P_stat.Max_san}";

        _sanGage.value = P_stat.C_san / (float)P_stat.Max_san;

        if(_sanGage.value > 0.75)
        {
            _sanImage.texture = _sanImages[0];
            _sanFill.color = new Color(1f, 1f, 1f);
        }
        else if (_sanGage.value > 0.3)
        {
            _sanImage.texture = _sanImages[1];
            _sanFill.color = new Color(1f, 1f, 1f);
        }
        else if (_sanGage.value > 0.15)
        {
            _sanImage.texture = _sanImages[2];
            _sanFill.color = new Color(1f, 1f, 1f);
        }
        else
        {
            _sanImage.texture = _sanImages[3];
            _sanFill.color = new Color(0.9921f, 0.2313f, 0.2235f);
        }
    }
}
