using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int currrentMobCount;
    [SerializeField] private int mobMaxCount;
    [SerializeField] private float spawnTime;

    private Transform[] spawnPoints = null;

    private int gameLevel;
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
        spawnPoints = spawnPoint.GetComponentsInChildren<Transform>();
    }

    void Start()
    {
        ChooseLevel(1);
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



}