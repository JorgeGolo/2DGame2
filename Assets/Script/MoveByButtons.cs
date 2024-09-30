using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveByButtons : MonoBehaviour
{

    public Button buttonUp;
    public Button buttonDown;
    public Button buttonRight;
    public Button buttonLeft;

    private bool moveUp = false;
    private bool moveDown = false;
    private bool moveRight = false;
    private bool moveLeft = false;

    public Player player; // Referencia al script donde está UpdateMotor



    // Start is called before the first frame update
     void Start()
    {
        // Asignar funciones a los eventos de los botones
        AssignButtonEvents(buttonUp, "Up");
        AssignButtonEvents(buttonDown, "Down");
        AssignButtonEvents(buttonRight, "Right");
        AssignButtonEvents(buttonLeft, "Left");
        ResetButtonStates();
    
    }

    public void ResetButtonStates()
    {
        Debug.Log("Resetear botones");
        // Asegúrate de que los botones no están en un estado presionado
        moveUp = false;
        moveDown = false;
        moveRight = false;
        moveLeft = false;
        
        // Aquí puedes agregar código adicional para cambiar el estado visual de los botones si es necesario.
    }
    void OnEnable()
    {
        ResetButtonStates(); // Resetea los estados cuando el objeto se habilita
    }

    void Update()
    {
        // Inicializamos x e y en 0
        float x = 0f;
        float y = 0f;

        // Verificamos qué botones están presionados para modificar x e y
        if (moveUp) y = 1;
        if (moveDown) y = -1;
        if (moveRight) x = 1;
        if (moveLeft) x = -1;

        // Pasamos el vector de movimiento a UpdateMotor
        if (player.isAlive)
        {
            player.UpdateMotor(new Vector3(x, y, 0));
        }
    }

    // Método para asignar eventos a los botones
    void AssignButtonEvents(Button button, string direction)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((data) => { OnButtonPress(direction); });
        trigger.triggers.Add(entryDown);

        EventTrigger.Entry entryUp = new EventTrigger.Entry();
        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener((data) => { OnButtonRelease(direction); });
        trigger.triggers.Add(entryUp);
    }    
    void OnButtonPress(string direction)
    {
        // Activamos o desactivamos el movimiento según la dirección
        switch (direction)
        {
            case "Up":
                moveUp = true;
                break;
            case "Down":
                moveDown = true;
                break;
            case "Right":
                moveRight = true;
                break;
            case "Left":
                moveLeft = true;
                break;
        }
    }

    public void OnButtonRelease(string direction)
    {
        // Desactivar el movimiento cuando se suelta el botón
        switch (direction)
        {
            case "Up":
                moveUp = false;
                break;
            case "Down":
                moveDown = false;
                break;
            case "Right":
                moveRight = false;
                break;
            case "Left":
                moveLeft = false;
                break;
        }
    }

}
