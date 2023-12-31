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

            // 첫 번째 터치 입력을 확인하고, UI 버튼을 클릭하지 않았을 때 함수를 작동합니다.
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
                        // 플레이어 회전 값
                        float playerAngle = transform.eulerAngles.z;

                        // 각도를 라디안으로
                        float angleInRadians = playerAngle * Mathf.Deg2Rad;

                        // 플레이어의 회전을 적용하여 Raycast 방향을 설정합니다.
                        Vector2 playerDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

                        // Raycast를 수행하여 벽과의 충돌을 감지합니다.
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, Mathf.Infinity, wallLayer);

                        // Raycast가 벽에 맞았을 때의 처리를 수행합니다.
                        if (hit.collider != null)
                        {
                            Vector2 hitPoint = hit.point;
                            Debug.Log(hitPoint);
                            targetPosition = hitPoint - playerDirection * distanceFromWall;
                        }

                        Quaternion currentRotation = transform.rotation;

                        // 현재 rotation의 z 값을 180도 회전합니다.
                        currentRotation *= Quaternion.Euler(0f, 0f, 180f);

                        // 변경된 rotation 값을 적용합니다.
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
