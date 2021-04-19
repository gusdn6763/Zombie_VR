using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MovingObject
{
    public static Player instance;

    [SerializeField] private List<XRController> controllers = null;
    [SerializeField] private Shake Dmgshake;

    private CharacterController characterController = null;     //VR Rig의 캐릭터 컨트롤러
    private GameObject head = null;                             //카메라 머리 위치

    public HandState usingGrab;                                 //현재 사용하고있는 손

    public float mass;

    public override void Awake()
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
        base.Awake();
        Dmgshake = GetComponent<Shake>();
        characterController = GetComponent<CharacterController>();
        head = GetComponent<XRRig>().cameraGameObject;
    }

    public void Start()
    {
        currentHp = HP;
        PositionController();
    }

    private void Update()
    {
        PositionController(); 
        CheckForInput();
        ApplyGravity();
    }

    /// <summary>
    /// 현재 위치 설정
    /// </summary>
    void PositionController()
    {
        float headHeight = Mathf.Clamp(head.transform.localPosition.y, 1, 2);
        characterController.height = headHeight;

        Vector3 newCenter = Vector3.zero;
        newCenter.x = head.transform.localPosition.x;
        newCenter.z = head.transform.localPosition.z;

        characterController.center = newCenter;
    }

    /// <summary>
    /// 설정된 컨트롤러중에 입력이 있을시 이동처리
    /// </summary>
    void CheckForInput()
    {
        foreach(XRController controller in controllers)
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
            {
                StartMove(position);
            }
        }
    }

    //입력받은 조이스틱값으로 이동
    void StartMove(Vector2 position)
    {
        Vector3 direction = new Vector3(position.x, 0, position.y);
        Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        direction = Quaternion.Euler(headRotation) * direction;

        Vector3 movement = direction * speed;
        characterController.Move(movement * Time.deltaTime);
    }

    //중력 적용
    void ApplyGravity()
    {
        Vector3 gravity = new Vector3(0, Physics.gravity.y * mass, 0);
        gravity.y *= Time.deltaTime;
        characterController.Move(gravity * Time.deltaTime);
    }

    /// <summary>
    /// 데미지 공격받을시
    /// </summary>
    /// <param name="damage"></param>
    public override void Damaged(int damage)
    {
        StartCoroutine(Dmgshake.ShakeCamera());
        base.Damaged(damage);
    }

    public override void Die()
    {
        base.Die();
    }

    public void Dead()
    {
        StopAllCoroutines();
        dmgCheck = false;
        gameObject.SetActive(false);
    }
}

