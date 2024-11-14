using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -10);
    [SerializeField] private float followSpeed = 5f; // �������� ���������� ������
    [SerializeField] private float rotationSpeed = 5f; // �������� �������� ������

    [Header("Zoom Settings")]
    [SerializeField] private float minZoom = 5f; // ����������� �������� ����
    [SerializeField] private float maxZoom = 20f; // ������������ �������� ����
    [SerializeField] private float zoomSpeed = 2f; // �������� ��������� ����

    private Transform target;
    private float currentZoom = 10f; // ������� ���

    private void Start()
    {
        currentZoom = offset.magnitude;
    }

    private void Update()
    {
        HandleZoom();
    }

    private void LateUpdate()
    {
        if (target != null)
            FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = target.position + offset.normalized * currentZoom;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scrollData * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
