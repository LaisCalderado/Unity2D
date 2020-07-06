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
            //O boneco receberá uma força na vertical, ou seja ele pulará
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0,forcaPulo));
            //Adicionando o som de pulo
            GetComponent<AudioSource>().Play();
        }
        
    }     

    void OnCollisionEnter2D(Collision2D other) {

        Debug.Log("Colidiu");
        
    }   

    void OnCollisionExit2D(Collision2D other) {
        
        Debug.Log("Parou de colidir");
    } 

}
