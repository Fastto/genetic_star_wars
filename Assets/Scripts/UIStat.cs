using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStat : MonoBehaviour
{
    [SerializeField] private GameObject stat;
    private void Start()
    {
        stat.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            stat.gameObject.SetActive(!stat.gameObject.activeSelf);
        }
    }
}
