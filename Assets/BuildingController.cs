using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    [Header(" -- Reference -- ")]
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private Text HPText;

    [Header(" -- Parameters -- ")] 
    [SerializeField] private int initialHP = 10;
    
    [Header(" -- Hidden stats -- ")]
    private bool uiCanvasIsActive = false;
    private int currentHP;
    
    
    // Start is called before the first frame update
    void Start()
    {
        uiCanvas.SetActive(uiCanvasIsActive);
        currentHP = initialHP;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
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
}
