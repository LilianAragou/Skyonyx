using UnityEngine;
using System.Collections;

public class AttackDistEnemy : MonoBehaviour
{
    public int degats = 1;
    public Transform weapon;
    private Vector3 positionWeapon;
    public GameObject projectil;
    private GameObject projectilSave;

    public float speedProjectil = 5f;
    public float projectilLifeTime = 1.5f;
    public float reloadTime = 0.7f;
    private bool reloading;

    private Vector3 mousePos;
    private Vector3 direction;
    private float angleProjectil;

    private SpriteRenderer skin;
    private Animator anim;

    void Start() {
        skin = GetComponent<SpriteRenderer>();
        positionWeapon = weapon.localPosition;
    }

    void Update() {
        mousePos = weapon.position;
        if(!skin.flipX){
            direction = mousePos;
            direction.x -= 1;
            direction = mousePos - direction;
            angleProjectil = 0;
        } else{
            direction = mousePos;
            direction.x += 1;
            direction = mousePos - direction;
            angleProjectil = 180;
        }
        direction.Normalize();

        if (!reloading) {
            reloading = true;
            projectilSave = Instantiate(projectil, weapon.position, Quaternion.Euler(0, 0, angleProjectil));
            projectilSave.GetComponent<Rigidbody2D>().linearVelocity = direction * speedProjectil;
            projectilSave.GetComponent<ProjectileEnemy>().degats = degats;
            projectilSave.GetComponent<ProjectileEnemy>().lifeTime = projectilLifeTime;
            StartCoroutine(waitShoot());
        }
    }

    IEnumerator waitShoot() {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }
}