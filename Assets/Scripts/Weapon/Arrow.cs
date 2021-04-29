using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Transform pos;
    private Rigidbody rigi;

    public float shotPower;
    public int damage;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        rigi.velocity = Vector3.zero;
        rigi.angularVelocity = Vector3.zero;
        transform.position = pos.position;
        transform.rotation = pos.rotation;
    }

    public void ArrowShoot(int damage)
    {
        transform.SetParent(null);
        transform.LookAt(Player.instance.transform);
        rigi.AddForce(transform.forward * shotPower);
        StartCoroutine(BulletDisable());
    }

    IEnumerator BulletDisable()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.hitBox))
        {
            other.GetComponentInParent<Player>().Damaged(damage);
            transform.SetParent(pos.transform, true);
            gameObject.SetActive(false);
        }
    }
}