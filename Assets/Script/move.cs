using UnityEngine;

public class Move : MonoBehaviour
{
    public PlayerStats stats;
    public Transform shadowPrefab;

    private Rigidbody2D rb;
    private CapsuleCollider2D monColl;
    private bool grounded;
    private float rayonDetection;
    private bool wallSliding;
    private bool touchingWallLeft, touchingWallRight;
    private SpriteRenderer skin;
    private Animator anim;
    private Transform shadow;
    private float lastWallJumpTime;
    public Transform groundChecker;
    public LayerMask wallLayer;
    bool touchingWallLeftFeet;
    bool touchingWallRightFeet;
    bool touchingWallLeftMid;
    bool touchingWallRightMid;

    void Start()
    {
        monColl = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        skin = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
        shadow.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
    }

    void Update()
    {
        groundCheck();
        moveCheck();
        if (grounded)
        {
            flipCheck();
        }
        animCheck();
        shadowUpdate();
        checkWall();

        if (Input.GetKeyDown(KeyCode.E))
        {
            SwapWithShadow();
        }
    }

    void checkWall()
    {
        float checkDistance = 0.75f;
        Vector2 position = transform.position;
        float colliderHeight = monColl.size.y * 0.5f;

        // Position des pieds du joueur (on part du centre et on descend)
        Vector2 feetPosition = position + Vector2.down * (colliderHeight - 0.05f);

        // Position intermédiaire entre le corps et les pieds
        Vector2 midPosition = position + Vector2.down * (colliderHeight * 0.5f);

        // Raycasts principaux (hauteur du corps)
        RaycastHit2D hitLeft = Physics2D.Raycast(position, Vector2.left, checkDistance, wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(position, Vector2.right, checkDistance, wallLayer);

        // Raycasts au niveau des pieds
        RaycastHit2D hitLeftFeet = Physics2D.Raycast(feetPosition, Vector2.left, checkDistance, wallLayer);
        RaycastHit2D hitRightFeet = Physics2D.Raycast(feetPosition, Vector2.right, checkDistance, wallLayer);

        // Raycasts entre le corps et les pieds
        RaycastHit2D hitLeftMid = Physics2D.Raycast(midPosition, Vector2.left, checkDistance, wallLayer);
        RaycastHit2D hitRightMid = Physics2D.Raycast(midPosition, Vector2.right, checkDistance, wallLayer);

        // Booleens de contact
        touchingWallLeft = hitLeft.collider != null;
        touchingWallRight = hitRight.collider != null;
        touchingWallLeftFeet = hitLeftFeet.collider != null;
        touchingWallRightFeet = hitRightFeet.collider != null;
        touchingWallLeftMid = hitLeftMid.collider != null;
        touchingWallRightMid = hitRightMid.collider != null;

        wallSliding = !grounded && (touchingWallLeft || touchingWallRight);
    }

    void groundCheck()
    {
        grounded = false;
        rayonDetection = monColl.size.x * 0.45f;
        Collider2D[] colls = Physics2D.OverlapCircleAll((Vector2)transform.position + Vector2.up * (monColl.offset.y + rayonDetection * 0.8f - (monColl.size.y / 2)), rayonDetection);
        foreach (Collider2D coll in colls)
        {
            if (coll != monColl && !coll.isTrigger)
            {
                grounded = true;
                break;
            }
        }
    }

    void moveCheck()
    {
        Debug.Log("grounded : " + grounded);
        Debug.Log("wall_slide : " + wallSliding);
        Vector2 velocity = rb.linearVelocity;
        if (grounded)
        {
            velocity.x = Input.GetAxis("Horizontal") * stats.speed;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                velocity.y = stats.jumpForce;
            }
            else if (wallSliding && Time.time > lastWallJumpTime + 0.4f)
            {
                lastWallJumpTime = Time.time;
                velocity.y = stats.jumpForce;
                velocity.x = touchingWallLeft ? stats.wallJumpForce : -stats.wallJumpForce;
            }
        }

        rb.linearVelocity = velocity;
    }

    void flipCheck()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            skin.flipX = Input.GetAxis("Horizontal") < 0;
        }
    }

    void animCheck()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("velocityY", rb.linearVelocity.y);
        anim.SetBool("grounded", grounded);
    }

    void shadowUpdate()
    {
        if (shadow != null)
        {
            Vector3 mirroredPosition = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
            shadow.position = mirroredPosition;
        }
    }

    void SwapWithShadow()
    {
        if (shadow == null || shadow.GetComponent<Collider2D>() == null) return;

        Vector3 originalPosition = transform.position;
        Vector3 shadowPosition = shadow.position;

        // Vérifier les collisions avec la shadow en utilisant son BoxCollider2D
        Collider2D[] results = new Collider2D[10];
        int count = shadow.GetComponent<Collider2D>().Overlap(new ContactFilter2D().NoFilter(), results);

        foreach (var coll in results)
        {
            // Vérifie les collisions avec d'autres objets, sauf le joueur lui-même
            if (coll != null && coll.gameObject != gameObject && !coll.isTrigger)
            {
                // Si une collision indésirable est détectée, annule le swap et renvoie le joueur à sa position originale
                transform.position = originalPosition;
                GetComponent<playerLife>()?.SendMessage("jeSuisMouru");
                return;
            }
        }

        // Si aucune collision indésirable n'est détectée, effectuer le swap
        transform.position = shadowPosition;

        // Réinitialise la position de la shadow à l'ancienne position du joueur
        shadow.position = originalPosition;
    }
}