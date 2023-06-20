using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGyro : MonoBehaviour
{
    public bool isGyroActive;
    public GameObject cam;
    private Gyroscope gyro;
    private Quaternion initialRotation;
    private float zAngle;

    // Start is called before the first frame update
    void Start()
    {
        // ���̷� ������ �ʱ�ȭ�մϴ�.
        gyro = Input.gyro;
        gyro.enabled = true;
        isGyroActive = false;

        // �ʱ� ȸ������ �����մϴ�.
        initialRotation = gyro.attitude;
    }

    // Update is called once per frame
    void Update()
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
