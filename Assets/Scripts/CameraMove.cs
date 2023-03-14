using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform _follower;
    [SerializeField] Transform _realCam;

    float rotX;
    float rotY;

    [SerializeField] float minClampAngle = 25;
    [SerializeField] float maxClampAngle = 45;

    [SerializeField] float sensitivity = 200;

    [SerializeField] float followSpeed = 5;

    Vector3 finalDir;
    float maxDistance = 5;
    float minDistance = 2;

    Vector3 dirNormal;
    float finalDis;

    float smoothness = 3;

    bool _isMouseVisible = false;


    // Start is called before the first frame update
    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormal = _realCam.localPosition.normalized;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _follower.position, Time.deltaTime * followSpeed);

        finalDir = transform.TransformPoint(dirNormal * maxDistance);

        RaycastHit hit;
        //Debug.DrawLine(transform.position, finalDir, Color.green, 5);
        if (Physics.Linecast(transform.position, finalDir, out hit))
        {
            finalDis = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDis = maxDistance;
        }

        _realCam.localPosition = Vector3.Lerp(_realCam.localPosition, dirNormal * finalDis, Time.deltaTime * smoothness);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _isMouseVisible = false;
        }
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _isMouseVisible= true;
        }
        if (_isMouseVisible) return;
        rotX += Input.GetAxis("Mouse Y")* -1 * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -minClampAngle, maxClampAngle);

        Quaternion rot = Quaternion.Euler(rotX,rotY,0);
        transform.rotation = rot;
        transform.position = _follower.position;

    }
}
