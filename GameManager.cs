using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject PainelCompleto;
    public bool isPause = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    
    public void Pause(){

        //Adicionando funcionalidade ao botão pause
        if (isPause){
            //Se estiver pausado eu desativo o pause
            PainelCompleto.SetActive(false);
            isPause = false;
        }
        else{
            //Se não estive pausado eu ativo o pause
            PainelCompleto.SetActive(true);
            isPause = true;
        }

    }
}
