using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Data
{
    public bool bgmOn;
    public bool soundOn;

    public float bgmVolume;
    public float soundVolume;

    public int playerClearStage;
    public int playerMoney;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Gun gun;
    [SerializeField] private List<Mob> mobs = new List<Mob>();
    [SerializeField] private int currrentMobCount = 0;
    [SerializeField] private int mobMaxCount = 10;
    [SerializeField] private float spawnTime;

    [SerializeField] private GameObject spawnPoint;
    private Transform[] spawnPoints = null;
    private Vector3 savePlayerPos;
    private Vector3 savePlayerRot;
    public Light mylight;
    private int gameLevel;
    public int currentSceneLevel = 1;
    public string currenBgm;
    public bool gameStarting = false;
    public bool isGameOver;
    public SettingView settingView;

    public int MyGameLevel { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        spawnPoint = GameObject.FindWithTag(Constant.spawn);
        spawnPoints = spawnPoint.GetComponentsInChildren<Transform>();
    }

    public void ChooseLevel(int level)
    {
        Player.instance.RayOnOff();
        gameStarting = true;
        gameLevel = level;
        spawnTime -= level;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        Player.instance.RayOnOff();
        for (int i = 0; i < 10; i++)
        {
            ObjectPoolManager.instance.MakeGun();
            yield return new WaitForSeconds(1f);
            SoundManager.instance.PlaySE(Constant.countDown + (10 - i).ToString());
            if (mylight != null)
            {
                mylight.intensity--;
            }
            if (i == 5)
            {
                StartCoroutine(CreateEnemy());
            }
        }
    }

    public IEnumerator CreateEnemy()
    {
        while (true)
        {
            if (currrentMobCount < mobMaxCount)
            {
                yield return new WaitForSeconds(spawnTime);
                int idx = Random.Range(1, spawnPoints.Length);
                Mob mob = Instantiate(mobs[Random.Range(0, mobs.Count)], spawnPoints[idx].position, spawnPoints[idx].rotation);
                mob.transform.SetParent(spawnPoints[idx]);
                mob.transform.position = spawnPoints[idx].transform.position;
                mob.StartingMob();
            }
        }
    }

    public void MoveStage(int moveStageLevel, Vector3 startPosition, Vector3 startRotation)
    {
        Player.instance.moveImpossible = true;
        currentSceneLevel = moveStageLevel;
        StopAllCoroutines();
        savePlayerPos = startPosition;
        savePlayerRot = startRotation;
        SceneManager.LoadScene(Constant.loadingScene);
    }
    public void StartScene()
    {
        gameStarting = false;
        Player.instance.rayCheck = true;
        Player.instance.RayOnOff();
        spawnPoint = GameObject.FindWithTag(Constant.spawn);
        spawnPoints = spawnPoint.GetComponentsInChildren<Transform>();
        settingView.gameObject.SetActive(true);
        Player.instance.transform.position = savePlayerPos;
        Player.instance.transform.rotation = Quaternion.Euler(savePlayerRot.x, savePlayerRot.y, savePlayerRot.z);
        Player.instance.transform.position = savePlayerPos;
        Player.instance.transform.rotation = Quaternion.Euler(savePlayerRot.x, savePlayerRot.y, savePlayerRot.z);
    }
}