using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlamePropertyController : MonoBehaviour
{
    public GameObject flame;

    public TextMeshProUGUI speedGauge;
    public TextMeshProUGUI redGauge;
    public TextMeshProUGUI greenGauge;
    public TextMeshProUGUI blueGauge;
    public TextMeshProUGUI growthChanceGauge;

    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    private FlameMovementController flameMovementController;

    private void Start()
    {
        flameMovementController = flame.GetComponent<FlameMovementController>();
    }

    public void SetSpeed(float speed)
    {
        flameMovementController.speed = speed;

        speedGauge.text = speed.ToString("0.00");
    }

    public void SetColor()
    {
        redGauge.text = redSlider.value.ToString("0.0");
        greenGauge.text = greenSlider.value.ToString("0.0");
        blueGauge.text = blueSlider.value.ToString("0.0");

        SendColors(redSlider.value, greenSlider.value, blueSlider.value);
    }

    private void SendColors(float red, float green, float blue)
    {
        flameMovementController.red = red;
        flameMovementController.green = green;
        flameMovementController.blue = blue;

        // Change the color of pre-existing flames.
        flameMovementController.OnColorChanged();
    }

    public void SetGrowthChance(float chance)
    {
        flameMovementController.trailFlameGrowthChance = chance;

        float chanceText = chance * 100.0f;
        growthChanceGauge.text = chanceText.ToString("0") + " %";
    }
}
