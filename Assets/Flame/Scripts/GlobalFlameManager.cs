using System.Collections.Generic;
using UnityEngine;

public class GlobalFlameManager : MonoBehaviour
{
    public static GlobalFlameManager instance; // Make this a singleton

    private List<FlameController> flameControllers;

    private float innerRed, innerGreen, innerBlue,
        outerRed, outerGreen, outerBlue;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            innerRed = 255.0f;
            innerGreen = 127.5f;
            innerBlue = 0.0f;

            outerRed = 255.0f;
            outerGreen = 0.0f;
            outerBlue = 0.0f;

            instance = this;
            flameControllers = new List<FlameController>();
        }
    }

    public void RegisterFlameController(FlameController flameController)
    {
        flameControllers.Add(flameController);

        flameController.SetHexagonsInnerColor(innerRed, innerGreen, innerBlue);
        flameController.SetHexagonsOuterColor(outerRed, outerGreen, outerBlue);

        flameController.setNoiseSeed(flameControllers.Count * 1000);
    }

    public void UnregisterFlameController(FlameController flameController)
    {
        flameControllers.Remove(flameController);
    }

    public void OnColorChanged(float innerRedChannel, float innerGreenChannel, float innerBlueChannel, float outerRedChannel, float outerGreenChannel, float outerBlueChannel)
    {
        foreach (FlameController flameController in flameControllers)
        {
            flameController.SetHexagonsInnerColor(innerRedChannel, innerGreenChannel, innerBlueChannel);
            flameController.SetHexagonsOuterColor(outerRedChannel, outerGreenChannel, outerBlueChannel);
        }

        innerRed = innerRedChannel;
        innerGreen = innerGreenChannel;
        innerBlue = innerBlueChannel;
        outerRed = outerRedChannel;
        outerGreen = outerGreenChannel;
        outerBlue = outerBlueChannel;
    }

    // Include any other methods that operate on all flames
}