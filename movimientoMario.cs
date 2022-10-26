using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movimientoMario : MonoBehaviour
{
    public float x=1.5f;
    public float y=1.5f;
    public float salto;
    public float marcador;
    public static int puntos=0;
    public int vidas=3;
    Animator anim;
    public GameObject corazon1,corazon2,corazon3; 
    Rigidbody2D rb;
    BoxCollider2D coll;
    public LayerMask piso;
    // Start is called before the first frame update
    void Start()
    {
       anim=GetComponent<Animator>(); 
       rb=GetComponent<Rigidbody2D>();
       coll=GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.D)){
                if(GetComponent<SpriteRenderer>().flipX==true){
                    GetComponent<SpriteRenderer>().flipX=false;
                }
            transform.Translate(new Vector2(x*(Time.deltaTime*10),0));
            anim.SetBool("correr",true);
            anim.SetBool("idle",false);
            anim.SetBool("brincar",false);
        }

        if(Input.GetKey(KeyCode.A)){
                if(GetComponent<SpriteRenderer>().flipX==false){
                    GetComponent<SpriteRenderer>().flipX=true;
                }
            transform.Translate(new Vector2(-x*(Time.deltaTime*10),0));
            anim.SetBool("correr",true);
            anim.SetBool("idle",false);
            anim.SetBool("brincar",false);
        }

        if(Input.GetKey(KeyCode.W) && CompruebaPiso()){
            //transform.Translate(new Vector2(0,y*(Time.deltaTime*10)));
            rb.AddForce(Vector2.up*0.7f,ForceMode2D.Impulse);
            anim.SetBool("correr",false);
            anim.SetBool("idle",false);
            anim.SetBool("brincar",true);
        }

        if(Input.anyKey!=true){
            anim.SetBool("correr",false);
            anim.SetBool("idle",true);
            anim.SetBool("brincar",false); 
        }

        if(transform.position.y<= -7.56f){
            if(PlayerPrefs.GetInt("checkpoint")==1){
                this.transform.position=new Vector2(18.66f,0.52f);
            }
            //SceneManager.LoadScene("GameOver");
        }


    }

    public void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag=="moneda"){
            puntos=puntos+1;
            PlayerPrefs.SetInt("puntaje",puntos);
            if(puntos==10){
                SceneManager.LoadScene("Nivel2");
            }
            Destroy(col.gameObject,0.2f);
        }

        if(col.gameObject.tag=="enemigo"){
            vidas=vidas-1;
            if(vidas==2){
                corazon1.SetActive(true);
                corazon2.SetActive(true);
                corazon3.SetActive(false);
            }

            if(vidas==1){
                corazon1.SetActive(true);
                corazon2.SetActive(false);
                corazon3.SetActive(false);
            }

            PlayerPrefs.SetInt("vidas",vidas);
            if(vidas<=0){
                SceneManager.LoadScene("GameOver");
            }
        }


        if(col.gameObject.tag=="checkpoint"){
            PlayerPrefs.SetInt("checkpoint",1);
            Destroy(col.gameObject);
        }

    }

    public void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag=="zona1"){
            //SceneManager.LoadScene("GameOver");
        }
    }

    public bool CompruebaPiso(){
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f,Vector2.down, 0.1f, piso);
    }
}
