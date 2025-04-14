using UnityEngine;

public class ennemiPatrol : MonoBehaviour
{
    public float speed = 2f;
    [SerializeField, Range(0.1f, 50f)] private float limiteDroite = 1f;
    [SerializeField, Range(0.1f, 50f)] private float limiteGauche = 1f;
    private Vector3 limiteDroitePosition;
    private Vector3 limiteGauchePosition;
    private Rigidbody2D rb;
    private float direction = 1f;
    private SpriteRenderer skin;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skin = GetComponent<SpriteRenderer>();
        limiteDroitePosition = transform.position + new Vector3(limiteDroite, 0, 0);
        limiteGauchePosition = transform.position - new Vector3(limiteGauche, 0, 0);
    }

    void Update()
    {
        if (transform.position.x > limiteDroitePosition.x) direction = -1f;
        if (transform.position.x < limiteGauchePosition.x) direction = 1f;

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        skin.flipX = direction < 0f;
    }

    void OnDrawGizmos()
    {
        Vector3 droite = transform.position + new Vector3(limiteDroite, 0, 0);
        Vector3 gauche = transform.position - new Vector3(limiteGauche, 0, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(droite, gauche);
        Gizmos.DrawCube(droite, Vector3.one * 0.2f);
        Gizmos.DrawCube(gauche, Vector3.one * 0.2f);
    }
}
