using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform rifleStart;
    [SerializeField] private Text HpText;

    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject Victory;

    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;
    private Camera playerCamera;

    public float health = 0;

    void Start()
    {
        ChangeHealth(0);
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
    }

    public void ChangeHealth(int hp)
    {
        health += hp;
        if (health > 100)
        {
            health = 100;
        }
        else if (health <= 0)
        {
            Lost();
        }
        HpText.text = health.ToString();
    }

    public void Win()
    {
        Victory.SetActive(true);
        DisablePlayerLook();
    }

    public void Lost()
    {
        GameOver.SetActive(true);
        DisablePlayerLook();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject buf = Instantiate(bullet);
            buf.transform.position = rifleStart.position;
            buf.GetComponent<Bullet>().setDirection(playerCamera.transform.forward);
            buf.transform.rotation = playerCamera.transform.rotation;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Collider[] tar = Physics.OverlapSphere(transform.position, 2);
            foreach (var item in tar)
            {
                if (item.CompareTag("Enemy"))
                {
                    Destroy(item.gameObject);
                }
            }
        }

        Collider[] targets = Physics.OverlapSphere(transform.position, 3);
        foreach (var item in targets)
        {
            if (item.CompareTag("Heal"))
            {
                ChangeHealth(50);
                Destroy(item.gameObject);
            }
            if (item.CompareTag("Finish"))
            {
                Win();
            }
            if (item.CompareTag("Enemy"))
            {
                Lost();
            }
        }
    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 movement = (cameraRight * moveX + cameraForward * moveZ) * moveSpeed;
        rb.velocity = movement;
    }

    private void DisablePlayerLook()
    {
        PlayerLook playerLook = GetComponent<PlayerLook>();
        if (playerLook != null)
        {
            playerLook.enabled = false;
        }
        Cursor.lockState = CursorLockMode.None;
    }
}
