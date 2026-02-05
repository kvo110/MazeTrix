using System;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;

public class AuthManager : MonoBehaviour
{
    private FirebaseAuth auth;

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void SignUp(string email, string password, Action<string> onResult)
    {
        if (!FirebaseBootstrap.Ready)
        {
            onResult?.Invoke("Firebase not ready yet. Try again in a second.");
            return;
        }

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
                    onResult?.Invoke(task.Exception?.GetBaseException().Message);
                    return;
                }

                onResult?.Invoke("Sign up successful.");
            });
    }

    public void Login(string email, string password, Action<string> onResult)
    {
        if (!FirebaseBootstrap.Ready)
        {
            onResult?.Invoke("Firebase not ready yet. Try again in a second.");
            return;
        }

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
                    onResult?.Invoke(task.Exception?.GetBaseException().Message);
                    return;
                }

                onResult?.Invoke("Login successful.");
            });
    }

    public void Logout(Action<string> onResult)
    {
        auth.SignOut();
        onResult?.Invoke("Logged out.");
    }
}
