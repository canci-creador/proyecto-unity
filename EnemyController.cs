using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float dectetionRadius = 5.0f;
    public float speed = 2.0f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // esto es para que decir que si detecta el jugador unicamente al jugador con el tag player, se mueve haci el jugador 
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if  (distanceToPlayer <  dectetionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            movement = new Vector2(direction.x, 0);
        }
        else
        {
            movement = Vector2.zero; // osea vector 2 es X,Y osea que basicamente si no lo detecta no se mueve
        }

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime); // esto es para que el enemigo se mueva
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))// verificar si el objeto con el que colisionamos es el jugador
        {
            Vector2 direccionDanio = (collision.transform.position - transform.position).normalized;
            //Vector2 direccionDanio esta es la creacio de la variable, ya que es la posiscion del enemigo en x pues en y es cero porque solo se va a mover de derecha a izquierda
            collision.gameObject.GetComponent<MovimientoJugador>().RecibeDanio(direccionDanio, 1);
        }
    }
    //COMANDO PARA VER EL RANGO DE DETECCION DEL ANEMIGO

    private void OnDrawGizmos()
     {
       Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position, dectetionRadius);
    }
}
