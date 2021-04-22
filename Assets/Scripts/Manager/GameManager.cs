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

    private int gameLevel;
    public int currentSceneLevel = 1;
    public string currenBgm;
    public bool isGameOver;

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

    void Start()
    {
        ChooseLevel(2);
    }

    public void ChooseLevel(int level)
    {
        gameLevel = level;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        for (int i = 0; i < 10; i++)
        {
            ObjectPoolManager.instance.MakeGun();
            yield return new WaitForSeconds(1f);
            SoundManager.instance.PlaySE(Constant.countDown + (10 - i).ToString());
        }
        StartCoroutine(CreateEnemy());
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

    ///// <summary>
    ///// 후반 작업 예정
    ///// </summary>
    //public void ResetInfo()
    //{
    //    SoundManager.instance.bgmIsOn = true;
    //    SoundManager.instance.soundIsOn = true;
    //    SoundManager.instance.audioSourceBgm.volume = 1f;
    //    SoundManager.instance.audioSourceEffects[0].volume = 1f;
    //    PlayerPrefs.SetInt("FirstView", 1);
    //    PlayerClearStage = 1;
    //    player.currentMoney = 0;
    //    player.ReserPlayer();
    //    CallSave(true);
    //}

    public void MoveStage(int moveStageLevel, Vector3 startPosition, Vector3 startRotation)
    {
        currentSceneLevel = moveStageLevel;
        StopAllCoroutines();
        SceneManager.LoadScene(Constant.loadingScene);
        savePlayerPos = startPosition;
        savePlayerRot = startRotation;
    }
    public void StartScene()
    {
        Player.instance.transform.position = savePlayerPos;
        Player.instance.transform.rotation = Quaternion.Euler(savePlayerRot.x, savePlayerRot.y, savePlayerRot.z);
        Player.instance.moveImpossible = true;
        spawnPoint = GameObject.FindWithTag(Constant.spawn);
        spawnPoints = spawnPoint.GetComponentsInChildren<Transform>();
        ChooseLevel(1);
    }
}