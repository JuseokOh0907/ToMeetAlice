using UnityEngine;

public abstract class ItemAndObject : MonoBehaviour
{
    [Header("가시성 설정")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float fadeStartDistance = 3f;
    [SerializeField] protected float fadeEndDistance = 2f;

    [Header("상호작용 설정")]
    [SerializeField] protected Transform playerTransform;
    protected bool isPlayerInRange = false;

    protected virtual void Start()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (playerTransform == null) playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 시작 시 투명하게 설정
        SetAlpha(0f);
    }

    protected virtual void Update()
    {
        HandleFade();

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            OnInteract();
            SoundManager.Instance.PlaySFX(SoundManager.Instance.itemGetSound);
        }
    }

    private void HandleFade()
    {
        if (playerTransform == null || spriteRenderer == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        float targetAlpha = Mathf.Clamp01(1 - (distance - fadeEndDistance) / (fadeStartDistance - fadeEndDistance));

        SetAlpha(Mathf.Lerp(spriteRenderer.color.a, targetAlpha, Time.deltaTime * 5f));
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    // 자식 스크립트에서 이 내용을 각자 구현합니다.
    protected abstract void OnInteract();

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInRange = true;
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInRange = false;
    }
}