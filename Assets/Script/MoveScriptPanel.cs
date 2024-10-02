using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveScriptPanel : MonoBehaviour
{
    private bool touchStart = false;
    private Vector2 pointA;  // Centro del joystick (posición del círculo exterior)
    private Vector2 pointB;  // Posición del ratón en coordenadas locales

    public Player player;  // Referencia al script del jugador

    public RectTransform circle;        // Círculo interior (joystick)
    public RectTransform outterCircle;  // Círculo exterior (límite del joystick)

    public Canvas canvas;  // El canvas donde está el joystick, para convertir correctamente las coordenadas
    public float joystickMaxRange = 100f;  // Radio máximo del joystick en píxeles

    // movement bug with attack button
    public Button attackButton;  // Referencia al botón de ataque

    void Start()
    {
        // Fijar la posición inicial del círculo exterior (outterCircle)
        // El centro del joystick está en la posición del círculo exterior
        pointA = outterCircle.anchoredPosition;  // Guardamos la posición inicial del círculo exterior


    }
    // Función que comprueba si el toque o clic está sobre el botón de ataque
    private bool IsPointerOverAttackButton()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject == attackButton.gameObject)
            {
                return true;
            }
        }

        return false;
    }
    void Update()
    {
         // Detectar cuando se inicia el toque o clic
        if (Input.GetMouseButtonDown(0) && !IsPointerOverAttackButton())
        {
            // Verificar si el clic está dentro del área del círculo exterior
            if (RectTransformUtility.RectangleContainsScreenPoint(outterCircle, Input.mousePosition, canvas.worldCamera))
            {
                touchStart = true;
            }
        }

        // Actualizar la posición mientras el toque o clic está activo
        if (touchStart && Input.GetMouseButton(0))
        {
            // Convertir la posición del ratón a coordenadas locales dentro del canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                outterCircle.parent.GetComponent<RectTransform>(),
                Input.mousePosition,
                canvas.worldCamera,
                out pointB);
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
            Vector2 offset = pointB - pointA;

            // Limitar el vector de movimiento para que no salga del círculo exterior
            Vector2 direction = Vector3.ClampMagnitude(offset, joystickMaxRange);

            // Aplicar el movimiento normalizado al jugador
            Vector2 normalizedDirection = direction / joystickMaxRange;
            player.UpdateMotor(new Vector3(normalizedDirection.x, normalizedDirection.y, 0 ));  // Pasar el movimiento en el eje X e Y

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
