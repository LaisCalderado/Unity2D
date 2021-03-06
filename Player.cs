﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour{

    public float forcaPulo;
    public float velocidadeMaxima;
    public int lives;
    public int rings;
    public Text TexteLives;
    public Text TexteRings;
    public bool noChao;
    public bool canFly;
    public bool inWater;
    public GameObject lastCheckPoint;

    // Use isto para inicialização
    void Start(){
        
        //Serve para acessar um componente
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200));

        TexteLives.text = lives.ToString();
        TexteRings.text = rings.ToString();

    }
    // Update is called once per frame
    void Update(){
        //Um variavel rigidbody na classe Rigidbody2D recebendo um comando
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        //Estou pegando os valores direcionais (setas do teclado) e colocou numa variavel (movimento)
        float movimento = Input.GetAxis("Horizontal");
        //Aplique uma velocidade ou rigidbody, na qual só foi aplicada no eixo x (a velocidade será multiplicada pelo movimento)
        rigidbody.velocity = new Vector2(movimento*velocidadeMaxima, rigidbody.velocity.y);

        //Se o movimento for menor que zero o boneco faz o flip (muda sua direção)
        if (movimento < 0)
        {
            //Pegando componente SpriteRenderer o obejto flip se ele for verdadeiro o boneco faz o flip
            GetComponent<SpriteRenderer>().flipX = true;
            
        } else if (movimento > 0)
        {
            //Pegando componente SpriteRenderer o obejto flip se ele for falso o boneco não faz o flip
            GetComponent<SpriteRenderer>().flipX = false;            
        }

        //Verificando se o boneco está andando para direita ou para a esqueda
        if(movimento > 0 || movimento < 0){
            //Se estive faz a animação de movimento
            GetComponent<Animator>().SetBool("walking", true);
        }else{
            //Se não estive (ou seja se ele estiver parado) fica parado
            GetComponent<Animator>().SetBool("walking", false);
        }

        //Ele só pode fazer essas coisa se ele NÃO estive na água
        if (!inWater){

            //se o comando espaco for pressionado
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(noChao){
                    //O boneco receberá uma força na vertical, ou seja ele pulará
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0,forcaPulo));
                    //Adicionando o som de pulo
                    GetComponent<AudioSource>().Play();
                    //Não pode voar
                    canFly = false;

                }else canFly = true; //Se não ele pode voar 
            }

            //Se estiver no chão ou apertando o botão de espaço
            if(canFly && Input.GetKey(KeyCode.Space) ){
                //Ele pode voar
                GetComponent<Animator>().SetBool("flyinng", true);//Flying é o parametro
                //Adicionando uma velocidade para ele "planar"
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, -0.5f);
            }
            //Se não estiver nem no chão e nem apertando espaço ele não pode voar
            else GetComponent<Animator>().SetBool("flyinng", false); 


            //Se o boneco estive no chão
            if(noChao){
                //Ele pode pular
                GetComponent<Animator>().SetBool("jumping", false);
            }else{ 
                //Se não, ele não pode voar
                GetComponent<Animator>().SetBool("jumping", true);
            }
        }else{
            //Se ele estive na agua e apertar para cima
            if (Input.GetKey(KeyCode.UpArrow)){
                //Adiciona uma força 
                rigidbody.AddForce(new Vector2(0, 6f * Time.deltaTime), ForceMode2D.Impulse);
            }
            //Se ele apertar para baixo (na agua), aciona uma força inversa
            if (Input.GetKey(KeyCode.DownArrow)){

                rigidbody.AddForce(new Vector2(0, -6f * Time.deltaTime), ForceMode2D.Impulse);
            }
            //Mas a todo tempo acionando uma for no player
            rigidbody.AddForce(new Vector2(0, 10f * Time.deltaTime), ForceMode2D.Impulse);
        }
        //Chamando a animação de nadar
        GetComponent<Animator>().SetBool("swimming", inWater);

        //Verifica se apertou a tecla ctrl
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            //Faz a animação
            GetComponent<Animator>().SetTrigger("hammer");
            Collider2D[] colliders = new Collider2D[3];
            //Pega o collider do HammerArea
            transform.Find("HammerArea").gameObject.GetComponent<Collider2D>()
                //Retornar todos os objetos que estão em colisao com o HammerArea e coloca na lista colliders
                .OverlapCollider(new ContactFilter2D(), colliders);
            //Faz a verificação e
            for(int i = 0; i < colliders.Length; i++){
                //Pegando cada collider e verificando se a tag é Monstros
                if(colliders[i] != null && colliders[i].gameObject.CompareTag("Monstros")){
                    //Se for destroi o monstro
                    Destroy(colliders[i].gameObject);
                }
            }
        }
    }     

    //Verificando se ele está ou não na "água"
    void OnTriggerEnter2D(Collider2D collision2D) {
        
        //Se o personagem colidir com a tag Water é pq está 
        if(collision2D.gameObject.CompareTag("Water")){
            inWater = true;
        }

        //Se o personagem colidir com uma moeda
        if(collision2D.gameObject.CompareTag("Moedas")){
            Destroy(collision2D.gameObject);//Assim que pegar as moedas elas serão destroidas
            rings++; //Contador de moedas
            TexteRings.text = rings.ToString();//Irá aumentar o numero na ilustração
        }

        //Verificar se o personagem colidiu com o CheckPonti
        if(collision2D.gameObject.CompareTag("CheckPoint")){
            //Saber qual foi o ultimo checkpoint q o personagem encostou
            lastCheckPoint = collision2D.gameObject;

        }     
    }

    void OnTriggerExit2D(Collider2D collision2D) {

        //Se o personagem parar de colidir com a tag Water é pq não está 
        if(collision2D.gameObject.CompareTag("Water")){
            inWater = false;
        }
    }


    //Verifica se está colidindo com algo
    void OnCollisionEnter2D(Collision2D collision2D) {
        //Se o persogaem colidiu com um monstro
        if(collision2D.gameObject.CompareTag("Monstros")){
            lives--;//Perde uma vida
            TexteLives.text = lives.ToString();//Muda a imagem de vida no game
            if(lives == 0){//Se o numero de vidas for igual a zero
                //O Personagem volta para o ultimo checkpoint que ele fez
                transform.position = lastCheckPoint.transform.position;
            }
        }

        //Se tive colidindo com alguma coisa então ele está no chão
        if(collision2D.gameObject.CompareTag("Plataformas")){

            noChao = true;
        }

        //Se o personagem colidir com a tag trampolim
        if(collision2D.gameObject.CompareTag("Trampolim")){
            //Ele receberá uma força na vertical, fazendo ele ter impulso
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 5f);
            
        }        
    }   

    
    //Verifica se parou de colidir
    void OnCollisionExit2D(Collision2D collision2D) {
    
       // Se não estier colidinfo com nada, é porque não está no chão
        if(collision2D.gameObject.CompareTag("Plataformas")){

            noChao = false;
        }
    } 

}
