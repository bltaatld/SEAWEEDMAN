using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeed : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public SpriteRenderer spriteRenderer;

    public float scaleSpeed = 0.2f; // ������ ���� �ӵ�

    private void Update()
    {
        GameManager.instance.playerMove.weedGage -= scaleSpeed * Time.deltaTime;

        // Box Collider 2D Offset Y �� ����
        Vector2 offset = boxCollider.offset;
        offset.y += scaleSpeed * Time.deltaTime;
        boxCollider.offset = offset;

        // Sprite Renderer Draw Mode Size Height �� ����
        Vector2 size = spriteRenderer.size;
        size.y += scaleSpeed * Time.deltaTime;
        spriteRenderer.size = size;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            scaleSpeed = 0f;
            GameManager.instance.rankValue += spriteRenderer.size.y;
            GameManager.instance.playerMove.isWeedEnd = true;
        }
    }
}
