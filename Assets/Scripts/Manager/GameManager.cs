using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private List<Mob> mobs = new List<Mob>();
    [SerializeField] private GameObject viewObject;
    [SerializeField] private int mobMaxCount = 0;
    [SerializeField] private float spawnTime = 0;

    private Transform[] spawnPoints = null;
    private Vector3 savePlayerPos;
    private Vector3 savePlayerRot;
    private int currrentMobCount = 0;

    public string currenBgm;
    public int currentSceneLevel = 0;
    public bool gameStarting = false;
    public bool isGameOver = false;

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
        spawnPoints = GameObject.FindWithTag(Constant.spawn).GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        viewObject.SetActive(true);
    }

    public void ChooseLevel(int level)
    {
        gameStarting = true;
        MyGameLevel = level;
        spawnTime -= level;
        if (Player.instance.rayCheck)
        {
            Player.instance.RayOn();
        }
        else
        {
            Player.instance.RayOff();
        }
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        for (int i = 0; i < 10; i++)
        {
            ObjectPoolManager.instance.MakeGun();
            yield return new WaitForSeconds(1f);
            SoundManager.instance.PlaySE(Constant.countDown + (10 - i).ToString());
            if (i == 5)
            {
                StartCoroutine(CreateEnemy());
            }
        }
    }

    public IEnumerator CreateEnemy()
    {
        while (!isGameOver)
        {
            if (currrentMobCount < mobMaxCount)
            {
                int idx = Random.Range(1, spawnPoints.Length);
                Mob mob = Instantiate(mobs[Random.Range(0, mobs.Count)], spawnPoints[idx].position, spawnPoints[idx].rotation);
                mob.transform.SetParent(spawnPoints[idx]);
                mob.transform.position = spawnPoints[idx].transform.position;
                mob.StartingMob();
                currrentMobCount++;
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void MoveStage(int moveStageLevel, Vector3 startPosition, Vector3 startRotation)
    {
        StopAllCoroutines();
        currentSceneLevel = moveStageLevel;
        savePlayerPos = startPosition;
        savePlayerRot = startRotation;
        SceneManager.LoadScene(Constant.loadingScene);
    }

    public void StartScene()
    {
        gameStarting = false;
        viewObject.SetActive(true);
        spawnPoints = GameObject.FindWithTag(Constant.spawn).GetComponentsInChildren<Transform>();

        Player.instance.RayOn();

        //디버깅용 주석
        //Player.instance.transform.position = savePlayerPos;
        //Player.instance.transform.rotation = Quaternion.Euler(savePlayerRot.x, savePlayerRot.y, savePlayerRot.z);
        //ObjectPoolManager.instance.transform.position = new Vector3(savePlayerPos.x + 1.5f, savePlayerPos.y, savePlayerPos.z + 1);
        //ObjectPoolManager.instance.transform.rotation = Quaternion.Euler(savePlayerRot.x, savePlayerRot.y, savePlayerRot.z);
    }
}