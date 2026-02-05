using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseBootstrap : MonoBehaviour
{
    public static bool Ready { get; private set; }

    private void Awake()
    {
        Ready = false;
    }

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var status = task.Result;

            if (status == DependencyStatus.Available)
            {
                Ready = true;
                Debug.Log("Firebase is ready.");
            }
            else
            {
                Debug.LogError("Firebase dependency issue: " + status);
            }
        });
    }
}
