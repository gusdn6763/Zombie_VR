using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    public static Player instance;

    [SerializeField] protected float hp;            //최대 HP
    [SerializeField] protected float currentHp;     //현재 HP
    [SerializeField] protected float speed;         //플레이어 속도
    [SerializeField] private XRNode playerMoveDevice;                //어떠한 기기로 이동할지 정하는 변수

    private CharacterController characterController;     //VR Rig의 캐릭터 컨트롤러
    private XRRig rig;                          
    public PlayerUI playerUi;

    private Vector2 inputAxis;
    public float mass = 1f;                                     //영향받는 중력크기
    public float additionalHeight = 0.2f;                       //추가적인 머리 크기
    public bool moveImpossible = false;                         //플레이어 이동을 금지
    public int damage;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        rig = GetComponent<XRRig>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        currentHp = hp;
    }

    private void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(playerMoveDevice);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();

        if (GameManager.instance.gameStarting)
        {
            if (!moveImpossible)
            {

            }
        }
        StartMove();
        ApplyGravity();
    }

    /// <summary>
    /// 변경된 높이값에 따른 PlayerController height값 변경
    /// </summary>
    void CapsuleFollowHeadset()
    {
        characterController.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        characterController.center = new Vector3(capsuleCenter.x, characterController.height / 2 + characterController.skinWidth, capsuleCenter.z);
    }


    //입력받은 조이스틱값으로 이동
    void StartMove()
    {
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        characterController.Move(direction * Time.fixedDeltaTime * speed);
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
    public void Damaged(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
        if (playerUi.isActiveAndEnabled)
        {
            playerUi.UIReflectionHp(currentHp, hp);
        }
    }

    public void Die()
    {
        if (!GameManager.instance.isGameOver)
        {
            speed = 0f;
            StopAllCoroutines();
            playerUi.PlayerDieUI();
            GameManager.instance.isGameOver = true;
            SoundManager.instance.PlaySE(Constant.playerDieSound);
        }
    }
}

