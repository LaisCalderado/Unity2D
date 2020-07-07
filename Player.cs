using System.Collections;
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
        }
        
        else{ 
            //Se não, ele não pode voar
            GetComponent<Animator>().SetBool("jumping", true);
        }
    }     
    //Verifica se está colidindo com algo
    void OnCollisionEnter2D(Collision2D collision2D) {

        //Se tive colidindo com alguma coisa então ele está no chão
        if(collision2D.gameObject.CompareTag("Plataformas")){

            noChao = true;
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
