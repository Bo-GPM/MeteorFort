using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    playerbuilding,
    meteorfalling
}
public class GameManager : MonoBehaviour
{
    
    static public GameManager instance;
    public GameState gameState;
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
    // Start is called before the first frame update
    void Start()
    {
        gameState=GameState.playerbuilding;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.meteorfalling)
        { 
            
        }
    }
    public IEnumerator SwitchStateAfterDelay(GameState nextState,float delayTime)
    {
        // 等待几秒
        yield return new WaitForSeconds(delayTime);

        // 切换到指定的状态
        gameState = nextState;

        // 在控制台输出当前状态
        Debug.Log("Switched to " + nextState);
    }


}
