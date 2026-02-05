using System;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;

public class AuthManager : MonoBehaviour
{
  [Header("UI References (drag these in the Inspector)")]
  [SerializeField] private TMP_InputField emailInput;
  [SerializeField] private TMP_InputField passwordInput;
  [SerializeField] private TMP_Text statusText;

  private FirebaseAuth auth;

  private void Awake()
  {
    // Delay FirebaseAuth creation until FirebaseBootstrap is ready
    auth = null;
  }

  // Called by the Sign Up button
  public void OnSignUpPressed()
  {
    string email = GetText(emailInput);
    string password = GetText(passwordInput);

    if (!ValidateInputs(email, password)) return;

    SetStatus("Creating account...");

    SignUp(email, password, message =>
    {
      SetStatus(message);
    });
  }

  // Called by the Login button
  public void OnLoginPressed()
  {
    string email = GetText(emailInput);
    string password = GetText(passwordInput);

    if (!ValidateInputs(email, password)) return;

    SetStatus("Logging in...");

    Login(email, password, message =>
    {
      SetStatus(message);
    });
  }

  // Optional logout button
  public void OnLogoutPressed()
  {
    Logout(message =>
    {
      SetStatus(message);
    });
  }

  // Core signup logic
  public void SignUp(string email, string password, Action<string> onResult)
  {
    if (!EnsureAuth(onResult)) return;

    auth.CreateUserWithEmailAndPasswordAsync(email, password)
      .ContinueWithOnMainThread(task =>
      {
        if (task.IsCanceled)
        {
          onResult?.Invoke("Sign up canceled.");
          return;
        }

        if (task.IsFaulted)
        {
          onResult?.Invoke(task.Exception.GetBaseException().Message);
          return;
        }

        onResult?.Invoke("Sign up successful.");
      });
  }

  // Core login logic
  public void Login(string email, string password, Action<string> onResult)
  {
    if (!EnsureAuth(onResult)) return;

    auth.SignInWithEmailAndPasswordAsync(email, password)
      .ContinueWithOnMainThread(task =>
      {
        if (task.IsCanceled)
        {
          onResult?.Invoke("Login canceled.");
          return;
        }

        if (task.IsFaulted)
        {
          onResult?.Invoke(task.Exception.GetBaseException().Message);
          return;
        }

        onResult?.Invoke("Login successful.");
      });
  }

  // Logout logic
  public void Logout(Action<string> onResult)
  {
    if (auth != null)
    {
      auth.SignOut();
    }

    onResult?.Invoke("Logged out.");
  }

  // Makes sure Firebase and Auth are ready
  private bool EnsureAuth(Action<string> onResult)
  {
    if (!FirebaseBootstrap.Ready)
    {
      onResult?.Invoke("Firebase not ready yet. Try again in a moment.");
      return false;
    }

    if (auth == null)
    {
      auth = FirebaseAuth.DefaultInstance;
    }

    return true;
  }

  // Simple input validation
  private bool ValidateInputs(string email, string password)
  {
    if (string.IsNullOrWhiteSpace(email))
    {
      SetStatus("Please enter an email.");
      return false;
    }

    if (string.IsNullOrWhiteSpace(password))
    {
      SetStatus("Please enter a password.");
      return false;
    }

    if (password.Length < 6)
    {
      SetStatus("Password must be at least 6 characters.");
      return false;
    }

    return true;
  }

  // Helper to safely get text from input fields
  private string GetText(TMP_InputField field)
  {
    return field == null ? "" : field.text.Trim();
  }

  // Update UI and log for debugging
  private void SetStatus(string message)
  {
    Debug.Log(message);

    if (statusText != null)
    {
      statusText.text = message;
    }
  }
}
