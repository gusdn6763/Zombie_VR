using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum HandState { NONE = 0, LEFT = 1, RIGHT = 2, BOTH = 3 }
public class CustomController : MonoBehaviour
{
    //����̽� �𵨵�
    [SerializeField] private List<GameObject> controllerModels;
    [SerializeField] private PlayerRayScript handRay;
    private Animator handAnimator;
    private GameObject controllerInstance; //����̽� ������Ʈ
    private GameObject handInstance;       //hand ������Ʈ
    private Gun currentWeapon;

    private bool menuButtonValue = false;
    private bool oneClicktriggerButtonCheck = false;

    public HandState currentHand;
    public InputDeviceCharacteristics characteristics;    //������� ���Ƿ� �������� ����̽� ���� �ѹ�
    public InputDevice currentUsingDevice;   //����� ��Ʈ�ѷ��� �������� �˷���
    public GameObject handModel;          //hand��
    [Header("üũ�� ��Ʈ�ѷ�")]public bool renderController = false; //hand���� ��Ʈ�ѷ����� Ȯ���ϴ� ����
    private void Start()
    {
        TryInitialize();
        CheckHandOrController();
        SetControllerPosition();
        handRay.GradientCheck(false);
    }

    private void Update()
    {
        //��밡���� ����̽��� ������ �ٽ� ȣ��
        //���� ��� ��뵵�� ���͸��� ������
        if (!currentUsingDevice.isValid)
        {
            TryInitialize();
            CheckHandOrController();
            SetControllerPosition();
            return;
        }
        if (!renderController)
        {
            UpdateHandAnimation();
        }
        if (currentUsingDevice.TryGetFeatureValue(CommonUsages.triggerButton, out menuButtonValue) && menuButtonValue && currentWeapon != null)
        {
            if (currentWeapon.GetType() == typeof(DoubleGun))
            {
                currentWeapon.Attack();
            }
            else if (oneClicktriggerButtonCheck && currentWeapon.GetType() == typeof(Gun))
            {
                currentWeapon.Attack();
                oneClicktriggerButtonCheck = false;
            }
        }
        else
        {
            oneClicktriggerButtonCheck = true;
        }
    }

    //���̺����� ��ŧ���� ����Ʈ���� Ȯ���ϰ� �������ִ� �Լ�
    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        //��Ʈ�ѷ��� �Է¹ޱ� ���� ����ϴ� ��
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        foreach (var device in devices)
        {
            //���ᰡ���� ����̽� �Ӽ��� ��������
            Debug.Log($"����̽� �̸�: {device.name}, ����� ����̽�: {device.characteristics}");
        }
        //���� ������ ����̽��� 1�� �̻��ϰ��
        if (devices.Count > 0)
        {
            currentUsingDevice = devices[0];

            //Oculus Quest Controller�� ���������� �ν��ؼ� �������̸��� �νĽ� �Ź������� �̸��� �ٲ۴�.
            string name = "";
            if ("Oculus Touch Controller - Left" == currentUsingDevice.name)
            {
                name = "Oculus Quest Controller - Left";
            }
            else if ("Oculus Touch Controller - Right" == currentUsingDevice.name)
            {
                name = "Oculus Quest Controller - Right";
            }

            GameObject currentControllerModel = controllerModels.Find(controller => controller.name == name);

            //9�� ���� ã�Ƽ� 3D���� ã������
            if (currentControllerModel)
            {
                controllerInstance = Instantiate(currentControllerModel, transform);
            }
            //�����صа� ������� �⺻ �𵨷� �������
            else
            {
                Debug.Log("�� �� ���� ���Դϴ�.");
                controllerInstance = Instantiate(controllerModels[0], transform);
            }

            handInstance = Instantiate(handModel, transform);
            handAnimator = handInstance.GetComponent<Animator>();
        }
    }
    void CheckHandOrController()
    {
        if (renderController)
        {
            handInstance.SetActive(false);
            controllerInstance.SetActive(true);
        }
        else
        {
            handInstance.SetActive(true);
            controllerInstance.SetActive(false);
        }
    }
    void SetControllerPosition()
    {
        if (currentUsingDevice.name.Contains("Left"))
        {
            currentHand = HandState.LEFT;
        }
        else if (currentUsingDevice.name.Contains("Right"))
        {
            currentHand = HandState.RIGHT;
        }
        else
        {
            currentHand = HandState.NONE;
        }
    }
    void UpdateHandAnimation()
    {
        // ���� ���� �׼����Ϸ��� �õ��ϸ� ������ ��ȯ
        //Ư�� ��� ���� �˻��ؼ� �������� true�� ��ȯ�մϴ�.
        //���� ��Ⱑ Ư�� ����� �������� �ʰų�, ��Ⱑ ��ȿ���� ���� ���(��: ��Ʈ�ѷ� ��Ȱ��) false�� ��ȯ�մϴ�.
        if (currentUsingDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (currentUsingDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    public void GetWeapon(Gun currentGun)
    {
        if (currentGun != null)
        {
            handRay.GradientCheck(false);
            this.currentWeapon = currentGun;
            this.currentWeapon.grapingHand = currentHand;
            this.currentWeapon.UpdateBulletText();
        }
    }

    public void DropWeapon()
    {
        handRay.GradientCheck(true);
        currentWeapon = null;
    }
}
