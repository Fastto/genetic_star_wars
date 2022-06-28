using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameControl : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private TMP_Dropdown redStrategy;
    [SerializeField] private TMP_Dropdown blueStrategy;
    
    [SerializeField] private List<Strategy> strategies;

    public static Strategy RedStrategy;
    public static Strategy BlueStrategy;

    private void Awake()
    {
        if (RedStrategy == null)
        {
            RedStrategy = strategies[0];
        }
        
        if (BlueStrategy == null)
        {
            BlueStrategy = strategies[0];
        }
    }

    private void Start()
    {
        restartButton.onClick.AddListener(OnRestartClickHandler);

        int redIndex = 0;
        int blueIndex = 0;
        for (var i = 0; i < strategies.Count; i++)
        {
            if (strategies[i].name.Equals(RedStrategy.name))
            {
                redIndex = i;
            }
            
            if (strategies[i].name.Equals(BlueStrategy.name))
            {
                blueIndex = i;
            }
        }

        redStrategy.value = redIndex;
        blueStrategy.value = blueIndex;

    }

    private void OnRestartClickHandler()
    {
        //set strategy
        RedStrategy = strategies[redStrategy.value];
        BlueStrategy = strategies[blueStrategy.value];

        SceneManager.LoadScene(0);
    }

    public Strategy GetStrategy(Team team)
    {
        if (RedStrategy == null)
        {
            RedStrategy = strategies[0];
        }
        
        if (BlueStrategy == null)
        {
            BlueStrategy = strategies[0];
        }
        
        if (team.id == 1)
        {
            return RedStrategy;
        }

        return BlueStrategy;
    }
}
