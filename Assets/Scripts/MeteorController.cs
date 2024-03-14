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
    public GameObject bigMeteor;
    public Vector2 center;
    public Vector2 size;
    public WeatherType weatherType;
    private bool hasChangedWeather = false;
    int numberOfSpawns = 300;  //数量
    public float levelTime = 5f; //持续时间
    [HideInInspector]public float spawnInterval;  //生成间隔
    public int numberOfVariantSpawn = 20; //圆形数量
    public int variantNum = 0;
    public int normalNum = 0;
    private void Awake()
    {
      
        Debug.Log(spawnInterval);
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
    private void Start()
    {
        spawnInterval = levelTime / numberOfSpawns;
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
    /// <summary>
    /// 正式生成方法
    /// </summary>
    /// <param name="spawnInterval"></param>
    /// <returns></returns>
    public IEnumerator SpawnMeteorAtRandom(float spawnInterval)
    {
        Debug.Log(spawnInterval);
        weatherType = (WeatherType)Random.Range(0, 3);
        for (int i = 0; i < numberOfSpawns; i++)
        {

            Vector2 pos = center + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));
            if (Random.Range(0, 100) < numberOfVariantSpawn * 100 / numberOfSpawns)
            {
                Instantiate(bigMeteor, pos, Quaternion.identity);
                variantNum++;
            }
            else
            {
                Instantiate(meteor, pos, Quaternion.identity);
                normalNum++;
            }
            
            yield return new WaitForSeconds(spawnInterval);
        }
        Debug.Log($"变体数量:{variantNum}");
        Debug.Log($"正常数量:{normalNum}");

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(center, size);
    }
    public void ClearNumCount()
    { 
        variantNum = 0;
        normalNum = 0;
    }

}
