using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoController : MonoBehaviour
{
    [SerializeField] private ShipController ship;

    [SerializeField] private Slider cargoSlider;
    [SerializeField] private Slider damageSlider;
   
    [SerializeField] private UIGenomeRow genomeRow;
    
    [SerializeField] private ShipGenome defailtGenome;
    [SerializeField] private Team team;

    private void Awake()
    {
        ship.Team = team;
        ship.Genome = Instantiate(defailtGenome);
    }

    private void Start()
    {
        genomeRow.ApplyGenome(0, defailtGenome);

        cargoSlider.value = defailtGenome.cargoGen;
        damageSlider.value = defailtGenome.damageGen;
        
        cargoSlider.onValueChanged.AddListener(OnSlidersChangeHandler);
        damageSlider.onValueChanged.AddListener(OnSlidersChangeHandler);
    }

    private void OnSlidersChangeHandler(float val)
    {
        ship.Genome.cargoGen = cargoSlider.value;
        ship.Genome.damageGen = damageSlider.value;
        ship.ApplyGenome();
        genomeRow.ApplyGenome(0, ship.Genome);
    }
}
