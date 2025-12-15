using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using TMPro;
using Google;
using Facebook.Unity;

public class AuthManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;
    
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;
    private string googleWebClientId = "953106095400-82b0l49vrcpc75s5vdi5ucsvhu3c6vcl.apps.googleusercontent.com";


    public GameObject loginScene;
    public GameObject sigScene;
    
    private void Start()
    {
        loginScene.SetActive(true);
        sigScene.SetActive(false);
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.Log("sos");
            }
        });

        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
        }
    public void ShowLogin()
    {
        loginScene.SetActive(true);
        sigScene.SetActive(false);
    }
    public void ShowSig()
    {
        loginScene.SetActive(false);
        sigScene.SetActive(true);
    }
    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }
    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text,passwordLoginField.text));
    }
    public void RegisterButton()
    {
        StartCoroutine(Register(emailRegisterField.text,passwordRegisterField.text,usernameRegisterField.text));
    }
    public void GoogleButton()
    {
        StartCoroutine(GoogleSigIn());
    }
    public void FaceBookButton()
    {
        StartCoroutine(FacebookSignIn());
    }
    private IEnumerator Login(string _email,string _password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email,_password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        if(LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with{LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError error = (AuthError)firebaseEx.ErrorCode;
            string message = "Login Failed";
            switch (error)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
               
            }
            Debug.LogWarning(message);
        }
        else
        {
            User = LoginTask.Result.User;
            Debug.Log($"User successfully: {User.DisplayName} {User.Email}");
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if(_username == "")
        {
            warningRegisterText.text = "Missing Username";
        }
        else if(passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
            if(RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                string message = "Register Failed";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                User = RegisterTask.Result.User;
                if(User != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _username };
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate:() => ProfileTask.IsCompleted);
                    if(ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";

                    }
                    else
                    {
                        ShowLogin();
                        warningRegisterText.text = "";

                    }
                }
            }
        }
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    private IEnumerator GoogleSigIn()
    {
        GoogleSignInConfiguration configuration = new GoogleSignInConfiguration
        {

            WebClientId = googleWebClientId,
            RequestIdToken = true,
            RequestEmail = true,
        };
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        var SignInTask = GoogleSignIn.DefaultInstance.SignIn();
        yield return new WaitUntil(() => SignInTask.IsCompleted);
        if (SignInTask.Exception != null)
        {
            Debug.Log("Google SignIn failed" + SignInTask.Exception);
            warningLoginText.text = "Google Sign-In Failed";
        }
        else
        {
            GoogleSignInUser user = SignInTask.Result;
            string idToken = user.IdToken;
            Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
            StartCoroutine(SignInWithCredential(credential));
        }
    }
    private IEnumerator FacebookSignIn()
    {

        yield return new WaitUntil(() => FB.IsInitialized);
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }
    private void AuthCallback(ILoginResult result)
    {
        if (result == null)
        {
            Debug.LogError("Facebook login result is null");
            warningLoginText.text = "Facebook Sign-In Failed";
            return;
        }
        if (FB.IsLoggedIn)
        {
            var aToken = AccessToken.CurrentAccessToken;

            Debug.Log("Facebook login successful");
            Debug.Log("User ID: " + aToken.UserId);
            Debug.Log("Token: " + aToken.TokenString);

            foreach (string perm in aToken.Permissions)
            {
                Debug.Log("Permission: " + perm);
            }

            Credential credential = FacebookAuthProvider.GetCredential(aToken.TokenString);
            StartCoroutine(SignInWithCredential(credential));
        }
        else
        { 
            if (result.Cancelled)
            {
                Debug.Log("User cancelled Facebook login");
                warningLoginText.text = "Facebook Sign-In Cancelled";
            }
            else if (!string.IsNullOrEmpty(result.Error))
            {
                Debug.LogError("Facebook login error: " + result.Error);
                warningLoginText.text = "Facebook Error: " + result.Error;
            }
            else
            {
                Debug.LogError("Facebook login failed");
                warningLoginText.text = "Facebook Sign-In Failed";
            }
        }
    }

    private IEnumerator SignInWithCredential(Credential credential)
    {
           var signInTask = auth.SignInWithCredentialAsync(credential) ;
            yield return new WaitUntil(()=>  signInTask.IsCompleted);
        if(signInTask.Exception != null)
        {
            Debug.LogWarning($"Fail to sign in with credential: {signInTask.Exception}");
            FirebaseException firebaseEx = signInTask.Exception.GetBaseException() as FirebaseException;   
            AuthError errorCode = (AuthError) firebaseEx.ErrorCode;
            string message = "Sign-In Failed";
            switch (errorCode)
            {
                case AuthError.AccountExistsWithDifferentCredentials:
                    message = "Account exists with different credentials";
                    break;
                case AuthError.InvalidCredential:
                    message = "Invalid credentials";
                    break;
                default:
                    message = "Authentication Failed";
                    break;
            }
            warningLoginText.text = message;

        }
    }
}
