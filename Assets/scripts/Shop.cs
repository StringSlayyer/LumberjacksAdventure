using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Shop : MonoBehaviour

{

    public GameObject buttons;
    public GameObject sell;
    public GameObject upgrade;
    public GameObject lights;
    private PlayerMovement player;
    private PlayerAssistant playerAssistant;
    public GameObject assistant;
    public GameObject spawnPoint;
    public GameObject flashlightObj;
    new Light light;
    Flashlight flashlight;

    GameData gameData;




    public TextMeshProUGUI shopTree;
    public TextMeshProUGUI shopMoney;
    public TextMeshProUGUI upgradeMoney;
    public TextMeshProUGUI lightMoney;
    public GameObject warning;

    public GameObject text;
    public GameObject panel;
    public GameObject slider;
    public GameObject stats;
    public GameObject objectives;

    private float assistantPrice = 250;
    private float upgrPDmgPrice = 100;
    public TextMeshProUGUI upgradePlayerDamagePrice;
    public GameObject buyA;
    public GameObject upgradeA;
    public GameObject buyL;
    public GameObject upgradeL;

    public GameObject currPDmg;
    public GameObject maxPDmg;
    public GameObject currPSpd;
    public GameObject maxPSpd;
    public GameObject currAInv;
    public GameObject maxAInv;
    public GameObject currASpd;
    public GameObject maxASpd;


    public GameObject currLCap;
    public GameObject maxLCap;
    public GameObject currLRange;
    public GameObject maxLRange;
    public GameObject currLAngle;
    public GameObject maxLAngle;

    AudioManager audioManager;


    private float upgrASpeedPrice = 50;
    public TextMeshProUGUI SpeedUpgradeAPrice;
    private float upgrAInvPrice = 50;
    public TextMeshProUGUI InventoryUpgradeAPrice;

    private float upgrPSpeedPrice = 50;
    public TextMeshProUGUI SpeedUpgradePPrice;


    private float rechargePrice = 50;
    public TextMeshProUGUI BatteryRechargePrice;
    private float upgrLCapacityPrice = 100;
    public TextMeshProUGUI CapacityUpgradeLPrice;
    private float upgrLRangePrice = 50;
    public TextMeshProUGUI RangeUpgradeLPrice;
    private float upgrLAnglePrice = 50;
    public TextMeshProUGUI AngleUpgradeLPrice;



    private int maxDamage = 100;
    private int maxInventory = 60;
    private float maxSpeed = 15f;

    private float maxAngle = 75f;
    private float maxCap = 500;
    private float maxRange = 40f;


    private bool isPlayer;
    private bool wasPlayer = false;
    private bool assistentBought = false;


    public void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerMovement>();
        }

        Collider[] objects = Physics.OverlapSphere(transform.position, 50f);
        int trees = 0;
        foreach (Collider collider in objects)
        {
            if (collider.gameObject.CompareTag("tree"))
            {
                trees++;
                Destroy(collider.gameObject);
            }
        }

        gameData = FindObjectOfType<GameData>();
        if (gameData != null)
        {
            assistentBought = gameData.isAssistant;
            if (assistentBought)
            {
                Instantiate(assistant, spawnPoint.transform.position, Quaternion.identity);
                buyA.SetActive(false);
                upgradeA.SetActive(true);
                FindAssistant();
            }
            if (gameData.wasUpgraded == true)
            {
                upgrPDmgPrice = gameData.upgrPDmgPrice;
                upgrPSpeedPrice = gameData.upgrPSpeedPrice;
                upgrAInvPrice = gameData.upgrAInvPrice;
                upgrASpeedPrice = gameData.upgrASpeedPrice;
                CheckMaxUpgrades();
            }
            else
            {
                gameData.upgrPDmgPrice = upgrPDmgPrice;
                gameData.upgrPSpeedPrice = upgrPSpeedPrice;
                gameData.upgrAInvPrice = upgrAInvPrice;
                gameData.upgrASpeedPrice = upgrASpeedPrice;
            }
        }

        audioManager = FindObjectOfType<AudioManager>();


        upgradePlayerDamagePrice.text = upgrPDmgPrice.ToString();
        SpeedUpgradeAPrice.text = upgrASpeedPrice.ToString();
        SpeedUpgradePPrice.text = upgrPSpeedPrice.ToString();
        InventoryUpgradeAPrice.text = upgrAInvPrice.ToString();

        
    }

    private void Update()
    {
        shopTree.text = player.treeCount.ToString();
        shopMoney.text = player.money.ToString();
        upgradeMoney.text = player.money.ToString();
        if(lightMoney != null)
        {
            lightMoney.text = player.money.ToString();
        }

        CheckPlayer();

        if (Input.GetKeyDown(KeyCode.E) && isPlayer)
        {
            text.SetActive(false);
            panel.SetActive(true);
            stats.SetActive(false);
            if (slider != null)
            {
                slider.SetActive(false);
            }
            objectives.SetActive(false);
            audioManager.Play("Open");
            PauseGame();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && panel.activeSelf)
        {
            if (upgrade.activeSelf)
            {
                upgrade.gameObject.SetActive(false);
                buttons.SetActive(true);
            } else if (sell.activeSelf)
            {
                sell.gameObject.SetActive(false);
                buttons.SetActive(true);
            } else if (lights != null && lights.activeSelf)
            {
                lights.gameObject.SetActive(false);
                buttons.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
                if (slider != null && gameData.flashlight)
                {
                    slider.SetActive(true);
                }
                stats.SetActive(true);
                objectives.SetActive(true);
                audioManager.Play("Open");
                ResumeGame();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void OpenSell()
    {
        buttons.SetActive(false);
        sell.SetActive(true);
        warning.SetActive(false);
    }

    public void OpenUpgrade()
    {
        buttons.SetActive(false);
        upgrade.SetActive(true);
    }

    public void OpenLight()
    {
        buttons.SetActive(false);
        lights.SetActive(true);
    }

    public void SellAllTrees()
    {
        if (player.treeCount >= 1)
        {
            warning.SetActive(false);
            player.money += (player.treeCount * 10);
            gameData.TotalEarnedMoney += (player.treeCount * 10);
            player.treeCount = 0;
        }
        else
        {
            warning.SetActive(true);
        }
    }

    public void SellSingleTree()
    {
        if (player.treeCount >= 1)
        {
            warning.SetActive(false);
            player.money += 10;
            gameData.TotalEarnedMoney += 10;
            player.treeCount--;
        }
        else
        {
            warning.SetActive(true);
        }
    }

    private void CheckPlayer()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, 15f);

        isPlayer = false;

        foreach (Collider collider in objects)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                isPlayer = true;
                break;
            }
        }

        if (isPlayer != wasPlayer)
        {
            if (isPlayer)
            {
                text.SetActive(!panel.activeSelf);
            }
            else
            {
                text.SetActive(false);
            }
        }
        wasPlayer = isPlayer;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void buyAssistant()
    {
        if (player.money >= assistantPrice && assistentBought == false)
        {
            player.money -= (int)assistantPrice;
            gameData.TotalSpentMoney += (int)assistantPrice;
            Instantiate(assistant, spawnPoint.transform.position, Quaternion.identity);
            assistentBought = true;
            buyA.SetActive(false);
            upgradeA.SetActive(true);
            FindAssistant();
            gameData.isAssistant = true;
        }
    }

    public void upgrPlayerDmg()
    {
        if (player.money >= upgrPDmgPrice && player.punchDamage < maxDamage) {
            player.money -= (int)upgrPDmgPrice;
            gameData.TotalSpentMoney += (int) upgrPDmgPrice;
            player.punchDamage = (int)(Mathf.Floor(player.punchDamage * 2));
            if (player.punchDamage >= maxDamage)
            {
                player.punchDamage = maxDamage;
                currPDmg.SetActive(false);
                maxPDmg.SetActive(true);
            }
            upgrPDmgPrice = Mathf.Floor((float)(upgrPDmgPrice * 1.25));
            gameData.upgrPDmgPrice = upgrPDmgPrice;
            upgradePlayerDamagePrice.text = upgrPDmgPrice.ToString();
        }
    }

    public void UpgradeAssistantSpeed()
    {
        if (player.money >= upgrASpeedPrice)
        {
            player.money -= (int)upgrASpeedPrice;
            gameData.TotalSpentMoney += (int)upgrASpeedPrice;
            playerAssistant.moveSpeed *= 1.25f;
            upgrASpeedPrice = Mathf.Floor((float)(upgrASpeedPrice * 1.5));
            if (playerAssistant.moveSpeed >= maxSpeed)
            {
                playerAssistant.moveSpeed = maxSpeed;
                currASpd.SetActive(false);
                maxASpd.SetActive(true);
            }
            SpeedUpgradeAPrice.text = upgrASpeedPrice.ToString();
            if (gameData.wasUpgraded == false)
            {
                gameData.wasUpgraded = true;
            }
            gameData.upgrASpeedPrice = upgrASpeedPrice;
            gameData.assistantSpeed = playerAssistant.moveSpeed;
        }
    }

    public void UpgradePlayerSpeed()
    {
        if (player.money >= upgrPSpeedPrice)
        {
            player.money -= (int)upgrPSpeedPrice;
            gameData.TotalSpentMoney += (int)upgrPSpeedPrice;
            player.moveSpeed *= 1.25f;
            upgrPSpeedPrice = Mathf.Floor((float)(upgrPSpeedPrice * 1.5));
            if (player.moveSpeed >= maxSpeed)
            {
                player.moveSpeed = maxSpeed;
                currPSpd.SetActive(false);
                maxPSpd.SetActive(true);
            }
            gameData.upgrPSpeedPrice = upgrPSpeedPrice;
            SpeedUpgradePPrice.text = upgrPSpeedPrice.ToString();
        }
    }

    public void UpgradeAssistantInventory()
    {
        if (player.money >= upgrAInvPrice)
        {
            player.money -= (int)(upgrAInvPrice);
            gameData.TotalSpentMoney += (int)upgrAInvPrice;
            playerAssistant.maxInventory = (int)(playerAssistant.maxInventory * 1.25);
            upgrAInvPrice = Mathf.Floor((float)(upgrAInvPrice * 1.5));
            if (playerAssistant.maxInventory >= maxInventory)
            {
                playerAssistant.maxInventory = maxInventory;
                currAInv.SetActive(false);
                maxAInv.SetActive(true);
            }
            gameData.wasUpgraded = true;
            gameData.upgrAInvPrice = upgrAInvPrice;
            InventoryUpgradeAPrice.text = upgrAInvPrice.ToString();
            gameData.assistantMInventory = playerAssistant.maxInventory;
        }
    }

    public void FindLight()
    {
        GameObject flashlightO = GameObject.FindGameObjectWithTag("flashlight");
        GameObject lightObj = GameObject.FindGameObjectWithTag("light");
        light = lightObj.GetComponent<Light>();
        flashlight = flashlightO.GetComponent<Flashlight>();
    }

    public void buyFlashlight()
    {
        if (player.money >= 250)
        {
            player.money -= 250;
            gameData.TotalSpentMoney += 250;
            flashlightObj.SetActive(true);
            buyL.SetActive(false);
            upgradeL.SetActive(true);
            player.FindFlashlight();
            FindLight();
            UpdateBatteryPrices();
            gameData.flashlight = true;
        }
    }

    public void ChargeBattery()
    {
        if (player.money >= rechargePrice)
        {
            player.money -= (int)rechargePrice;
            gameData.TotalSpentMoney += (int)rechargePrice;
            flashlight.currentEnergy = flashlight.maxEnergy;
        }
    }

    public void UpgradeRange()
    {
        if (player.money >= upgrLRangePrice)
        {
            player.money -= (int)upgrLRangePrice;
            gameData.TotalSpentMoney += (int)upgrLRangePrice;
            light.range *= 1.1f;
            if (light.range >= maxRange)
            {
                light.range = maxRange;
                currLRange.SetActive(false);
                maxLRange.SetActive(true);
            }
            gameData.flashUpgrade = true;
            upgrLRangePrice = Mathf.Floor((float)(upgrLRangePrice * 1.5));
            RangeUpgradeLPrice.text = upgrLRangePrice.ToString();

        }
    }

    public void UpgradeCapacity()
    {
        if (player.money >= upgrLCapacityPrice)
        {
            player.money -= (int)upgrLCapacityPrice;
            gameData.TotalSpentMoney += (int)upgrLCapacityPrice;
            float energyRatio = flashlight.currentEnergy / flashlight.maxEnergy;

            flashlight.maxEnergy *= 1.5f;
            flashlight.currentEnergy = flashlight.maxEnergy * energyRatio;
            flashlight.currentEnergy = Mathf.Min(flashlight.currentEnergy, flashlight.maxEnergy);
            if (flashlight.maxEnergy >= maxCap)
            {
                flashlight.maxEnergy = maxCap;
                currLCap.SetActive(false);
                maxLCap.SetActive(true);
            }
            gameData.flashUpgrade = true;
            upgrLCapacityPrice = Mathf.Floor((float)(upgrLCapacityPrice * 1.5));
            CapacityUpgradeLPrice.text = upgrLCapacityPrice.ToString();
        }
    }

    public void UpgradeAngle()
    {
        if (player.money >= upgrLAnglePrice)
        {
            player.money -= (int)upgrLAnglePrice;
            gameData.TotalSpentMoney += (int)upgrLAnglePrice;
            light.spotAngle *= 1.1f;
            if (light.spotAngle >= maxAngle)
            {
                light.spotAngle = maxAngle;
                currLAngle.SetActive(false);
                maxLAngle.SetActive(true);
            }
            gameData.flashUpgrade = true;
            upgrLAnglePrice = Mathf.Floor((float)(upgrLAnglePrice * 1.5));
            AngleUpgradeLPrice.text = upgrLAnglePrice.ToString();
        }
    }

    public void FindAssistant()
    {
        GameObject assistantObj = GameObject.FindGameObjectWithTag("helper");
        playerAssistant = assistantObj.GetComponent<PlayerAssistant>();
    }

    private void CheckMaxUpgrades()
    {
        if (player.punchDamage == maxDamage)
        {
            currPDmg.SetActive(false);
            maxPDmg.SetActive(true);
        }
        if (playerAssistant.moveSpeed == maxSpeed)
        {
            currASpd.SetActive(false);
            maxASpd.SetActive(true);
        }
        if (player.moveSpeed == maxSpeed)
        {
            currPSpd.SetActive(false);
            maxPSpd.SetActive(true);
        }
        if (playerAssistant.maxInventory == maxInventory)
        {
            currAInv.SetActive(false);
            maxAInv.SetActive(true);
        }
    }

    private void UpdateBatteryPrices()
    {
        BatteryRechargePrice.text = rechargePrice.ToString();
        CapacityUpgradeLPrice.text = upgrLCapacityPrice.ToString();
        RangeUpgradeLPrice.text = upgrLRangePrice.ToString();
        AngleUpgradeLPrice.text = upgrLAnglePrice.ToString();
    }

}
