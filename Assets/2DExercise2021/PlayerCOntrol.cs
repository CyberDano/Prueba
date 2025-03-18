using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerCOntrol : MonoBehaviour
{
    [Header("Cosas modificables")]
    public float speed = 0.1f;
    public float JumpStr = 1f;
    public CamaraEjes MisEjes;
    public bool PuedeSaltarDesdeLaPared;
    public KeyCode Izquierda;
    public KeyCode Derecha;
    public KeyCode Saltar;
    public float MuerteY;
    [Header("Estados y Partes")]
    public Animator Anim;
    public SpriteRenderer SpriteR;
    public Rigidbody2D RBody;
    public TextMeshProUGUI Timertext;
    public Text TimerText2;
    public Collider2D BoxCol;
    public Vector3 StartPos;
    public enum CamaraEjes { EjeX, EjeY, Ambos };    
    public bool bSpawn;
    public bool bLeft;
    public bool bRight;
    public bool bMovingLeft;
    public bool bMovingRight;
    public bool bJump;
    public bool bFall;
    public bool bWall;
    public bool bAttack;
    public bool bFalling;
    public bool bCling;
    public bool bAgacharse;
    public bool bDead;
    public bool bWin;
    public int VertSpeed;
    private int PrevVertSpeed;
    //public Vector3 StartPos;
    public float timepassed;
    public int secspassed;
    private float GravityScale;
    [Header("Topes")]
    public bool Bot;
    public bool Left;
    public bool Right;
    public bool Top;
    public GameObject BotCollider;
    private GameObject SideCollider;
    public RaycastHit2D[] Cho;
    [Header("Checkpoints")]
    public GameObject ControladorPrincipal;

    // Start is called before the first frame update
    void Start()
    {
        //if(GameObject.Find("Controlador") == null){
        //   GameObject g = Instantiate(ControladorPrincipal);
        //    g.name = "Controlador";
        //}
        GravityScale = GetComponent<Rigidbody2D>().gravityScale;
        StartPos = gameObject.transform.position;
        if (GameObject.Find("Timer"))
        {
            Timertext = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, (-transform.up *(BoxCol.bounds.extents.y * 1.1f)), Color.red, 0);
        LayerMask mask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, BoxCol.bounds.extents.x, - transform.up, BoxCol.bounds.extents.y*0.6f, mask);
        RaycastHit2D Lefthit = Physics2D.CircleCast(transform.position, BoxCol.bounds.extents.y/2, -transform.right, BoxCol.bounds.extents.x * 0.65f, mask);
        RaycastHit2D Righthit = Physics2D.CircleCast(transform.position, BoxCol.bounds.extents.y/2, transform.right, BoxCol.bounds.extents.x * 0.65f, mask);
        
        if (hit.collider != null)
        {
            Bot = true;
            RBody.gravityScale = 1;
            bCling = false;
            bFall = false;
            Anim.SetBool("WallCling", false);
            //BotCollider = hit.collider.gameObject;
        }
        else
        {
            Bot = false;
        }
        if (Lefthit.collider != null)
        {           
            Left = true;
            //BotCollider = hit.collider.gameObject;
        }
        else
        {
            Left = false;
        }
        if (Righthit.collider != null)
        {
            Right = true;
            //BotCollider = hit.collider.gameObject;
        }
        else
        {
            Right = false;
        }
        if (!bWin) {
            timepassed += Time.deltaTime;
            if (TimerText2 != null)
            {
                TimerText2.color = Color.white;
            }
           
        }
        if (bWin)
        {
            if (TimerText2 != null) {
                TimerText2.color = Color.red;
            }
           
        }
        
        secspassed = Mathf.FloorToInt(timepassed);
        //Debug.Log(RBody.velocity);
        PrevVertSpeed = VertSpeed;
        VertSpeed = Mathf.CeilToInt(RBody.velocity.y);
        Anim.SetInteger("VertVelocity", VertSpeed);
        if (MisEjes == CamaraEjes.EjeX) {
            Camera.main.transform.position = new Vector3(gameObject.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);

        }
        if (MisEjes == CamaraEjes.EjeY)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z);

        }
        if (MisEjes == CamaraEjes.Ambos)
        {
            Camera.main.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z);

        }
        if (TimerText2 != null) {
            TimerText2.text = (secspassed / 60).ToString("00") + ":" + (secspassed % 60).ToString("00");
        }
        //if (Input.GetKeyDown(KeyCode.A) && !bRight && !bSpawn && !bWin)
        //{
        //    bLeft = true;
        //    Anim.SetBool("bLeft", true);
        //    Anim.SetBool("bRight", false);
        //}
        //if (Input.GetKeyUp(KeyCode.A) && !bSpawn && !bWin) {
        //    bLeft = false;
        //}
        //if (Input.GetKeyUp(KeyCode.D)  && !bSpawn && !bWin)
        //{
        //    bRight = false;
        //}
        //if (Input.GetKeyDown(KeyCode.D) && !bLeft && !bSpawn && !bWin)
        //{
        //    Anim.SetBool("bLeft", false);
        //    Anim.SetBool("bRight", true);
        //    bRight = true;
        //}
        if (Input.GetKeyDown(Izquierda) && !bSpawn && !bWin)
        {
            bLeft = true;
            if (!bRight) {
                bMovingLeft = true;
            }
        }
        if (Input.GetKeyUp(Izquierda) && !bSpawn && !bWin)
        {
            bLeft = false;
            if (bRight && !bMovingRight)
            {
                bMovingRight = true;
            }
            if (bMovingLeft) {
                bMovingLeft = false;
            }
        }
        if (Input.GetKeyUp(Derecha) && !bSpawn && !bWin)
        {
            bRight = false;
            if (bLeft && !bMovingLeft)
            {
                bMovingLeft = true;
            }
            if (bMovingRight)
            {
                bMovingRight = false;
            }
        }
        if (Input.GetKeyDown(Derecha) && !bSpawn && !bWin)
        {
            bRight = true;
           
            if (!bLeft)
            {
                bMovingRight = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.S) && !bRight && !bSpawn && !bWin)
        {
            Anim.SetBool("bDown", true);
            bAgacharse = true;
        }
        if (Input.GetKeyUp(KeyCode.S) && !bSpawn && !bWin)
        {
            Anim.SetBool("bDown", false);
            bAgacharse = false;
        }
        if (bMovingLeft && !bDead && !bSpawn && !bWin && !bAgacharse)
        {
            if (!bJump)
            {
                SpriteR.flipX = true;
                if (!Left) {
                    transform.Translate(new Vector3(-3f * speed * Time.deltaTime, 0, 0));
                    Anim.SetBool("bLeft", true);
                    Anim.SetBool("bRight", false);
                }

            }
            if (bJump && !bCling)
            {
                SpriteR.flipX = true;
                if (!Left)
                {
                    transform.Translate(new Vector3(-1.4f * speed * Time.deltaTime, 0, 0));
                }
                
            }
        }
        if (bMovingRight && !bDead && !bSpawn && !bWin && !bAgacharse)
        {
            if (!bJump)
            {
                SpriteR.flipX = false;
                if (!Right)
                {
                    transform.Translate(new Vector3(3 * speed * Time.deltaTime, 0, 0));
                    Anim.SetBool("bLeft", false);
                    Anim.SetBool("bRight", true);
                }

                //RBody.MovePosition(new Vector3(1 * speed, 0, 0));
            }
            if (bJump && !bCling)
            {
                SpriteR.flipX = false;
                if (!Right)
                {
                    transform.Translate(new Vector3(1.4f * speed * Time.deltaTime, 0, 0));
                }
            }
        }
        if (!bRight && !bLeft)
        {
            Anim.SetBool("bLeft", false);
            Anim.SetBool("bRight", false);
        }
        if (!Left && !Right && bCling) {
            bCling = false;
            Anim.SetBool("WallCling", false);
        }
        if (gameObject.transform.position.y <= MuerteY && PrevVertSpeed < 0)
        {
            bDead = true;
            RBody.velocity = new Vector3(0, 0, 0);
            RBody.gravityScale = 0;
            Anim.SetTrigger("Die");
        }
        if (PrevVertSpeed >= 0 && VertSpeed < 0 && (bJump || bSpawn))
        {
            bFall = true;
        }

        if (VertSpeed > 0 && (Bot || Top))
        {
            RBody.gravityScale = GravityScale;
            RBody.velocity = new Vector3(0, 0, 0);
            bFall = true;
        }
        if (VertSpeed == 0 && !Bot)
        {

            RBody.gravityScale = GravityScale;
            bFall = true;
        }
        if (VertSpeed < 0 && bFall)
        {
            if (Right)
            {
                Anim.SetBool("WallCling", true);
                bCling = true;
                RBody.gravityScale = GravityScale / 2;
                SpriteR.flipX = false;
            }
            if (Left)
            {
                Anim.SetBool("WallCling", true);
                bCling = true;
                RBody.gravityScale = GravityScale / 2;
                SpriteR.flipX = true;
            }
            if (VertSpeed <= -23 && bCling)
            {
                RBody.velocity = new Vector3(RBody.velocity.x, -23, 0);

            }
            if (Bot) {
                if (!bSpawn)
                {
                    //Debug.LogWarning("Chocado con suelo " + bCling);
                    RBody.velocity = new Vector3(0, 0, 0);
                    //RBody.gravityScale = 0;
                    //gameObject.transform.position = new Vector3(gameObject.transform.position.x, (BotCollider.transform.position.y + BotCollider.GetComponent<Collider2D>().bounds.extents.y + gameObject.GetComponent<Collider2D>().bounds.extents.y), gameObject.transform.position.z);
                    //bJump = false;
                    //bFall = false;
                    Anim.SetBool("WallCling", false);
                    if (bCling) {
                        if (Left) {
                            SpriteR.flipX = false;
                        }
                        if (Right) {
                            SpriteR.flipX = true;
                        }
                        bCling = false;
                    }
                }
                if (bSpawn)
                {
                    RBody.velocity = new Vector3(0, 0, 0);
                    RBody.gravityScale = 0;
                    bJump = false;
                    bFall = false;
                    Anim.SetTrigger("Spawn");
                    Anim.SetBool("WallCling", false);
                    if (bCling)
                    {
                        
                        bCling = false;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Y) && !bJump && !bDead && !bSpawn && !bWin && VertSpeed == 0)
        {
            bAttack = true;
            Anim.SetTrigger("Attack");
        }
        if (Input.GetKeyDown(Saltar) && Bot && !bDead && !bSpawn && !bWin && VertSpeed == 0)
        {
            Debug.Log("Jump");
            RBody.AddForce(new Vector2(0, 300*JumpStr));
        }
        if (Input.GetKeyDown(Saltar) && !Bot && !bDead && !bSpawn && !bWin && PuedeSaltarDesdeLaPared)
        {
           
            if (Left)
            {
                bFall = false;
                RBody.velocity = new Vector3(0, 0, 0);
                //RBody.gravityScale = GravityScale;
                //bJump = true;
                SpriteR.flipX = false;
                RBody.AddForce(new Vector2(1 * JumpStr, 270 * JumpStr));
                Debug.Log("WallJump!");
            }
            if (Right)
            {
                bFall = false;
                RBody.velocity = new Vector3(0, 0, 0);
                //RBody.gravityScale = GravityScale;
                //bJump = true;
                SpriteR.flipX = true;
                RBody.AddForce(new Vector2(-1 * JumpStr, 270 * JumpStr));
                //Debug.Log("WallJump");
            }
        }
        if (bDead || bWin)
        {
            RBody.gravityScale = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Muerte")
        {
            //Debug.Log("Palmas");
            bDead = true;
            RBody.velocity = new Vector3(0, 0, 0);
            RBody.gravityScale = 0;
            Anim.SetTrigger("Die");


        }
        if (other.tag == "Exit")
        {
            //Debug.Log("Palmas");
            bWin = true;
            RBody.velocity = new Vector3(0, 0, 0);
            RBody.gravityScale = 0;
            Anim.SetTrigger("Win");
        }
    }
    public void RestartScene() {
        SceneManager.LoadScene(0);

    }
    public void Spawn()
    {
        bSpawn = false;

    }
    public void Chocar(int side, bool bA, GameObject Col) {
        if (side == 1) {
            Bot = bA;
            BotCollider = Col;
            
        }
        if (side == 2)
        {
            Left = bA;
            SideCollider = Col;
            if (bA)
            {
                RBody.velocity = new Vector3(0, RBody.velocity.y, 0);
            }
        }
        if (side == 3)
        {
            Top = bA;
           
        }
        if (side == 4)
        {
            Right = bA;
            SideCollider = Col;
            if (bA)
            {
                RBody.velocity = new Vector3(0, RBody.velocity.y, 0);
            }
        }

    }
    public void SetCheckpoint(Vector2 v) {
        PlayerPrefs.SetFloat("SavedX", v.x);
        PlayerPrefs.SetFloat("SavedY", v.y);
    }
}
