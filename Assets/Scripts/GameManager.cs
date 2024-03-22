using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    playerbuilding,
    meteorfalling
}
public class GameManager : MonoBehaviour
{

    [Header(" -- Building Related -- ")] 
    [SerializeField] private int initialGold = 100;
    [SerializeField] private float playerRotateSpeed = 10f;
    [SerializeField] private float forbitHeight = 8f;
    [SerializeField] private GameObject[] buildingBlockList;
    
    [Header(" -- Factory Related -- ")] 
    [SerializeField] private BuildingController[] factoryPrefabs;

    [Header(" -- UI Related -- ")] 
    [SerializeField] private Text GoldText;
    [SerializeField] private GameObject gameOverPanel;

    [Header(" -- Meteor Related -- ")]
    [SerializeField] MeteorController[] meteorControllers;
    [SerializeField] private float meteorsSettlingTime = 3f;

    private int activeBuidling;
    private bool isBuildingActive = false;
    private Vector3 mousePosition;
    private GameObject tempBlock;
    public int currentGold;
    private Camera mainCam;
    
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
        // DontDestroyOnLoad(gameObject);
        meteorControllers = FindObjectsOfType<MeteorController>();
        factoryPrefabs = FindObjectsOfType<BuildingController>();
        gameOverPanel = GameObject.Find("GameOverPanel");
        mainCam = Camera.main;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameState=GameState.playerbuilding;
        currentGold = initialGold;
        
        // Disable the game component
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.meteorfalling)
        { 
            
        }
        else if (gameState == GameState.playerbuilding)
        {
            // Update UI
            UpdateUI();
            
            // Building logic
            // 1. Update Mouse Pos
            // 2. Display selected blocks (if any)
            // 3. Player rotating the block?
            // 4. Place Block if player pressed left mouse button OR cancel building
            updateMousePos();
            updateTempBlockPosAndRot();
            if (isBuildingActive)
            {
                placeBlock();
            }
            
            
        }
    }
    public IEnumerator SwitchStateAfterDelay(GameState nextState,float delayTime)
    {
        // 等待几秒
        yield return new WaitForSeconds(delayTime);

        // 切换到指定的状态
        gameState = nextState;

        // Wait few seconds for meteor settle down
        StartCoroutine(FactorySettlement(meteorsSettlingTime));
        
        foreach (MeteorController meteorController in meteorControllers)
        {
            meteorController.ClearNumCount();
        }

            // 在控制台输出当前状态
            Debug.Log("Switched to " + nextState);
    }

    public IEnumerator FactorySettlement(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        // Add actions when game state is changing from rain back to building
        if (gameState == GameState.playerbuilding)
        {
            foreach (BuildingController buildingController in factoryPrefabs)
            {
                buildingController.Settlement();
            }
        }
    }
    
    private void UpdateUI()
    {
        GoldText.text = new string($"Gold: {currentGold}");
    }
    private void updateMousePos()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 0 - mainCam.transform.position.z;
        mousePosition = mainCam.ScreenToWorldPoint(mousePosition);
    }

    private void updateTempBlockPosAndRot()
    {
        if (tempBlock != null)
        {
            tempBlock.transform.position = mousePosition;
            if (Input.GetKey(KeyCode.Q))
            {
                tempBlock.transform.Rotate(0, 0, transform.rotation.z + playerRotateSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.E))
            {
                tempBlock.transform.Rotate(0, 0, transform.rotation.z - playerRotateSpeed * Time.deltaTime);
            }
        }
    }

    private void placeBlock()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mousePosition.y < forbitHeight)
            {
                // TODO: tell player this is too low to build
                Debug.LogWarning("Building distance is too low");
            }
            else
            {
                currentGold -= tempBlock.GetComponent<BlockController>().getCost();
                Vector3 blockVec = new Vector3(mousePosition.x, mousePosition.y, 0);
                Instantiate(buildingBlockList[activeBuidling], blockVec, tempBlock.transform.rotation);
                Destroy(tempBlock);
                isBuildingActive = false;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Destroy(tempBlock);
            isBuildingActive = false;
        }
    }

    public void SelectBlock(int blockIndex)
    {
        // Check gold first
        if (buildingBlockList[blockIndex].GetComponent<BlockController>().getCost() >= currentGold) 
        {
            // TODO: tell player they have insufficient funds
            Debug.LogWarning("Insufficient Funds!");
        }
        else
        {
            // instantiate, disable rigidbody & collider
            tempBlock = Instantiate(buildingBlockList[blockIndex], mousePosition, quaternion.identity);
            tempBlock.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            tempBlock.GetComponent<BoxCollider2D>().enabled = false;
            
            // Active building setup
            activeBuidling = blockIndex;
            isBuildingActive = true;
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
