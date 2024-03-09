using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum WeatherType
{
    EastWind,
    WestWind,
    StraightWind
}


public class MeteorController : MonoBehaviour
{
    static public MeteorController instance;
    public GameObject meteor;
    public Vector2 center;
    public Vector2 size;
    public WeatherType weatherType;
    private bool hasChangedWeather = false;
    int numberOfSpawns = 30;
    private void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && !hasChangedWeather)
        {
            weatherType = (WeatherType)Random.Range(0, 3);
            Debug.Log(weatherType);
            for (int i = 0; i < numberOfSpawns; i++)
            {
                SpawnMeteorAtRandomTest();
            }
            hasChangedWeather = true;
        }
        else if (!Input.GetKey(KeyCode.Q))
        {
            hasChangedWeather = false;
        }

    }
    void SpawnMeteorAtRandomTest()
    {
        Vector2 pos = center + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));
        Instantiate(meteor, pos, Quaternion.identity);
    }

    public void SpawnMeteorAtRandom()
    {
        weatherType = (WeatherType)Random.Range(0, 3);
        for (int i = 0; i < numberOfSpawns; i++)
        {
            Vector2 pos = center + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));
            Instantiate(meteor, pos, Quaternion.identity);
        }
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(center, size);
    }

}
