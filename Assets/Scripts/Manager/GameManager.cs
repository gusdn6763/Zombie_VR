using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private List<Mob> mobs = new List<Mob>();      //랜덤으로 생성하는 몹들
    [SerializeField] private GameObject viewObject;                 //씬 시작시 셋팅 캔버스 활성화
    [SerializeField] private int mobMaxCount = 0;                   //최대로 출현한 몹들의 수
    [SerializeField] private float spawnTime = 0;                   //몹 스폰 일정 시간

    private Transform[] spawnPoints = null;                         //몹 스폰 포인트
    private Vector3 savePlayerPos;                                  //플레이어 시작위치
    private Vector3 savePlayerRot;                                  //플레이어 시작위치
    private int currrentMobCount = 0;                               //현재 몹 갯수

    public string currenBgm;                                        //현재 실행중인 음악
    public int currentSceneLevel = 0;                               //현재 스테이지 레벨
    public bool gameStarting = false;                               //난이도 선택시 true활성화
    public bool isGameOver = false;                                 //플레이어 죽을시 true활성화

    public int Difficulty { get; set; }

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

    /// <summary>
    /// 난이도 선택 버튼에서 함수 실행
    /// </summary>
    /// <param name="level">선택한 난이도 레벨</param>
    public void ChooseLevel(int level)
    {
        gameStarting = true;
        Difficulty = level;
        spawnTime -= level;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        ObjectPoolManager.instance.MakeDoubleGun();
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

    /// <summary>
    /// 포탈로 이동시 작동하는 함수
    /// </summary>
    /// <param name="moveStageLevel">이동할 스테이지</param>
    /// <param name="startPosition">시작위치</param>
    /// <param name="startRotation">시작위치</param>
    public void MoveStage(int moveStageLevel, Vector3 startPosition, Vector3 startRotation)
    {
        StopAllCoroutines();
        currentSceneLevel = moveStageLevel;
        savePlayerPos = startPosition;
        savePlayerRot = startRotation;
        SceneManager.LoadScene(Constant.loadingScene);
    }

    /// <summary>
    /// 씬으로 이동이 완료시 실행하는 함수
    /// </summary>
    public void StartScene()
    {
        gameStarting = false;
        viewObject.SetActive(true);
        spawnPoints = GameObject.FindWithTag(Constant.spawn).GetComponentsInChildren<Transform>();

        Player.instance.transform.position = savePlayerPos;
        Player.instance.transform.rotation = Quaternion.Euler(savePlayerRot.x, savePlayerRot.y, savePlayerRot.z);
        ObjectPoolManager.instance.transform.position = new Vector3(savePlayerPos.x + 1.5f, savePlayerPos.y, savePlayerPos.z + 1);
        ObjectPoolManager.instance.transform.rotation = Quaternion.Euler(savePlayerRot.x, -savePlayerRot.y, savePlayerRot.z);
    }
}