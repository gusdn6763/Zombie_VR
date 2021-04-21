using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] private List<Mob> mobs = new List<Mob>();
    [SerializeField] private Transform spawnPoint;

    private Transform[] spawnPoints = null;
    public float spawnTime;
    public int currrentMobCount;
    public int mobMaxCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        spawnPoints = spawnPoint.GetComponentsInChildren<Transform>();
    }




}
