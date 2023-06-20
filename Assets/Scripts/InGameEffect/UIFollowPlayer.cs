using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollowPlayer : MonoBehaviour
{
    public Transform playerTransform; // �÷��̾� ������Ʈ�� Transform ������Ʈ
    public Image uiImage; // ����ٴ� UI Image ������Ʈ

    private void LateUpdate()
    {
        transform.position = playerTransform.position;
        transform.rotation = playerTransform.rotation;
        uiImage.fillAmount = GameManager.instance.playerMove.weedGage / 100f;
    }
}
