using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BuildingController : MonoBehaviour
{
    private enum FactoryState
    {
        Normal,
        Destoryed
    }
    
    public enum BuildingType
    {
        MainTown,
        ResearchLab,
        Farm,
        Turret,
        Shield
    }
    
    [Header(" -- Reference -- ")]
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private Text upgradeCostText;
    [SerializeField] private Image HPBarPercentage;
    [SerializeField] private Text currentLevelText;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shieldPrefab;

    [Header(" -- Parameters -- ")] 
    [SerializeField] private int initialHP = 10;
    [SerializeField] private int hpIncrementPerLevel = 5;
    [SerializeField] private int buildingType;
    [SerializeField] private int initialUpgradeCost = 100;
    [SerializeField] private int upgradeCostIncrement = 50;
    [SerializeField] private int initialGoldOutput = 0;
    [SerializeField] private int goldOutputIncrement = 2;
    [SerializeField] private int maxBuildingLevel = 5;

    [Header(" -- Aux Parameters -- ")] 
    [SerializeField] private float turretBulletsFiringRate = 4f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float spreadAngle = 45f;
    [SerializeField] private int initialShieldEnergy = 100;
    [SerializeField] private int shieldIncrementPerLevel = 30;
    
    [Header(" -- Hidden stats -- ")]
    private bool uiCanvasIsActive = false;
    private int currentHP;
    private FactoryState _factoryState;
    private BuildingType _buildingType;
    private int upgradeCost;
    private int buildingLevel = 0;
    private int goldOutputPerRound = 0;
    private float nextShootTime = 0f;
    private GameObject currentShield;
    
    // Start is called before the first frame update
    void Start()
    {
        uiCanvas.SetActive(uiCanvasIsActive);
        currentHP = initialHP;
        _buildingType = (BuildingType)buildingType;
        upgradeCost = initialUpgradeCost;
        if (_buildingType == BuildingType.Shield)
        {
            currentShield = Instantiate(shieldPrefab, Vector3.zero, quaternion.identity);
        }
        // Debug.Log(buildingType);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        switch (_factoryState)
        {
            case FactoryState.Destoryed:
                // TODO: play destroy animation
                break;
            case FactoryState.Normal:
                // Normal functionality when building is not destroyed
                if (GameManager.instance.gameState == GameState.meteorfalling)
                    NormalFunctionality();
                
                // if current HP <= 0, change to destroy state
                if (currentHP <= 0)
                {
                    _factoryState = FactoryState.Destoryed;
                    // StartCoroutine(PlayOnDestroyAnimation());
                    
                }
                break;
        }
    }

    private void NormalFunctionality()
    {
        switch (_buildingType)
        {
            case BuildingType.Turret:
                // Shoot bullets intercept meteor
                Debug.Log("Time is:" + Time.time + "nextShootTime is:" + nextShootTime);
                if (Time.time >= nextShootTime)
                {
                    ShootBullet();
                    nextShootTime = Time.time + 1f / turretBulletsFiringRate / (1f + buildingLevel);
                }
                break;
            case BuildingType.Shield:
                // check if shield is broken and close it
                // Or not
                break;
        }
    }
    
    private void ShootBullet()
    {
        // Calculate a random angle within the spread
        float angle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
        
        // Calculate the bullet's direction based on the random angle
        Vector2 shootDirection = Quaternion.Euler(0, 0, angle) * transform.up;
        Quaternion bulletRotation = Quaternion.LookRotation(Vector3.forward, shootDirection);
        
        GameObject bullet = Instantiate(bulletPrefab, transform.position + Vector3.up * 1.5f, bulletRotation);
        Debug.LogWarning("New Bullet generated");
        // Set the bullet's velocity
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = shootDirection * bulletSpeed;
    }
    
    private IEnumerator PlayOnDestroyAnimation()
    {
        yield return null;
    }
    private void UpdateUI()
    {
        // Update HP bar
        HPBarPercentage.fillAmount = (float)currentHP / initialHP;
        
        // Update Upgrade text
        upgradeCostText.text = $"{upgradeCost}";
        currentLevelText.text = $"{buildingLevel}";
        
    }
    
    private void OnMouseDown()
    {
        uiCanvasIsActive = !uiCanvasIsActive;
        AudioManager.audioInstance.PlayAudio(11);
        uiCanvas.SetActive(uiCanvasIsActive);
    }

    public void DebugAddHP()
    {
        currentHP++;
    }

    public void takeDamage(int tempDamage)
    {
        currentHP -= tempDamage;
    }

    public void Settlement()
    {
        // Several things to do
        // 1. repair it
        // 2. if destroyed, make it no output for this stage 
        // 3. if still there, make it do whatever it should done.
        
        currentHP = initialHP;
        bool isDestroyed = _factoryState == FactoryState.Destoryed;
        _factoryState = FactoryState.Normal;

        if (!isDestroyed)
        {
            // TODO: productive things
            switch (_buildingType)
            {
                case BuildingType.MainTown:
                    // Make money after round is over
                    goldOutputIncrement = goldOutputIncrement * buildingLevel + initialGoldOutput;
                    GameManager.instance.currentGold += goldOutputIncrement;
                    break;
                case BuildingType.ResearchLab:
                    // Nothing to do
                    break;
                case BuildingType.Farm:
                    // Make Money after round is over
                    goldOutputIncrement = goldOutputIncrement * buildingLevel + initialGoldOutput;
                    GameManager.instance.currentGold += goldOutputIncrement;
                    break;
                case BuildingType.Shield:
                    // Instantiate the shield and change the HP of it
                    if (currentShield != null)
                    {
                        Destroy(currentShield);
                    }
                    currentShield = Instantiate(shieldPrefab, Vector3.zero, quaternion.identity);
                    currentShield.GetComponent<BlockController>()
                        .SetHP(initialShieldEnergy + shieldIncrementPerLevel * buildingLevel);
                    break;
            }
        }
        else
        {
            // TODO: no output, but do whatever need to be done.
            switch (_buildingType)
            {
                case BuildingType.MainTown:
                    GameManager.instance.GameOver();
                    break;
            }
        }
    }

    public void UpgradeBuilding()
    {
        // Check if gold is enough
        if (GameManager.instance.currentGold <= upgradeCost || buildingLevel >= maxBuildingLevel)
        {
            AudioManager.audioInstance.PlayAudio(9);
            // TODO: tell player u need more money
            Debug.LogError("Insufficient money or max building level reached");
        }
        else
        {

            GameManager.instance.currentGold -= upgradeCost;
            AudioManager.audioInstance.PlayAudio(13);
            buildingLevel++;
            upgradeCost += upgradeCostIncrement;
            goldOutputPerRound = initialGoldOutput + goldOutputIncrement * buildingLevel;
            initialHP += hpIncrementPerLevel;
            switch (_buildingType)
            {
                case BuildingType.ResearchLab:
                    // Unlock building
                    GameManager.instance.UnlockBuildings(buildingLevel - 1);
                    break;
            }
        }
        
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("it collided");
        if (other.gameObject.CompareTag("BuildingBlock"))
        {
            other.gameObject.GetComponent<BlockController>().takeDamage(9999);
            takeDamage(1);
        }
    }
    
    
}
