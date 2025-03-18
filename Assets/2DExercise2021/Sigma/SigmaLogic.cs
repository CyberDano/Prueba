using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigmaLogic : MonoBehaviour
{
    public float TimeToAttack;
    public int Attacks;
    public float Speed;
    public Animator Anim;
    private float AttackTime;
    private bool bAttacking;
    public GameObject Player;
    public Vector3 MovementVect;

    // Start is called before the first frame update
    void Start()
    {
        //Anim = gameObject.GetComponent<Animator>();
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!bAttacking) {
            MovementVect = new Vector3((Player.transform.position.x - gameObject.transform.position.x), (Player.transform.position.y - gameObject.transform.position.y), 0);
            gameObject.transform.Translate(MovementVect.normalized * (Speed * (MovementVect.magnitude/10)));
            AttackTime += Time.deltaTime;
        }
        
        if (AttackTime >= TimeToAttack) {
            bAttacking = true;
            int r = Random.Range(1, Attacks+1);
            Anim.SetInteger("Attack", r);
            AttackTime = 0;
        }
    }
    public void EndAttack() {
        bAttacking = false;
        Anim.SetInteger("Attack", 0);
    }
}
