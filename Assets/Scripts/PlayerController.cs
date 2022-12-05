using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _mouseSensity = 120.0f;
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _shootDistance = 50.0f;
    [SerializeField] private float _shootForce = 15.0f;

    private float headRotation;
    private Camera camera;
    private Rigidbody RbPlayer;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponentInChildren<Camera>();
        headRotation = camera.transform.rotation.x;
        RbPlayer = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerView();
        PlayerMove();
        if (Input.GetButtonDown("Shoot"))
        {
            Shoot();
        }
    }

    private void PlayerView()
    {
        float xM = Input.GetAxis("Mouse X") * _mouseSensity * Time.deltaTime;
        float yM = Input.GetAxis("Mouse Y") * _mouseSensity * Time.deltaTime;

        headRotation -= yM;
        headRotation = Mathf.Clamp(headRotation, -90.0f, 90.0f);

        transform.Rotate(0, xM, 0);
        camera.transform.localRotation = Quaternion.Euler(headRotation, 0, 0);
    }

    private void PlayerMove()
    {
        float HorizontalMove = Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        float VerticalMove = Input.GetAxis("Vertical") * _speed * Time.deltaTime;

        Vector3 Move = new Vector3(HorizontalMove, 0, VerticalMove);

        RbPlayer.MovePosition(RbPlayer.position + Move);

        //transform.Translate(Vector3.right * HorizontalMove * _speed * Time.deltaTime);
        //transform.Translate(Vector3.forward * VerticalMove * _speed * Time.deltaTime);
    }

    private void Shoot()
    {
        Ray RayShoot = new Ray(camera.transform.position,camera.transform.forward);
        RaycastHit Shoot;

        if (Physics.Raycast(RayShoot,out Shoot, _shootDistance) && Shoot.rigidbody != null)
        {
            Vector3 ForceVector = -Shoot.normal * _shootForce;
            Shoot.rigidbody.AddForceAtPosition(ForceVector,Shoot.point,ForceMode.Impulse);
        }
    }
}
