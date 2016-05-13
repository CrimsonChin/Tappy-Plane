using UnityEngine;
using System.Collections;

using System;
using UnityEngine.UI;

public class Plane : MonoBehaviour
{

    public float speed = 2;
    public float force = 300;
    private bool isGameStarted;
    private float direction;
    private float switchTime = 2;

    void Start()
    {
        direction = 1;
        GetComponent<Rigidbody2D>().isKinematic = true;
        isGameStarted = false;

        //InvokeRepeating("ReverseDirection", 0, 1);
    }

    void Update()
    {
        if (!isGameStarted)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * force);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Restart
        Application.LoadLevel(Application.loadedLevel);
    }

    void OnEnable()
    {
        EventManager.Subscribe("GameStarted", HandleGameStarted);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe("GameStarted", HandleGameStarted);
    }

    private void HandleGameStarted()
    {
        isGameStarted = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
    }
}
