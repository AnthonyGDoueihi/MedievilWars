using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera cam;
    float speed;
    MapGenerator mapGen;

    public bool bMoveRight;
    public bool bMoveLeft;
    public bool bMoveUp;
    public bool bMoveDown;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        mapGen = FindObjectOfType<MapGenerator>();
    }

    // Update is called once per frame
    void Update () {
        speed = cam.orthographicSize;

        GetMoveDirection();
        MoveCamera();

    }

    void GetMoveDirection()
    {

    }

    void MoveCamera()
    {

        if (bMoveRight || !Physics2D.Raycast(cam.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)), cam.transform.forward, 10.0f))
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);

        }

        if (bMoveLeft || !Physics2D.Raycast(cam.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)), cam.transform.forward, 10.0f))
        {
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);

        }

        if (bMoveUp || !Physics2D.Raycast(cam.ViewportToWorldPoint(new Vector3(0.5f, 0, 0)), cam.transform.forward, 10.0f))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);

        }

        if (bMoveDown || !Physics2D.Raycast(cam.ViewportToWorldPoint(new Vector3(0.5f, 1, 0)), cam.transform.forward, 10.0f))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);

        }



        if (cam.orthographicSize <= 1.7)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
                cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel");
        }
        else if (cam.orthographicSize >= 7)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel");
        }
        else
        {
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel");
        }

    }
}
