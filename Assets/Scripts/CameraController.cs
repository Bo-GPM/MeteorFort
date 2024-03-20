using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera cam;
    bool dragPanMoveActive;
    private Vector2 lastMousePos;
    Vector3 originialPosition = Vector3.zero;
    float originalSize;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;
    [SerializeField] float zoomStep;
    [SerializeField] [Range(0, 0.02f)] private float cameraPanningSpeed = 0.03f;
    float cameraSize;
    private void Awake()
    {
        originialPosition = transform.position;
        originalSize = cam.fieldOfView;
        cameraSize = originalSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDir.y = 1f;
        if (Input.GetKey(KeyCode.S)) moveDir.y = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = 1f;
        //int edgeScrollSize = 20;
        //if (Input.mousePosition.x < edgeScrollSize) moveDir.x = -1f;
        //if (Input.mousePosition.y < edgeScrollSize) moveDir.y = -1f;
        //if (Input.mousePosition.x > Screen.width - edgeScrollSize) moveDir.x = 1f;
        //if (Input.mousePosition.y > Screen.height - edgeScrollSize) moveDir.y = 1f;
        if (Input.GetMouseButtonDown(1))
        {
            dragPanMoveActive = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            dragPanMoveActive = false;
        }
        if (dragPanMoveActive)
        {
            // Adjust panning speed based on FOV. Change cameraPanningSpeed to make it feels right :)
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePos;
            float panningSpeedFactor = cam.fieldOfView / cameraSize;
            moveDir = new Vector3(-mouseMovementDelta.x * panningSpeedFactor, -mouseMovementDelta.y * panningSpeedFactor, 0) * cameraPanningSpeed;
            transform.Translate(moveDir, Space.World);
            
            lastMousePos = Input.mousePosition;

        }
        float moveSpeed = 50f;
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = originialPosition;
            cam.orthographicSize = originalSize;
            cameraSize = originalSize;
        }


        if (Input.mouseScrollDelta.y > 0)
        {
            cameraSize -= zoomStep;

        }
        if (Input.mouseScrollDelta.y < 0)
        {
            cameraSize += zoomStep;
        }
        cameraSize = Mathf.Clamp(cameraSize, minSize, maxSize);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, cameraSize, 10 * Time.deltaTime);

    }
}
