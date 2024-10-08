using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMenuUI : MonoBehaviour
{
 private Animator animator;

    private void Start()
    {
        // Obtén el componente Animator del botón
        animator = GetComponent<Animator>();
    }

    public void OnButtonPress()
    {
        // Activa la transición a la animación de presionado
        animator.SetTrigger("PressButton");
    }
}