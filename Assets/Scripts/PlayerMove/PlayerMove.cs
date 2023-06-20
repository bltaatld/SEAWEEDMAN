using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    [Header("* Weed System")]
	public bool isWeedGrow;
	public bool isWeedEnd;
    public float weedGage;
    public GameObject weedPrefab;
	public GameObject rotateObject;

    [Header("* Player Move")]
    public float maxSpeed;
	public float currentSpeed;
	public float distanceFromWall = 0.1f;
    public float moveDuration = 3f;
    public float moveTimer = 0f;
	public bool isMoving = false;
	public bool CanMove;
    public LayerMask wallLayer;
    public SpriteRenderer curserRender;
    public GameObject gageRender;
    public Animator anim;

    private bool isButtonPressed = false;
	private Vector2 targetPosition;
	private Vector2 startPosition;

    private void Start()
    { 
        currentSpeed = maxSpeed;
    }

    void Update()
    {
        transform.Rotate(Vector3.back * currentSpeed * Time.deltaTime);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // ù ��° ��ġ �Է��� Ȯ���ϰ�, UI ��ư�� Ŭ������ �ʾ��� �� �Լ��� �۵��մϴ�.
            if (touch.phase == TouchPhase.Began && !isButtonPressed && !isMoving && !CanMove && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (weedGage > 0)
                {
                    Camera.main.orthographicSize = 40f;
                    currentSpeed = 0f;

                    if (!isWeedEnd)
                    {
                        isWeedGrow = true;
                    }

                    if (isWeedEnd)
                    {
                        currentSpeed = maxSpeed;

                        moveTimer = 0f;
                        startPosition = transform.position;

                        isMoving = true;
                        // �÷��̾� ȸ�� ��
                        float playerAngle = transform.eulerAngles.z;

                        // ������ ��������
                        float angleInRadians = playerAngle * Mathf.Deg2Rad;

                        // �÷��̾��� ȸ���� �����Ͽ� Raycast ������ �����մϴ�.
                        Vector2 playerDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

                        // Raycast�� �����Ͽ� ������ �浹�� �����մϴ�.
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, Mathf.Infinity, wallLayer);

                        // Raycast�� ���� �¾��� ���� ó���� �����մϴ�.
                        if (hit.collider != null)
                        {
                            Vector2 hitPoint = hit.point;
                            Debug.Log(hitPoint);
                            targetPosition = hitPoint - playerDirection * distanceFromWall;
                        }

                        Quaternion currentRotation = transform.rotation;

                        // ���� rotation�� z ���� 180�� ȸ���մϴ�.
                        currentRotation *= Quaternion.Euler(0f, 0f, 180f);

                        // ����� rotation ���� �����մϴ�.
                        transform.rotation = currentRotation;
                        isWeedEnd = false;
                    }
                }
                if(weedGage <= 0)
                {
                    GameManager.instance.timeRemaining = 10f;
                }
            }
        }

        if (isMoving)
        {
            moveTimer += Time.deltaTime;

            if (moveTimer <= moveDuration)
            {
                isWeedEnd = false;
                currentSpeed = 0f;
                float t = moveTimer / moveDuration;
                transform.position = Vector2.Lerp(startPosition, targetPosition, t);
                anim.SetBool("IsMove", true);
                curserRender.gameObject.SetActive(false);
                gageRender.SetActive(false);
            }
            else
            {
                isMoving = false;
                isWeedEnd = false;
                currentSpeed = maxSpeed;
                anim.SetBool("IsMove", false);
                curserRender.gameObject.SetActive(true);
                gageRender.SetActive(true);
                GameManager.instance.cameraHandler.CenterCameraOnPlayerPosition();
            }
        }

        if (isWeedGrow)
        {
            Quaternion rotation = transform.rotation;
            rotation *= Quaternion.Euler(0f, 0f, -90f);
            GameObject instance = Instantiate(weedPrefab, transform.position, rotation);
            instance.transform.parent = rotateObject.transform;
            isWeedGrow = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Ground"))
		{
            currentSpeed *= -1f;
		}
	}
}
