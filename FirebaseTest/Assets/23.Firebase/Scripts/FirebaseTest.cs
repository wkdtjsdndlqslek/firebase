using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseTest : MonoBehaviour
{
    private async void Start()
    {
        DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync();
        print(status);
        var result = await FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync("abc@abc.abc", "123456");
        print(result.User.UserId);
    }
}
