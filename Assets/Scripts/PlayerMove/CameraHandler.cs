using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform playerTransform; // �÷��̾��� Transform ������Ʈ
    public Transform childTransform;
    private Vector3 childeOffset; // ī�޶�� �÷��̾� ������ �Ÿ�


    public void CenterCameraOnPlayerPosition()
    {
        childeOffset = childTransform.position;
        Vector3 targetPosition = playerTransform.position;
        Camera.main.transform.position = new Vector3(targetPosition.x, targetPosition.y, Camera.main.transform.position.z);
        childTransform.position = childeOffset;
    }
}
