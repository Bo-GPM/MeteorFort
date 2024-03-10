using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] private int blockHP = 10;
    [SerializeField] private int cost = 30;

    void Update()
    {
        CheckTerminate();
    }

    public void takeDamage(int damageNumber)
    {
        blockHP -= damageNumber;
    }
    
    private void CheckTerminate()
    {
        if (blockHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int getCost()
    {
        return cost;
    }
}
