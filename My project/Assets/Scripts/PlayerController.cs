using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public ColorType currentColor = ColorType.Red;
    public Renderer[] bodyRenderers;
    public Material[] colorMaterials;
    public float jumpForce = 6f;
    public float duckDuration = 0.6f;
    public Animator animator;

    // Level-wise speed
    public int currentLevel = 1;
    public float minSpeed = 2f;
    public float maxSpeed = 15f;
    public int tilesPerLevel = 40;
    public int maxLevel = 10;

    Rigidbody rb;
    bool isGrounded = true;
    public bool isDucking = false;    // made public so other scripts can check if needed
    Vector2 touchStart;
    float swipeThreshold = 80f;

    private int tilesPassed = 0;
    private float forwardSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        UpdateColorVisual();
        forwardSpeed = minSpeed;
    }

    void Update()
    {
        HandleKeyboard();
        HandleTouch();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (GameManager.isGameOver) return; // 🚨 Stop movement when game over

        rb.MovePosition(transform.position + Vector3.forward * forwardSpeed * Time.fixedDeltaTime);
        UpdateLevelSpeed();
    }

    void HandleKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) CycleColor(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) CycleColor(1);
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) StartCoroutine(Duck());
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;
        Touch t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began) touchStart = t.position;
        else if (t.phase == TouchPhase.Ended)
        {
            Vector2 delta = t.position - touchStart;
            if (Mathf.Abs(delta.y) > swipeThreshold && Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
            {
                if (delta.y > 0) Jump(); else StartCoroutine(Duck());
            }
            else
            {
                if (t.position.x < Screen.width / 2) CycleColor(-1); else CycleColor(1);
            }
        }
    }

    public void CycleColor(int dir)
    {
        int next = ((int)currentColor + dir) % System.Enum.GetNames(typeof(ColorType)).Length;
        if (next < 0) next += System.Enum.GetNames(typeof(ColorType)).Length;
        currentColor = (ColorType)next;
        UpdateColorVisual();
        animator.SetTrigger("ColorChange");
    }

    void UpdateColorVisual()
    {
        Material m = colorMaterials[(int)currentColor];
        foreach (var r in bodyRenderers) r.material = m;
    }

    public void Jump()
    {
        if (!isGrounded || isDucking) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        isGrounded = false;
    }

    // Keep this coroutine for player-initiated duck
    public IEnumerator Duck()
    {
        if (isDucking) yield break;
        isDucking = true;
        yield return new WaitForSeconds(duckDuration);
        isDucking = false;
    }

    // Public API other scripts (props) can call to force the duck animation for specific duration
    public void ForceDuck(float overrideDuration = -1f)
    {
        if (isDucking) return; // if already ducking, ignore
        if (overrideDuration > 0f)
            StartCoroutine(ForcedDuckRoutine(overrideDuration));
        else
            StartCoroutine(Duck());
    }

    IEnumerator ForcedDuckRoutine(float dur)
    {
        isDucking = true;
        yield return new WaitForSeconds(dur);
        isDucking = false;
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            tilesPassed++;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Tile tile = other.GetComponent<Tile>();
        if (tile != null)
        {
            if (tile.tileColorType != currentColor)
            {
                GameManager.GameOver();
                rb.linearVelocity = Vector3.zero; // cancel momentum immediately
                animator.SetBool("IsGameOver", true); // 🚨 trigger idle state
            }
        }
    }

    void UpdateAnimator()
    {
        animator.SetFloat("SpeedY", rb.linearVelocity.y);
        animator.SetFloat("Duck", isDucking ? 1f : 0f);
    }

    void UpdateLevelSpeed()
    {
        currentLevel = (tilesPassed / tilesPerLevel) + 1;
        int clampedLevel = Mathf.Min(currentLevel, maxLevel);
        float t = (float)(clampedLevel - 1) / (maxLevel - 1);
        forwardSpeed = Mathf.Lerp(minSpeed, maxSpeed, t);
    }
}
