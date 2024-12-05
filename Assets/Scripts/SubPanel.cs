using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPanel : MonoBehaviour
{
    [SerializeField]
    private Animator _anim;

    private void OnEnable()
    {
        _anim.SetTrigger("Pade");
    }
}
