using UnityEngine;
using UnityEngine.UI;

public class MoveScriptPanel : MonoBehaviour
{
    private bool touchStart = false;    // Indica si el joystick está activo
    private Vector2 pointA;             // Centro del joystick (posición del círculo exterior)
    private Vector2 pointB;             // Posición actual del toque en coordenadas locales

    public Player player;               // Referencia al script del jugador
    public RectTransform circle;        // Círculo interior (joystick)
    public RectTransform outterCircle;  // Círculo exterior (límite del joystick)
    public Canvas canvas;               // El canvas donde está el joystick, para convertir correctamente las coordenadas
    public float joystickMaxRange = 100f; // Radio máximo del joystick en píxeles

    private int joystickTouchId = -1;   // Para rastrear el toque/clic que controla el joystick

    void Start()
    {
        // Fijar la posición inicial del círculo exterior (outterCircle)
        pointA = outterCircle.anchoredPosition;
    }

    void Update()
    {
        // Si se detecta un clic o toque nuevo
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // Verificar si el toque/clic está dentro del área del círculo exterior y que el joystick no esté en uso
            if (RectTransformUtility.RectangleContainsScreenPoint(outterCircle, Input.mousePosition, canvas.worldCamera) && joystickTouchId == -1)
            {
                // Activar el control del joystick y registrar el ID del toque o clic
                touchStart = true;
                joystickTouchId = Input.touchCount > 0 ? Input.GetTouch(0).fingerId : 0;
            }
        }

        // Actualizar el movimiento mientras el toque/clic está activo
        if (touchStart)
        {
            // Si el toque actual coincide con el que está controlando el joystick
            if (Input.touchCount > 0 && joystickTouchId == Input.GetTouch(0).fingerId)
            {
                // Convertir la posición del toque en coordenadas locales del canvas
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    outterCircle.parent.GetComponent<RectTransform>(),
                    Input.GetTouch(0).position,
                    canvas.worldCamera,
                    out pointB
                );
            }
            else if (Input.GetMouseButton(0) && joystickTouchId == 0)  // Caso para control con mouse
            {
                // Convertir la posición del ratón en coordenadas locales dentro del canvas
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    outterCircle.parent.GetComponent<RectTransform>(),
                    Input.mousePosition,
                    canvas.worldCamera,
                    out pointB
                );
            }
        }

        // Si el toque/clic ha finalizado
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            // Si el toque que terminó es el que controla el joystick
            if (Input.touchCount > 0 && Input.GetTouch(0).fingerId == joystickTouchId)
            {
                touchStart = false;
                joystickTouchId = -1;  // Resetear el ID de toque
            }
            else if (joystickTouchId == 0 && Input.GetMouseButtonUp(0))  // Caso para mouse
            {
                touchStart = false;
                joystickTouchId = -1;
            }
        }
    }

    void FixedUpdate()
    {
        if (touchStart)
        {
            // Calcular la diferencia entre las posiciones inicial y final
            Vector2 offset = pointB - pointA;

            // Limitar el vector de movimiento para que no salga del círculo exterior
            Vector2 direction = Vector3.ClampMagnitude(offset, joystickMaxRange);

            // Aplicar el movimiento normalizado al jugador
            Vector2 normalizedDirection = direction / joystickMaxRange;
            player.UpdateMotor(new Vector3(normalizedDirection.x, normalizedDirection.y, 0));  // Pasar el movimiento en el eje X e Y

            // Mover el círculo interior (joystick) dentro del área limitada por el círculo exterior
            circle.anchoredPosition = pointA + direction;
        }
        else
        {
            // Si no hay toque, parar el movimiento y restaurar el círculo interior al centro del círculo exterior
            player.UpdateMotor(Vector3.zero);
            circle.anchoredPosition = pointA;
        }
    }
}
