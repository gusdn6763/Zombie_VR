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
    public float spawnTime;
    public bool isGameOver;
    public int currenLevel;
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


    void Start()
    {
        if (spawnPoints.Length > 0)
        {
            StartCoroutine(this.CreateEnemy());
        }
    }

    IEnumerator CreateEnemy()
    {
        while (!isGameOver)
        {
            if (currrentMobCount < mobMaxCount)
            {
                yield return new WaitForSeconds(spawnTime);
                int idx = Random.Range(0, spawnPoints.Length);
                Mob mob = Instantiate(mobs[0], spawnPoints[idx].position, spawnPoints[idx].rotation);
                mob.transform.SetParent(spawnPoints[idx]);
                mob.transform.position = spawnPoints[idx].transform.position;
            }
            else
            {
                yield return null;
            }
        }
    }


    public void MenuScene()
    {
        Player.instance.gameObject.SetActive(false);
        currenBgm = Constant.mainMenuBgm;
        SoundManager.instance.PlayBgm(currenBgm);
        //CallLoad(true);
    }


    /// <summary>
    /// ���� ���۽� ����
    /// </summary>
    //public void StartScene()
    //{
    //    Vector3 startPos = new Vector3(0f, -4f, 0f);

    //    player.gameObject.SetActive(true);
    //    player.transform.position = startPos;

    //    currenBgm = Constant.startBgm;
    //    SoundManager.instance.PlayBgm(currenBgm);

    //    //ù �����̸� ����â�� �������� ���� ����
    //    if (PlayerPrefs.GetInt("FirstView", 1) == 1)
    //    {
    //        Time.timeScale = 0f;
    //        //Instantiate(firstPopupView);
    //        PlayerPrefs.SetInt("FirstView", 0);
    //    }
    //}

    /// <summary>
    /// ���� ���� �������� ����
    /// </summary>
    //public void ClearStage()
    //{
    //    if (currentLevel == PlayerClearStage)
    //    {
    //        PlayerClearStage++;
    //    }
    //    player.gameObject.SetActive(false);

    //    VictoryView victoryViewPanel = Instantiate(victoryView).GetComponent<VictoryView>();
    //    victoryViewPanel.GetInfo(player.currentMoney, 3, PlayerClearStage - 1);
    //    SoundManager.instance.PlaySE(Constant.win);
    //    player.currentMoney += currentLevel * 10;
    //    CallSave(true);
    //}


    /// <summary>
    /// �Ĺ� �۾� ����
    /// </summary>
    //public void FailedStage()
    //{
    //    Instantiate(failedView);
    //    Time.timeScale = 0f;
    //    player.ReserPlayer();
    //    CallLoad(true);
    //}

    /// <summary>
    /// �Ĺ� �۾� ����
    /// </summary>
    //public void CallSave(bool stageClear)
    //{
    //    data.bgmOn = SoundManager.instance.bgmIsOn;
    //    data.soundOn = SoundManager.instance.soundIsOn;
    //    data.bgmVolume = SoundManager.instance.audioSourceBgm.volume;
    //    data.soundVolume = SoundManager.instance.audioSourceEffects[0].volume;

    //    if (stageClear)
    //    {
    //        data.playerClearStage = PlayerClearStage;
    //        data.playerMoney = player.currentMoney;
    //    }

    //    Debug.Log("���� ������ ����");

    //    BinaryFormatter bf = new BinaryFormatter();                             //���� ��ȯ
    //    FileStream file = File.Create(Application.persistentDataPath + "/SaveFile.dat");  //���� �����

    //    bf.Serialize(file, data);
    //    file.Close();

    //    Debug.Log(Application.persistentDataPath + "�� ��ġ�� �����߽��ϴ�.");
    //}
    ///// <summary>
    ///// �Ĺ� �۾� ����
    ///// </summary>
    ///// <param name="stageClear"></param>
    //public void CallLoad(bool stageClear)
    //{
    //    BinaryFormatter bf = new BinaryFormatter();

    //    if (!(File.Exists(Application.persistentDataPath + "/SaveFile.dat")))
    //    {
    //        Debug.Log("����");
    //        return;
    //    }

    //    FileStream file = File.Open(Application.persistentDataPath + "/SaveFile.dat", FileMode.Open);

    //    if (file != null && file.Length > 0)
    //    {
    //        data = (Data)bf.Deserialize(file);

    //        SoundManager.instance.bgmIsOn = data.bgmOn;
    //        SoundManager.instance.soundIsOn = data.soundOn;
    //        SoundManager.instance.audioSourceBgm.volume = data.bgmVolume;
    //        SoundManager.instance.audioSourceEffects[0].volume = data.soundVolume;
    //        if (stageClear)
    //        {
    //            PlayerClearStage = data.playerClearStage;
    //            player.currentMoney = data.playerMoney;
    //        }
    //        Debug.Log("�ҷ����⼺��");
    //    }
    //    else
    //    {
    //        Debug.Log("����� ���̺� ������ �����ϴ�");
    //    }
    //    file.Close();
    //}

    ///// <summary>
    ///// �Ĺ� �۾� ����
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