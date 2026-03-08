using System;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;

public class AuthManager : MonoBehaviour
{
  [Header("UI References (drag these in the Inspector)")]
  [SerializeField] private TMP_InputField emailInput;
  [SerializeField] private TMP_InputField passwordInput;
  [SerializeField] private TMP_Text statusText;

  private FirebaseAuth auth;
  private FirebaseFirestore db;

  private void Awake()
  {
    // Firebase will be initialized by FirebaseBootstrap first
    auth = null;
    db = null;
  }

  // Called by Register button
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

  // Called by Login button
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

  // Create Firebase account
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

        // account created successfully
        string uid = auth.CurrentUser.UserId;

        CreateUserDocument(uid, email, result =>
        {
          onResult?.Invoke(result);
        });
      });
  }

  // Login existing user
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

  // Save player profile in Firestore
  private void CreateUserDocument(string uid, string email, Action<string> onResult)
  {
    if (db == null)
    {
      db = FirebaseFirestore.DefaultInstance;
    }

    var userData = new
    {
      email = email,
      createdAt = Timestamp.GetCurrentTimestamp(),
      bestScore = 0,
      highestLevel = 1,
      health = 100,
      hunger = 100
    };

    db.Collection("users").Document(uid).SetAsync(userData)
      .ContinueWithOnMainThread(task =>
      {
        if (task.IsCompleted)
        {
          onResult?.Invoke("Account created and saved to database.");
        }
        else
        {
          onResult?.Invoke("Account created but database write failed.");
        }
      });
  }

  // Logout
  public void Logout(Action<string> onResult)
  {
    if (auth != null)
    {
      auth.SignOut();
    }

    onResult?.Invoke("Logged out.");
  }

  // Make sure Firebase is ready
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

    if (db == null)
    {
      db = FirebaseFirestore.DefaultInstance;
    }

    return true;
  }

  // Check user inputs
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

  // safely read input field text
  private string GetText(TMP_InputField field)
  {
    return field == null ? "" : field.text.Trim();
  }

  // update UI and log
  private void SetStatus(string message)
  {
    Debug.Log(message);

    if (statusText != null)
    {
      statusText.text = message;
    }
  }
}
