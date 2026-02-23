using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    private SpriteRenderer spriteRenderer;
    private PlayerState playerState;
    private Animator animator;

    [SerializeField] private Sprite idleImage, backImage, frontImage, leftImage, rightImage;
    [Header("옷 입은 버전 이미지")]
    [SerializeField] private Sprite idleClothed, backClothed, frontClothed, leftClothed, rightClothed;
    [SerializeField] private RuntimeAnimatorController clothedAnimator; // 옷 입은 버전 애니메이터

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerState = GetComponent<PlayerState>();
        animator = GetComponent<Animator>();
        // 이미지가 설정되어 있지 않을 경우를 대비한 안전장치
        if (frontImage != null) idleImage = frontImage;

        // 시작 지점 초기화
        if (playerState != null)
        {
            playerState.currentStartPoint = transform.position;
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        UpdateSpriteAndAnimation(moveX, moveY);
        Move(moveX, moveY);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerState.getHatsCount > 0 && !playerState.isInvincible)
            {
                playerState.UpdateHatCount(-1);
                playerState.isInvincible = true;
                StageManager.Instance.ShowMessage("Player use Hat!\nCan ignore trap once!!");
            }
        }
    }

    void UpdateSpriteAndAnimation(float x, float y)
    {
        bool isMoving = (x != 0 || y != 0);

        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
            if (isMoving)
            {
                animator.SetFloat("MoveX", x);
                animator.SetFloat("MoveY", y);
            }
        }

        if (x < 0) spriteRenderer.sprite = rightImage;
        else if (x > 0) spriteRenderer.sprite = leftImage;
        else if (y < 0) spriteRenderer.sprite = frontImage;
        else if (y > 0) spriteRenderer.sprite = backImage;
        else spriteRenderer.sprite = idleImage;

        if (isMoving)
        {
            idleImage = spriteRenderer.sprite;
        }
    }

    public void ChangeToClothedRabbit()
    {
        // 1. 기본 이미지들을 옷 입은 버전으로 교체
        idleImage = idleClothed;
        backImage = backClothed;
        frontImage = frontClothed;
        leftImage = leftClothed;
        rightImage = rightClothed;

        // 2. 현재 보여지는 이미지 즉시 업데이트
        spriteRenderer.sprite = idleImage;

        // 3. 애니메이터 컨트롤러 교체 (필요 시)
        if (clothedAnimator != null)
        {
            GetComponent<Animator>().runtimeAnimatorController = clothedAnimator;
        }
    }

    void Move(float x, float y)
    {
        // 대각선 이동 시 빨라지는 것을 방지하기 위해 .normalized 추가
        Vector3 moveDir = new Vector3(x, y, 0).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}