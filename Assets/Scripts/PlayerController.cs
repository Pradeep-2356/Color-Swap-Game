using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    public ColorType currentColor = ColorType.Red;
    public Renderer[] bodyRenderers; 
    public Material[] colorMaterials; 
    public float forwardSpeed = 5f;  
    public float jumpForce = 6f;
    public float duckDuration = 0.6f;
    public Animator animator;

    // Level-wise speed
    public int currentLevel = 1;       // Start level
    public float baseSpeed = 5f;       // Speed at level 1
    public float speedIncrement = 1f;  // How much speed increases per level
    public int tilesPerLevel = 40;     // Same as TileSpawner

    Rigidbody rb;
    bool isGrounded = true;
    bool isDucking = false;
    Vector2 touchStart;
    float swipeThreshold = 80f;

    private int tilesPassed = 0;

    void Awake(){ 
        rb = GetComponent<Rigidbody>(); 
        UpdateColorVisual(); 
    }

    void Update(){
        HandleKeyboard(); 
        HandleTouch();
        UpdateAnimator();
    }

    void FixedUpdate() {
        rb.MovePosition(transform.position + Vector3.forward * forwardSpeed * Time.fixedDeltaTime);
        UpdateLevelSpeed();
    }

    void HandleKeyboard(){
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) CycleColor(-1);
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) CycleColor(1);
        if(Input.GetKeyDown(KeyCode.Space)) Jump();
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) StartCoroutine(Duck());
    }

    void HandleTouch(){
        if(Input.touchCount==0) return;
        Touch t = Input.GetTouch(0);
        if(t.phase==TouchPhase.Began) touchStart = t.position;
        else if(t.phase==TouchPhase.Ended){
            Vector2 delta = t.position - touchStart;
            if(Mathf.Abs(delta.y) > swipeThreshold && Mathf.Abs(delta.y) > Mathf.Abs(delta.x)){
                if(delta.y > 0) Jump(); else StartCoroutine(Duck());
            } else {
                if(t.position.x < Screen.width/2) CycleColor(-1); else CycleColor(1);
            }
        }
    }

    public void CycleColor(int dir){
        int next = ((int)currentColor + dir) % System.Enum.GetNames(typeof(ColorType)).Length;
        if(next<0) next += System.Enum.GetNames(typeof(ColorType)).Length;
        currentColor = (ColorType)next;
        UpdateColorVisual();
        animator.SetTrigger("ColorChange");
    }

    void UpdateColorVisual(){
        Material m = colorMaterials[(int)currentColor];
        foreach(var r in bodyRenderers) r.material = m;
    }

    public void Jump(){
        if(!isGrounded || isDucking) return;
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        isGrounded = false;
    }

    IEnumerator Duck(){
        if(isDucking) yield break;
        isDucking = true;
        yield return new WaitForSeconds(duckDuration);
        isDucking = false;
    }

    void OnCollisionEnter(Collision c){
        if(c.gameObject.CompareTag("Ground")) {
            isGrounded = true;
            tilesPassed++;            // Increment tiles passed on landing
        }
    }

    void UpdateAnimator(){
        animator.SetFloat("SpeedY", rb.velocity.y);
        animator.SetFloat("Duck", isDucking ? 1f : 0f);
    }

    void UpdateLevelSpeed(){
        // Calculate current level based on tiles passed
        currentLevel = (tilesPassed / tilesPerLevel) + 1;
        forwardSpeed = baseSpeed + (currentLevel - 1) * speedIncrement;
    }
}

