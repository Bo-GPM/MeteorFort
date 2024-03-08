using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] private int blockHP = 10;
    
    
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
}
