using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float energyConsumptionRate = 10f;
    public float rechargeRate = 5f;
    public float maxUsageTime = 30f;

    public bool isOn = false;
    public float currentEnergy;
    public float currentUsageTime;
    Light light;
    AudioManager manager;


    public Slider slider;
    public GameObject sliderObj;

    void Start()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("light");
        light = gameObject.GetComponent<Light>();
        currentEnergy = maxEnergy;
        light.enabled = false;
        manager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        slider.maxValue = maxEnergy;
        slider.value = currentEnergy;
        Debug.Log("value " + slider.value);
        if (isOn)
        {
            ConsumeEnergy();
            UpdateUsageTime();

            if (currentEnergy <= 0)
            {
                
                TurnOffFlashlight();
            }
        }
        else
        {
            manager.Stop("LightBuzz");
        }
    }

    void ConsumeEnergy()
    {
        currentEnergy -= energyConsumptionRate * Time.deltaTime;
        manager.Play("LightBuzz");

    }

    

    void UpdateUsageTime()
    {
        currentUsageTime -= Time.deltaTime;
    }

    public void ToggleFlashlight()
    {
        isOn = !isOn;
        Debug.Log("svetlo pred " + light.enabled);
        if (isOn)
        {
            light.enabled = true;
        }
        else
        {
            light.enabled = false;
        }

        manager.Play("LightSwitch");
        if (isOn && currentEnergy <= 0)
        {
            TurnOffFlashlight();
        }
    }

    void TurnOffFlashlight()
    {
        isOn = false;
        light.enabled = false;
        currentUsageTime = maxUsageTime;
        manager.Play("LightSwitch");
        manager.Stop("LightBuzz");
    }
}
