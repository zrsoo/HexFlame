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

    public TMP_Dropdown burningElementDropdown;

    private void Start()
    {
        flameMovementController = flame.GetComponent<FlameMovementController>();

        burningElementDropdown.onValueChanged.AddListener(delegate { ChangeFlameElement(); });
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

        SendInnerColors(redSlider.value, greenSlider.value, blueSlider.value);
    }

    private void SendInnerColors(float red, float green, float blue)
    {
        flameMovementController.innerRedChannel = red;
        flameMovementController.innerGreenChannel = green;
        flameMovementController.innerBlueChannel = blue;

        // Change the color of pre-existing flames.
        flameMovementController.OnColorChanged();
    }

    private void SendOuterColors(float red, float green, float blue)
    {
        flameMovementController.outerRedChannel = red;
        flameMovementController.outerGreenChannel = green;
        flameMovementController.outerBlueChannel = blue;

        // Change the color of pre-existing flames.
        flameMovementController.OnColorChanged();
    }

    public void SetGrowthChance(float chance)
    {
        flameMovementController.trailFlameGrowthChance = chance;

        float chanceText = chance * 100.0f;
        growthChanceGauge.text = chanceText.ToString("0") + " %";
    }

    private void ChangeFlameElement()
    {
        // Change color based on dropdown selection
        switch (burningElementDropdown.value)
        {
            // Default Fire
            case 0:
                SendInnerColors(255.0f, 127.5f, 0.0f);
                SendOuterColors(255.0f, 0.0f, 0.0f);

                redSlider.value = 255.0f;
                greenSlider.value = 127.5f;
                blueSlider.value = 0.0f;

                break;
            // Hydrogen
            case 1:
                SendInnerColors(153.0f, 153.0f, 255.0f);
                SendOuterColors(0.0f, 0.0f, 255.0f);

                redSlider.value = 153.0f;
                greenSlider.value = 153.0f;
                blueSlider.value = 255.0f;

                break;
            // Lithium
            case 2:
                SendInnerColors(255.0f, 25.0f, 25.0f);
                SendOuterColors(255.0f, 0.0f, 0.0f);

                redSlider.value = 255.0f;
                greenSlider.value = 25.0f;
                blueSlider.value = 25.0f;

                break;
            // Copper
            case 3:
                SendInnerColors(0.0f, 255.0f, 255.0f);
                SendOuterColors(51.0f, 204.0f, 51.0f);

                redSlider.value = 0.0f;
                greenSlider.value = 255.0f;
                blueSlider.value = 255.0f;

                break;
            // Potassium
            case 4:
                SendInnerColors(255.0f, 0.0f, 0.0f);
                SendOuterColors(102.0f, 0.0f, 102.0f);

                redSlider.value = 255.0f;
                greenSlider.value = 0.0f;
                blueSlider.value = 0.0f;

                break;
            default:
                Debug.Log("Invalid selection");
                break;
        }
    }
}
