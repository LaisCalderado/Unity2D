using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InimigoHorizontal : MonoBehaviour{


    //Variavel de controle (para ver se colidiu ou não)
    public bool colidde = false;
    //Variavel de velocidade
    public float move = -2;


    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
        //Pega a velocidade do objeto pelo RigidBody 
        // E a velocidade vai ser (-2) logo ele começa andando para a direita, e não tem velocidade vertical
        GetComponent<Rigidbody2D>().velocity = new Vector2(move, GetComponent<Rigidbody2D>().velocity.y);
        //Se o obejto colidir ele flipa (vira)
        if (colidde){
            
            Flip();
        }    
    }

    void Flip(){

        move *= -1;//A variavel para inverter os lado (ex se for -2 * -1 = 2 vira para a direita, 2 * -1 = -2) vira para a esquerda
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        colidde = false;
        //Debu.Log(GetComponent<SpriteRenderer>());
    }

    void OnCollisionEnter2D(Collision2D collision2D) {
        //Se o objeto colidir com a Tag plataforma ele vira
        if(collision2D.gameObject.CompareTag("Plataformas")) colidde = true;
    }

    void OnCollisionExit2D(Collision2D collision2D) {
    //Se ele parar de colidir com a Tag ele não vira
        if(collision2D.gameObject.CompareTag("Plataformas")) colidde = false;
    }
    
}
