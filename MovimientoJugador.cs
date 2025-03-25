using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;
    public float fuerzaRebote = 10f;
    public float longitudRaycast = 0.1f; // es lo largo de la raya roja que detecta la colision con el suelo
    public LayerMask capaSuelo;

    private bool enSuelo;
    private bool recibiendoDano;
    private Rigidbody2D rb;
    private int saltosActuales;
    private int maxSaltos = 2;
    public Animator animator;
    private bool estabaEnSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float VelocidadX = Input.GetAxis("Horizontal") * velocidad;//movimiento del jugador 
        animator.SetFloat("movement", Mathf.Abs(VelocidadX));//activacion de la animacion de moverse

        // Voltear sprite osea el jugador según dirección
        if (VelocidadX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (VelocidadX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (!recibiendoDano) // se pone el if para poder hacer que cuando reciba daño no se pueda mover(esete no es el error porque ya lo quite y sigue igual)
        transform.position += new Vector3(VelocidadX * Time.deltaTime, 0, 0);
        
        // Guardamos el estado anterior del suelo
        estabaEnSuelo = enSuelo;

        // Raycast para detectear la colision con el suelo 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        enSuelo = hit.collider != null;

        // Si toca el suelo y antes no lo estaba, reiniciar los saltos
        if (enSuelo && !estabaEnSuelo)
        {
            saltosActuales = 0;
        }

        // Salto normal y doble salto
        if (Input.GetKeyDown(KeyCode.W) && saltosActuales < maxSaltos && !recibiendoDano) //almomento de querer saltar no debe recibi daño por eso la condicion && !recibiendoDano
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f); // Resetear velocidad vertical
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse); // Aplicar salto
            saltosActuales++;
            
        }
        // Activar animación de salto
        animator.SetBool("enSuelo", enSuelo);
        animator.SetBool("recibeDanio", recibiendoDano);
    }
    //funcion recibe daño fuera del udate porque unicamente se va ejecutar cuando el enemigo choque con el personaje
    public void RecibeDanio(Vector2 direccion, int cantDanio) //Vector2 dieccion es la dirrecion donde rrecibe el daño, int cantDanio cuanto daño le causa el enemigo
    {
        if (!recibiendoDano)
        {
            recibiendoDano = true; // porque ya estoy diciendo que esta public va a hacer que reciba daño
            Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;// esto es la fuerza de rebote y la dirrecion
            rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);// fomula de rebote

            
        }
    }
    
    public void DesactivoDanio() // esta funcion se hace para que el jugador no resiva daño infiinito ya que esta cancela a la otra por decir lo asi
    {
        // la funcion de public void DesactivoDanio, no se activa aca se activa en el animator se hizo de la simanera
        //voy a la petaña de animation teniendo seleccionado el player en gameObject, busco mi animacion de daño marco el ultimo frame, lugo voy a la opcion de add envent 
        //que esta debajo del 1 que parece y poner le cursor sobre una de las dos y selecciona la qque diga add event y miramos el inspector en funcion va salir la opcion de 
        //desactivar daño
        recibiendoDano = false;
    }
    void OnDrawGizmos() //esto es para detectar el suelo con una linea rojo que invoca en el unity, y es alinea tiene colision con el suelo 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}
