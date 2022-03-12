using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Main : MonoBehaviour {

    static public Main S; // A singleton for Main
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies; // Array of Enemy prefabs
    public GameObject[] enemyPrefabs;
    private static GameObject enemyPrefab; // added by me
    public float enemySpawnPerSecond = 0.5f; // # Enemies/second
    public float enemyDefaultPadding = 1.5f; // Padding for position
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield
    };

    public static int waveDifficulty = 1; // added by me
    // public float delayBeforeWave = 5f; // added by me
    public static int delayAfterWave = 5; // added by me
    public static int waveNum = 0;
    public TextMeshProUGUI waveNumText;
    public TextMeshProUGUI playerScoreText;
    public  TextMeshProUGUI gameOverText;
    public GameObject gameOverTextObj; // added by me

    private BoundsCheck bndCheck;

    void Start()
    {
        WaveCounter();
        gameOverTextObj.SetActive(false);
    }

    public void ShipDestroyed( Enemy e)
    {
        // Potentially generate a PowerUp
        if (Random.value <= e.powerUpDropChance)
        {
            // Choose which PowerUp to pick
            // Pick one from the possibilities in powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            // Spawn a PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            // Set it to the proper WeaponType
            pu.SetType(puType);

            // Set it to the position of the destroyed ship
            pu.transform.position = e.transform.position;
        }
    }

    private void Awake()
    {
        S = this;
        // Set bndCheck to reference the BoundsCheck component on this GameObject
        bndCheck = GetComponent<BoundsCheck>();

        // Invoke SpawnEnemy() once (in 2 seconds, based on default values)
        // Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        // A generic Dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy()
    {
        // Pick a random Enemy prefab to instantiate
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        // Position the ENemy above the screen with a random x position
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        // Set the initial position for the spawned Enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
    }

    public void DelayedRestart(float delay)
    {
        // Invoke the Restart() method in delay seconds
        gameOverTextObj.SetActive(true);
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        // Reload _Scene_0 to restart the game
        SceneManager.LoadScene("_Scene_0");
        waveNum = 0;
        waveDifficulty = 1;
        Enemy.playerScore = 0;
    }
    ///<summary>
    ///Static function that gets a WeaponDefinition from the WEAP_DICT static
    ///protected field of the main class.
    /// </summary>
    /// <returns>The WeaponDefinition or, if there is no WeaponDefinition with
    /// the WeaponType passed in, returns a new WeaponDefinition with a
    /// WeaponType of none..</returns>
    /// <param name="wt">The WeaponType of the desired WeaponDefinition</param>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        // Check to make sure that the key exists in the Dictionary
        // Attempting to retrieve a key that didn't exist would throw an error,
        // so the following if statement is important.
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        // This returns a new WeaponDefinition with a type of WeaponType.none,
        // which means it has failed to find the right WeaponDefinition
        return new WeaponDefinition();
    }

    public void WaveController()
    {
        enemyPrefabs = GameObject.FindGameObjectsWithTag("Enemy");
        enemyPrefab = GameObject.FindGameObjectWithTag("Enemy");
        
        if (enemyPrefabs.Length == 0 || enemyPrefab == null || !enemyPrefab) {
            StartCoroutine(NextWave());
            return;
        }
    }

    IEnumerator NextWave()
    {
        for (int i = 0; i < waveDifficulty; i++) {
            SpawnEnemy();
        }

        waveNum++;
        waveDifficulty++;

        yield return new WaitForSeconds(delayAfterWave);
    }

    public void WaveCounter()
    {
        waveNumText.text = "Wave: " + waveNum.ToString();
    }

    public void ScoreCounter()
    {
        playerScoreText.text = "Score: " + Enemy.playerScore.ToString();
    }

    void Update()
    {
        WaveController();
        WaveCounter();
        ScoreCounter();
    }
}
