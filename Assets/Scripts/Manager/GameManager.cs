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

    [SerializeField] private List<Mob> mobs = new List<Mob>();
    [SerializeField] private Transform spawnPoint;

    private Transform[] spawnPoints = null;

    public string currenBgm;
    public bool isGameOver;
    public float spawnTime;
    public int currrentMobCount;
    public int mobMaxCount;

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


    public void ChooseLevel(int level)
    {
        StartCoroutine(StartGame(level));
    }

    IEnumerator StartGame(int level)
    {
        for (int i = 0; i < 10; i++)
        {
            SoundManager.instance.PlaySE(Constant.countDown + (10 - i).ToString());
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(CreateEnemy(level));
    }

    public IEnumerator CreateEnemy(int level)
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
                EnhancedMob(mob, level);
            }
        }
    }

    public void EnhancedMob(Mob mob, int level)
    {
        mob.speed += level;
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