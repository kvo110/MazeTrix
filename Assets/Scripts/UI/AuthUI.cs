using TMPro;
using UnityEngine;

public class AuthUI : MonoBehaviour
{
  [Header("Inputs")]
  public TMP_InputField emailInput;
  public TMP_InputField passwordInput;

  [Header("Output")]
  public TMP_Text statusText;

  [Header("Managers")]
  public AuthManager authManager;

  private void Start()
  {
    if (statusText != null)
    {
      statusText.text = "Ready. Enter email + password.";
    }
  }

  public void OnClickSignUp()
  {
    if (authManager == null)
    {
      SetStatus("AuthManager not assigned in Inspector.");
      return;
    }

    var email = emailInput != null ? emailInput.text.Trim() : "";
    var pass = passwordInput != null ? passwordInput.text : "";

    authManager.SignUp(email, pass, SetStatus);
  }

  public void OnClickLogin()
  {
    if (authManager == null)
    {
      SetStatus("AuthManager not assigned in Inspector.");
      return;
    }

    var email = emailInput != null ? emailInput.text.Trim() : "";
    var pass = passwordInput != null ? passwordInput.text : "";

    authManager.Login(email, pass, SetStatus);
  }

  public void OnClickLogout()
  {
    if (authManager == null)
    {
      SetStatus("AuthManager not assigned in Inspector.");
      return;
    }

    authManager.Logout(SetStatus);
  }

  private void SetStatus(string msg)
  {
    Debug.Log(msg);
    if (statusText != null)
    {
      statusText.text = msg;
    }
  }
}
