using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum HandState { NONE = 0, LEFT = 1, RIGHT = 2, BOTH = 3 }
public class CustomController : MonoBehaviour
{
    //디바이스 모델들
    [SerializeField] private List<GameObject> controllerModels;
    private Animator handAnimator;
    private GameObject controllerInstance; //디바이스 오브젝트
    private GameObject handInstance;       //hand 오브젝트
    private Weapon weapon;

    private bool menuButtonValue;
    private bool oneClicktriggerButtonCheck;

    public HandState currentHand;
    public InputDeviceCharacteristics characteristics;    //사람들이 임의로 정의해준 디바이스 열거 넘버
    public InputDevice currentUsingDevice;   //연결된 컨트롤러가 무엇인지 알려줌
    public GameObject handModel;          //hand모델
    [Header("체크시 컨트롤러")]public bool renderController = false; //hand인지 컨트롤러인지 확인하는 변수
    private void Start()
    {
        TryInitialize();
        CheckHandOrController();
        SetControllerPosition();
    }

    private void Update()
    {
        //사용가능한 디바이스가 없으면 다시 호출
        //예를 들어 사용도중 배터리가 방전시
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
        if (currentUsingDevice.TryGetFeatureValue(CommonUsages.triggerButton, out menuButtonValue) && menuButtonValue && weapon != null)
        {
            weapon.Attack();
        }

    }

    //바이브인지 오큘러스 리프트인지 확인하고 연결해주는 함수
    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        //컨트롤러를 입력받기 위해 사용하는 것
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        foreach (var device in devices)
        {
            //연결가능한 디바이스 속성이 무엇인지
            Debug.Log($"디바이스 이름: {device.name}, 연결된 디바이스: {device.characteristics}");
        }
        //접속 가능한 디바이스가 1개 이상일경우
        if (devices.Count > 0)
        {
            currentUsingDevice = devices[0];

            //Oculus Quest Controller를 구버전으로 인식해서 구버전이름을 인식시 신버전으로 이름을 바꾼다.
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

            //9개 모델을 찾아서 3D모델을 찾아주자
            if (currentControllerModel)
            {
                controllerInstance = Instantiate(currentControllerModel, transform);
            }
            //설정해둔게 없을경우 기본 모델로 만들어줌
            else
            {
                Debug.Log("알 수 없는 모델입니다.");
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
        // 현재 값에 액세스하려고 시도하며 다음을 반환
        //특정 기능 값을 검색해서 가져오면 true를 반환합니다.
        //현재 기기가 특정 기능을 지원하지 않거나, 기기가 유효하지 않은 경우(예: 컨트롤러 비활성) false를 반환합니다.
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

    public void GetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public void DropWeapon()
    {
        weapon = null;
    }
}
