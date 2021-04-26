using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MovingObject
{
    public static Player instance;

    [SerializeField] private List<XRController> controllers = null;
    [SerializeField] private GameObject[] rays;

    private CharacterController characterController = null;     //VR Rig의 캐릭터 컨트롤러
    private GameObject head = null;                             //카메라 머리 위치

    public PlayerUI playerUi;                            

    public float mass = 1f;
    public bool moveImpossible = false;
    public bool rayCheck = true;

    public void Awake()
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
        characterController = GetComponent<CharacterController>();
        head = GetComponent<XRRig>().cameraGameObject;
    }

    public void Start()
    {
        currentHp = hp;
    }

    private void Update()
    {
        if (GameManager.instance.gameStarting)
        {
            CheckForInput();
            ApplyGravity();
        }
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

    void Jump()
    {
        Vector3 direction = new Vector3(0, 5f, 0);
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
        base.Damaged(damage);
        if (playerUi.isActiveAndEnabled)
        {
            playerUi.UIReflectionHp(currentHp, hp);
        }
    }

    public override void Die()
    {
        if (!GameManager.instance.isGameOver)
        {
            base.Die();
            playerUi.PlayerDieUI();
            GameManager.instance.isGameOver = true;
            SoundManager.instance.PlaySE(Constant.playerDieSound);
        }
    }

    public void RayOn()
    {
        for (int i = 0; i < rays.Length; i++)
        {
            rays[i].SetActive(true);
        }
    }
    public void RayOff()
    {
        for (int i = 0; i < rays.Length; i++)
        {
            rays[i].SetActive(false);
        }
    }
    public void PlayerMove(Vector3 direction, float speed)
    {
        Vector3 movement = direction * speed;
        gameObject.transform.Translate(movement * Time.deltaTime);
    }
}

