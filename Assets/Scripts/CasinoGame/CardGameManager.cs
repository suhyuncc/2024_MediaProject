using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase
{
    standby,
    main,
    dice,
    result
}

public class CardGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cards;

    private Phase phase;

    // Start is called before the first frame update
    void Start()
    {
        phase = Phase.standby;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Set()
    {

    }
}
