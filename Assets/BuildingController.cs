using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Text HPText;

    [Header(" -- Parameters -- ")] 
    [SerializeField] private int initialHP = 10;
    [SerializeField] private int buildingType;
    
    [Header(" -- Hidden stats -- ")]
    private bool uiCanvasIsActive = false;
    private int currentHP;
    private FactoryState _factoryState;
    private BuildingType _buildingType;
    
    // Start is called before the first frame update
    void Start()
    {
        uiCanvas.SetActive(uiCanvasIsActive);
        currentHP = initialHP;
        _buildingType = (BuildingType)buildingType;
        Debug.Log(buildingType);
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
                // TODO: normal functionality
                
                // if current HP <= 0, change to destroy state
                if (currentHP <= 0)
                {
                    _factoryState = FactoryState.Destoryed;
                    // StartCoroutine(PlayOnDestroyAnimation());

                }
                break;
        }
    }

    private IEnumerator PlayOnDestroyAnimation()
    {
        yield return null;
    }
    private void UpdateUI()
    {
        // Update HP Text
        HPText.text = $"HP: {currentHP}";
        
    }

    private void OnMouseDown()
    {
        uiCanvasIsActive = !uiCanvasIsActive;
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

        if (!isDestroyed)
        {
            // TODO: productive things
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
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("it collided");
        if (other.gameObject.CompareTag("BuildingBlock"))
        {
            other.gameObject.GetComponent<BlockController>().takeDamage(9999);
            takeDamage(1);
        }
    }
    
    
}
