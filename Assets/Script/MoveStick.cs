using UnityEngine;

public class MoveStick : MonoBehaviour
{
    private bool touchStart = false;
    private Vector3 pointA;  // Centro del joystick (posición del círculo exterior)
    private Vector3 pointB;  // Posición del ratón

    public Player player;  // Referencia al script del jugador

    public Transform circle;        // Círculo interior (joystick)
    public Transform outterCircle;  // Círculo exterior (límite del joystick)

    public float joystickMaxRange = 1.0f;  // Radio máximo del joystick

    void Start()
    {
        // Fijar la posición inicial del círculo exterior (outterCircle)
        // Puedes ajustar esta posición para colocar el joystick en cualquier lugar de la pantalla
        pointA = outterCircle.position;  // El centro del joystick está en la posición del círculo exterior
    }

    void Update()
    {
        // Detectar cuando se inicia el toque o clic
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = true;
        }

        // Actualizar la posición mientras el toque o clic está activo
        if (Input.GetMouseButton(0))
        {
            // Obtener la posición del ratón en el mundo
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            pointB.z = 0;  // Asegurarse de que la coordenada Z sea cero
        }
        else
        {
            touchStart = false;
        }
    }

    void FixedUpdate()
    {
        if (touchStart)
        {
            // Calcular la diferencia entre las posiciones inicial y final
            Vector3 offset = pointB - pointA;

            // Limitar el vector de movimiento para que no salga del círculo exterior
            Vector3 direction = Vector3.ClampMagnitude(offset, joystickMaxRange);

            // Aplicar el movimiento normalizado al jugador
            Vector3 normalizedDirection = direction / joystickMaxRange;
            player.UpdateMotor(normalizedDirection);

            // Mover el círculo interior (joystick) dentro del área limitada por el círculo exterior
            circle.position = pointA + direction;
        }
        else
        {
            // Si no hay toque, parar el movimiento y ocultar el joystick
            player.UpdateMotor(Vector3.zero);
            circle.position = pointA;  // Restaurar el círculo interior al centro del círculo exterior
        }
    }
}
