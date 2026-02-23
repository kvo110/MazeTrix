using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class FirestoreTest : MonoBehaviour
{
  private FirebaseFirestore db;

  private void Start()
  {
    if (!FirebaseBootstrap.Ready)
    {
      Debug.Log("Firebase not ready for Firestore.");
      return;
    }

    db = FirebaseFirestore.DefaultInstance;

    WriteTestData();
  }

  private void WriteTestData()
  {
    Dictionary<string, object> player = new Dictionary<string, object>
    {
      { "username", "TestUser" },
      { "level", 1 },
      { "health", 100 }
    };

    db.Collection("players").Document("test_player")
      .SetAsync(player)
      .ContinueWithOnMainThread(task =>
      {
        if (task.IsCompleted)
        {
          Debug.Log("Firestore write successful.");
        }
        else
        {
          Debug.LogError("Firestore write failed: " + task.Exception);
        }
      });
  }
}
