using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("---Weather---")]
    [SerializeField] Text weatherText;
    [SerializeField] Text windForce;
    [SerializeField] Meteor meteorInstance;
    [Header("---Button---")]
    [SerializeField] GameObject playerTurn;
    [SerializeField] GameObject meteorTurn;
    private bool canClick = true;
    private bool coroutineStarted = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        switch (MeteorController.instance.weatherType)
        {
            case WeatherType.EastWind:
                weatherText.text = "East Wind!";
                windForce.text = "Wind Power：" + meteorInstance.forceMagnitudeRight;
                break;
            case WeatherType.WestWind:
                weatherText.text = "West Wind!";
                windForce.text = "Wind Power：" + meteorInstance.forceMagnitudeLeft;
                break;
            case WeatherType.StraightWind:
                weatherText.text = "Straight Wind!";
                windForce.text = "Wind Power：" + meteorInstance.forceMagnitudeDown;
                break;
        }
        switch (GameManager.instance.gameState)
        {
            case GameState.playerbuilding:
                playerTurn.SetActive(true);
                meteorTurn.SetActive(false);
                break;
            case GameState.meteorfalling:
                playerTurn.SetActive(false);
                meteorTurn.SetActive(true);
                if (!coroutineStarted)
                {
                    StartCoroutine(GameManager.instance.SwitchStateAfterDelay(GameState.playerbuilding, MeteorController.instance.levelTime/2));
                    coroutineStarted = true;
                }

                break;

        }
    }

    public void ClickStageButton()
    {
        if (canClick)
        {
            Debug.Log(GameManager.instance.gameState);
            canClick = false;
            switch (GameManager.instance.gameState)
            {
                case GameState.playerbuilding:
                    StartCoroutine(MeteorController.instance.SpawnMeteorAtRandom(MeteorController.instance.spawnInterval));
                    StartCoroutine(GameManager.instance.SwitchStateAfterDelay(GameState.meteorfalling,0f));
                    coroutineStarted = false;
                    break;
                case GameState.meteorfalling:
                    break;

            }
            StartCoroutine(WaitForClick());
        }

    }
    IEnumerator WaitForClick()
    {
        yield return new WaitForSeconds(MeteorController.instance.levelTime/2);
        // N秒后重新启用按钮点击
        canClick = true;
    }


}
