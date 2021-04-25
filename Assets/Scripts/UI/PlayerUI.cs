using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image bloodScreen;
    [SerializeField] private Text leftBulletCount;
    [SerializeField] private Text rightBulletCount;
    [SerializeField] private Image playerHp;
    [SerializeField] private Image playerHpBox;
    [SerializeField] private Image playerDieImage;
    [SerializeField] private Text playerDieText;

    private bool uiCheck = true;
    public bool UiCheck
    { 
        get
        {
            return uiCheck;
        }
        set
        {   //this.gameObject.SetActive(value); 하면 안됌
            bloodScreen.gameObject.SetActive(value);
            leftBulletCount.gameObject.SetActive(value);
            rightBulletCount.gameObject.SetActive(value);
            playerHp.gameObject.SetActive(value);
            playerHpBox.gameObject.SetActive(value);
        } 
    }
    public void UIReflectionHp(float currHp, float initHp)
    {
        playerHp.fillAmount = (currHp / initHp);
        StartCoroutine(ShowBloodScreen());
    }

    public void UIReflectionlLeftBullet(int currBullet, int MaxBullet)
    {
        leftBulletCount.text = currBullet.ToString() + " / " + MaxBullet.ToString();
    }

    public void UIReflectionlRightBullet(int currBullet, int MaxBullet)
    {
        rightBulletCount.text = currBullet.ToString() + " / " + MaxBullet.ToString();
    }

    IEnumerator ShowBloodScreen()
    {
        //BloodScreen 텍스처의 알파값을 불규칙하게 변경
        bloodScreen.color = new Color(1, 0, 0, Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        //BloodScreen 텍스처의 색상을 모두 0으로 변경
        bloodScreen.color = Color.clear;
    }

    public void PlayerDieUI()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine(float _speed = 0.02f)
    {
        Color imageColor = playerDieImage.color;
        Color txtColor = playerDieText.color;
        while (imageColor.a < 1f)
        {
            imageColor.a += _speed;
            txtColor.a += _speed;
            playerDieImage.color = imageColor;
            playerDieText.color = txtColor;
            yield return new WaitForSeconds(0.01f);
        }
    }


}
