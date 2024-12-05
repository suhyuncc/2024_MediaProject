using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    private List<string> list = new List<string>();

    private void Awake()
    {
        if (list.Contains(this.name))
        {
            Destroy(this.gameObject);
            return;
        }

        list.Add(this.name);
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("¿€µø");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
