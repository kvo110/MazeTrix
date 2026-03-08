using UnityEngine;

public class MenuManager : MonoBehaviour
{
  [Header("Screen References")]
  [SerializeField] private GameObject welcomeScreen;
  [SerializeField] private GameObject registerScreen;
  [SerializeField] private GameObject loginScreen;

  private void Start()
  {
    // Start on the welcome screen first
    ShowWelcomeScreen();
  }

  public void ShowWelcomeScreen()
  {
    if (welcomeScreen != null)
    {
      welcomeScreen.SetActive(true);
    }

    if (registerScreen != null)
    {
      registerScreen.SetActive(false);
    }

    if (loginScreen != null)
    {
      loginScreen.SetActive(false);
    }
  }

  public void ShowRegisterScreen()
  {
    if (welcomeScreen != null)
    {
      welcomeScreen.SetActive(false);
    }

    if (registerScreen != null)
    {
      registerScreen.SetActive(true);
    }

    if (loginScreen != null)
    {
      loginScreen.SetActive(false);
    }
  }

  public void ShowLoginScreen()
  {
    if (welcomeScreen != null)
    {
      welcomeScreen.SetActive(false);
    }

    if (registerScreen != null)
    {
      registerScreen.SetActive(false);
    }

    if (loginScreen != null)
    {
      loginScreen.SetActive(true);
    }
  }

  public void QuitGame()
  {
    // This works in a built game
    Application.Quit();

    // This message helps while testing in the editor
    Debug.Log("Quit button pressed.");
  }
}
