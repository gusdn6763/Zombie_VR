using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    public static Player instance;

    [SerializeField] protected float hp;            //�ִ� HP
    [SerializeField] protected float currentHp;     //���� HP
    [SerializeField] protected float speed;         //�÷��̾� �ӵ�
    [SerializeField] private XRNode playerMoveDevice;                //��� ���� �̵����� ���ϴ� ����

    private CharacterController characterController;     //VR Rig�� ĳ���� ��Ʈ�ѷ�
    private XRRig rig;                          
    public PlayerUI playerUi;

    private Vector2 inputAxis;
    public float mass = 1f;                                     //����޴� �߷�ũ��
    public float additionalHeight = 0.2f;                       //�߰����� �Ӹ� ũ��
    public bool moveImpossible = false;                         //�÷��̾� �̵��� ����
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
    /// ����� ���̰��� ���� PlayerController height�� ����
    /// </summary>
    void CapsuleFollowHeadset()
    {
        characterController.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        characterController.center = new Vector3(capsuleCenter.x, characterController.height / 2 + characterController.skinWidth, capsuleCenter.z);
    }


    //�Է¹��� ���̽�ƽ������ �̵�
    void StartMove()
    {
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        characterController.Move(direction * Time.fixedDeltaTime * speed);
    }

    //�߷� ����
    void ApplyGravity()
    {
        Vector3 gravity = new Vector3(0, Physics.gravity.y * mass, 0);
        gravity.y *= Time.deltaTime;
        characterController.Move(gravity * Time.deltaTime);
    }

    /// <summary>
    /// ������ ���ݹ�����
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

