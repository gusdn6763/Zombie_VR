using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BulletCtrl : MonoBehaviour 
{
    private Rigidbody rigi;
    private TrailRenderer trailRenderer;

    public Transform barrelLocation;
    public float shotPower;
    public int damage;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }
    private void OnEnable()
    {
        if (barrelLocation != null)
        {
            transform.position = barrelLocation.position;
            transform.rotation = barrelLocation.rotation;
            rigi.AddForce(barrelLocation.forward * shotPower);
            
            StartCoroutine(BulletDisable());
        }
    }

    private void OnDisable()
    {
        trailRenderer.Clear();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rigi.Sleep();
    }


    IEnumerator BulletDisable()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider Col)
    {
        if (Col.gameObject.CompareTag(Constant.monster))
        {
            Col.GetComponent<MovingObject>().Damaged(damage);
            gameObject.SetActive(false);
        }
        if (Col.gameObject.CompareTag(Constant.zombiePart))
        {
            Col.GetComponent<Part>().Damaged(damage);
            gameObject.SetActive(false);
        }

    }
}
