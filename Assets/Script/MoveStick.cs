using UnityEngine;

public class MoveStick : MonoBehaviour
{
    private bool touchStart = false;
    private Vector3 pointA;
    private Vector3 pointB;

    public Player player;  // Referencia al script del jugador donde se mueve.

    public Transform circle;
    public Transform outterCircle;

    void Update()
    {
        // Detectar cuando se inicia el toque o clic
        if(Input.GetMouseButtonDown(0))
        {
            pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            circle.transform.position = pointA * -1;
            outterCircle.transform.position = pointA * -1;
            circle.GetComponent<SpriteRenderer>().enabled = true;
            outterCircle.GetComponent<SpriteRenderer>().enabled = true;
        }

        // Actualizar la posición mientras el toque o clic está activo
        if(Input.GetMouseButton(0))
        {
            touchStart = true;
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        }
        else
        {
            touchStart = false;
        }
    }

    void FixedUpdate()
    {
        if(touchStart)
        {
            Debug.Log("mover");
            
            // Calcular la diferencia entre las posiciones inicial y final
            Vector3 offset = pointB - pointA;

            // Normalizar el vector para limitar la dirección y evitar un desplazamiento excesivo
            Vector3 direction = Vector3.ClampMagnitude(new Vector3(offset.x, offset.y, 0), 1.0f);

            // Llamar a UpdateMotor para mover al jugador
            player.UpdateMotor(direction);
            circle.transform.position = new Vector3(pointA.x + direction.x, pointA.y + direction.y, 0) * -1;
        }
        else
        {
            // Si no hay toque, parar el movimiento
            player.UpdateMotor(Vector3.zero);
            circle.GetComponent<SpriteRenderer>().enabled = false;
            outterCircle.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
