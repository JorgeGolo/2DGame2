using UnityEngine;
using UnityEngine.UI;

public class MoveStick : MonoBehaviour
{
    public Sprite MoveStickLeftSprite;
    public Sprite MoveStickRightSprite;
    public Sprite MoveStickUpSprite;
    public Sprite MoveStickDownSprite;
    public Sprite MoveStickIdleSprite; // Sprite en reposo

    public Image moveStickImage; // La imagen del stick

    private Vector2 inputVector;

    void Start()
    {
        // Inicializamos el stick en el sprite en reposo
        moveStickImage.sprite = MoveStickIdleSprite;
        inputVector = Vector2.zero;
    }

    public void OnDrag(Vector2 direction)
    {
        // Calculamos la dirección de arrastre
        inputVector = direction.normalized; // Normalizamos el vector para obtener solo la dirección
        UpdateStickSprite();
    }

    void UpdateStickSprite()
    {
        // Cambiar el sprite según la dirección del inputVector
        if (inputVector.x < 0)
        {
            Debug.Log(inputVector.x);
            moveStickImage.sprite = MoveStickLeftSprite; // Mover a la izquierda
        }
        else if (inputVector.x > 0)
        { Debug.Log(inputVector.x);
            moveStickImage.sprite = MoveStickRightSprite; // Mover a la derecha
        }
        else if (inputVector.y > 0)
        { Debug.Log(inputVector.y);
            moveStickImage.sprite = MoveStickUpSprite; // Mover hacia arriba
        }
        else if (inputVector.y < 0)
        { Debug.Log(inputVector.x);
            moveStickImage.sprite = MoveStickDownSprite; // Mover hacia abajo
        }
        else
        {
            moveStickImage.sprite = MoveStickIdleSprite; // Si no hay movimiento, sprite en reposo
        }
    }

    public void ResetStick()
    {
        // Volvemos el sprite del stick a su estado de reposo
        moveStickImage.sprite = MoveStickIdleSprite;
    }
}
