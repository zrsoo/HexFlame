using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlamePropertyController : MonoBehaviour
{
    public GameObject flame;
    public TextMeshProUGUI speedGauge;

    private FlameMovementController flameMovementController;

    public void SetSpeed(float speed)
    {
        flameMovementController = flame.GetComponent<FlameMovementController>();
        flameMovementController.speed = speed;

        speedGauge.text = speed.ToString("0.00");
    }
}
