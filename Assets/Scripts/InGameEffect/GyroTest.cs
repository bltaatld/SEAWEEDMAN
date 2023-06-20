using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroTest : MonoBehaviour
{
    public bool isGyroActive;
    public GameObject cam;
    private Gyroscope gyro;
    private Quaternion initialRotation;
    private float zAngle;

    void Start()
    {
        // ���̷� ������ �ʱ�ȭ�մϴ�.
        gyro = Input.gyro;
        gyro.enabled = true;
        isGyroActive = false;

        // �ʱ� ȸ������ �����մϴ�.
        initialRotation = gyro.attitude;
    }

    void Update()
    {
        GameManager.instance.playerMove.CanMove = isGyroActive;

        if (isGyroActive)
        {
            // ���̷� ������ ȸ�� ���� �����ɴϴ�.
            Quaternion rotation = gyro.attitude;

            // �ʱ� ȸ������ �������� ������� ȸ������ ����մϴ�.
            Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * rotation;

            // Z�� ȸ�� ������ �����մϴ�.
            zAngle = relativeRotation.eulerAngles.z;

            // ������Ʈ�� ȸ���� �����մϴ�.
            transform.rotation = Quaternion.Euler(0f, 0f, zAngle);
        }
    }

    public void ResetCameraPos()
    {
        cam.transform.position = new Vector3(0, 0, -30);
        transform.position = new Vector3(0, 0, 10);
    }

    // ȸ������ �ʱ�ȭ�ϴ� �Լ�
    public void SetRotation()
    {
        if (isGyroActive)
        {
            isGyroActive = false;
            return;
        }
        else
        {
            isGyroActive = true;
        }
    }
}