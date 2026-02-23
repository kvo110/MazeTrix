using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.InputSystem;

public class TabSwitch : MonoBehaviour
{
    EventSystem system;
    public Selectable initialUIElement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        system = EventSystem.current;
        if (initialUIElement != null)
        {
            initialUIElement.Select(); // select the initial UI element
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            try
            {
                Selectable current = system.currentSelectedGameObject.GetComponent<Selectable>();
                if (current != null)
                {
                    Selectable next = current.FindSelectableOnDown();
                    if (next != null)
                    {
                        next.Select();
                    }
                    else
                    {
                        initialUIElement.Select();
                    }
                }
            }
            catch (System.NullReferenceException)
            {
                if (initialUIElement != null)
                {
                    initialUIElement.Select();
                }
            }

        }
    }
}
