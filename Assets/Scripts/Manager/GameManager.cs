using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private List<Mob> mobs = new List<Mob>();      //�������� �����ϴ� ����
    [SerializeField] private GameObject viewObject;                 //�� ���۽� ���� ĵ���� Ȱ��ȭ
    [SerializeField] private int mobMaxCount = 0;                   //�ִ�� ������ ������ ��
    [SerializeField] private float spawnTime = 0;                   //�� ���� ���� �ð�

    private Transform[] spawnPoints = null;                         //�� ���� ����Ʈ
    private Vector3 savePlayerPos;                                  //�÷��̾� ������ġ
    private Vector3 savePlayerRot;                                  //�÷��̾� ������ġ
    private int currrentMobCount = 0;                               //���� �� ����

    public string currenBgm;                                        //���� �������� ����
    public int currentSceneLevel = 0;                               //���� �������� ����
    public bool gameStarting = false;                               //���̵� ���ý� trueȰ��ȭ
    public bool isGameOver = false;                                 //�÷��̾� ������ trueȰ��ȭ

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
    /// ���̵� ���� ��ư���� �Լ� ����
    /// </summary>
    /// <param name="level">������ ���̵� ����</param>
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
    /// ��Ż�� �̵��� �۵��ϴ� �Լ�
    /// </summary>
    /// <param name="moveStageLevel">�̵��� ��������</param>
    /// <param name="startPosition">������ġ</param>
    /// <param name="startRotation">������ġ</param>
    public void MoveStage(int moveStageLevel, Vector3 startPosition, Vector3 startRotation)
    {
        StopAllCoroutines();
        currentSceneLevel = moveStageLevel;
        savePlayerPos = startPosition;
        savePlayerRot = startRotation;
        SceneManager.LoadScene(Constant.loadingScene);
    }

    /// <summary>
    /// ������ �̵��� �Ϸ�� �����ϴ� �Լ�
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