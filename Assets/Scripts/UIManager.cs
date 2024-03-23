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
    [Header("---Meteor Controller---")]
    [SerializeField] MeteorController frontController;
    [SerializeField] MeteorController middleController;
    [SerializeField] MeteorController backController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        switch (middleController.weatherType)
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
                    //这里要比较 严谨点的话
                    StartCoroutine(GameManager.instance.SwitchStateAfterDelay(GameState.playerbuilding, middleController.levelTime));
                    coroutineStarted = true;
                }

                break;

        }
    }

    public void ClickStageButton()
    {
        if (canClick)
        {
            AudioManager.audioInstance.PlayAudio(14);
            //(AudioManager.audioInstance.PlayAudioAfterDelay(0, 0.6f));
            Debug.Log(GameManager.instance.gameState);
            canClick = false;
            switch (GameManager.instance.gameState)
            {
                case GameState.playerbuilding:
                    StartCoroutine(frontController.SpawnMeteorAtRandom(frontController.spawnInterval));
                    StartCoroutine(middleController.SpawnMeteorAtRandom(middleController.spawnInterval));
                    StartCoroutine(backController.SpawnMeteorAtRandom(backController.spawnInterval));
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
        //这里要比较 严谨点的话
        yield return new WaitForSeconds(middleController.levelTime);
        // N秒后重新启用按钮点击
        canClick = true;
    }


}
