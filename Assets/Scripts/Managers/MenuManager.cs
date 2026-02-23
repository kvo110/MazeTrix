using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject[] menus; //will hold references to all menu GameObjects
    public GameObject initalMenu; //the menu that should be active at the start

    void Start()
    {
        initalMenu.SetActive(true); // Activate the initial menu
        foreach (GameObject menu in menus)
        {
            if (menu != initalMenu)
            {
                menu.SetActive(false); // Deactivate all other menus
            }
        }
    }


    public void OpenMenu(GameObject openThisMenu)
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(menu == openThisMenu); // Activate only the selected menu
        }
    }
}
