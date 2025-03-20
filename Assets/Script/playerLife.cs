using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class playerLife : MonoBehaviour
{
    public int vie = 2;
    public Slider barreDeVie;
    private int vieMax;
    private Vector3 positionSpawn;
    private bool invincible = false;
    private SpriteRenderer skin;
    private float timer;
    void Start() {
        vieMax = vie;
        positionSpawn = transform.position;
        skin = GetComponent<SpriteRenderer>();
        updateVie();
    }

    void Update() {
        if(skin.color != Color.white) {
            skin.color = Color.Lerp(Color.red, Color.white, (Time.time - timer) * 2f);
        }
    }

    void takeDamage(int damage) {
        if(!invincible) {
            vie -= damage;
            invincible = true;
            skin.color = Color.red;
            timer = Time.time;
            if (vie <= 0) {
                jeSuisMouru();
            }
            updateVie();
            StartCoroutine(waitDamage());
        }        
    }

    void updateVie() {
        barreDeVie.value = (float)vie/ (float)vieMax;
    }

    void jeSuisMouru() {
        transform.position = positionSpawn;
        vie = vieMax;
        updateVie();
    }

    IEnumerator waitDamage() {
        yield return new WaitForSeconds(0.5f);
        invincible = false;
    }

    void OnTriggerEnter2D(Collider2D truc) {
        if(truc.tag == "Kill") {
            jeSuisMouru();
        }
        if (truc.tag == "Respawn") {
            positionSpawn = transform.position;
        }
    }
}
