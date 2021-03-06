using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGenomeRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI holdScore;
    [SerializeField] private TextMeshProUGUI healthScore;
    [SerializeField] private TextMeshProUGUI damageScore;
    [SerializeField] private Slider healthDamageSlider;
    [SerializeField] private Slider holdSlider;

    private void Awake()
    {
        ApplyGenome(0, null);
    }

    public void ApplyGenome(int score, ShipGenome genome)
    {
        if (genome == null)
        {
            healthDamageSlider.gameObject.SetActive(false);
            holdSlider.gameObject.SetActive(false);
            scoreText.text = "";
        }
        else
        {
            if (!healthDamageSlider.gameObject.activeSelf)
            {
                healthDamageSlider.gameObject.SetActive(true);
                holdSlider.gameObject.SetActive(true);
            }

            scoreText.text = score.ToString();
            holdSlider.value = genome.GetCapacityPoints();
            healthDamageSlider.value = genome.GetCapacityPoints() + genome.GetHealthPoints();

            holdScore.text = Math.Round(genome.GetCapacityPoints(), 3).ToString();
            healthScore.text = Math.Round(genome.GetHealthPoints(), 3).ToString();
            damageScore.text = Math.Round(genome.GetDamagePoints(), 3).ToString();
        }
    }
}
