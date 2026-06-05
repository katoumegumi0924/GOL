using UnityEngine;

/// <summary>
/// CameraController：
/// </summary>
public class CameraController
{
    public LifeData lifeData;

    public Camera mainCamera;

    private Vector2 _camera_position;
    private float _camera_zoom;

    private const float MOVE_SPEED = 2.0f;
    private const float ZOOM_SPEED = 2.0f;
    private const float MAX_ZOOM = 10f;
    private const float MIN_ZOOM = 0.1f;


    public void Init(LifeData _lifeData)
    {
        lifeData = _lifeData;

        mainCamera = Camera.main;

        _camera_position = mainCamera.transform.position;
        _camera_zoom = mainCamera.orthographicSize;
    }

    public void Free()
    {
        lifeData = null;

        mainCamera = null;

        _camera_position = Vector2.zero;
        _camera_zoom = 0f;
    }

    public void SetNew()
    {

    }

    public void Update()
    {
        CameraUpdate();
    }

    private void CameraUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            _camera_zoom -= scroll * ZOOM_SPEED;
            _camera_zoom = Mathf.Clamp(_camera_zoom, MIN_ZOOM, MAX_ZOOM);

            mainCamera.orthographicSize = _camera_zoom;
        }

        Vector2 moveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDir.y += 1.0f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDir.y -= 1.0f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x -= 1.0f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x += 1.0f;
        }

        if (moveDir != Vector2.zero)
        {
            _camera_position += moveDir * MOVE_SPEED * _camera_zoom * Time.deltaTime;
            mainCamera.transform.position = new Vector3(_camera_position.x, _camera_position.y, -10); ;
        }
    }
}
