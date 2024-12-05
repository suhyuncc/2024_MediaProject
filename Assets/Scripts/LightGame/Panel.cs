using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    private Button _button;
    private AudioSource _sfx;

    // Start is called before the first frame update
    void Awake()
    {
        _sfx = this.GetComponent<AudioSource>();
        _button = this.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.Rotate(new Vector3(0f, 0f, 90f) * Time.deltaTime);
    }

    public void Spin()
    {
        StartCoroutine(spin());
        _sfx.Play();
    }

    IEnumerator spin()
    {
        float init_z = this.transform.rotation.eulerAngles.z;

        float speed = 3.0f;
        int count = 0;

        while (count < (int)(90 / speed))
        {
            count++;
            _button.enabled = false;
            this.transform.Rotate(new Vector3(0f, 0f, speed));
            yield return null;
        }

        _button.enabled = true;
        MiniGameManager.instance.End_Checking();
    }
}
