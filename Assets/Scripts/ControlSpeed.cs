using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlSpeed : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    // Start is called before the first frame update
    void Start()
    {
        SetSpeed(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSpeed(1);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSpeed(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSpeed(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetSpeed(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetSpeed(16);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetSpeed(0);
        }
    }

    private void SetSpeed(int speed)
    {
        Time.timeScale = speed;
        speedText.text = Time.timeScale.ToString();
    }
}
