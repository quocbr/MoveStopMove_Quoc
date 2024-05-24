using Firebase.Auth;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using System;

public class AuthFirebase : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    //Login variables
    [Header("Login")]
    public GameObject LoginPanel;
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public GameObject RegisterPanel;
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    [Header("Forget Password")]
    public GameObject ForgetPasswordPanel;
    public TMP_InputField emailForgetPassword;
    public GameObject infor;
    public TMP_Text warForgetPasswordText;
    public TMP_Text comForgetPasswordText;

    [Header("Send Email")]
    public GameObject SendEmailPanel;
    public TMP_Text tileText;
    public TMP_Text inforText;

    void Awake()
    {
        try
        {
            //Check that all of the necessary dependencies for Firebase are present on the system
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    //If they are avalible Initialize Firebase
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
    }

    private void ResetAttibutes()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
        warningLoginText.text = "";
        confirmLoginText.text = "";
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
        warningRegisterText.text = "";
        emailForgetPassword.text = "";
        warForgetPasswordText.text = "";
        comForgetPasswordText.text = "";
        infor.SetActive(false);
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    public void LoginBackButton()
    {
        ResetAttibutes();
        SendEmailPanel.SetActive(false);
        RegisterPanel.SetActive(false);
        ForgetPasswordPanel.SetActive(false);
        LoginPanel.SetActive(true);
        
    }

    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    public void RegisterBackButton()
    {
        ResetAttibutes();
        LoginPanel.SetActive(false);
        SendEmailPanel.SetActive(false);
        ForgetPasswordPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    public void ForgetButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(ForgetPassword(emailForgetPassword.text));
    }

    public void ForgetBackButton()
    {
        ResetAttibutes();
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(false);
        SendEmailPanel.SetActive(false);
        ForgetPasswordPanel.SetActive(true);
    }

    public void OnShowEmail(bool isVer, string email, string error)
    {
        if (isVer)
        {
            tileText.text = "Please Verify Your Email.";
            inforText.text = $"Please verify your email address \n Verification email has been sent to {email}";
        }
        else
        {
            tileText.text = "Don't Sent To Your Email!";
            inforText.text = $"Couldn't sent email : {error}";
        }
        SendEmailPanel.SetActive(true);
    }

    public void OffShowEmail()
    {
        SendEmailPanel.SetActive(false);
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result.User;
            Debug.Log(User.UserId);
            if (User.IsEmailVerified)
            {
                Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
                warningLoginText.text = "";
                confirmLoginText.text = "Logged In";

                //SaveSystem.SetString("access_token",User.UserId);
                PlayerPrefs.SetString("access_token", User.UserId);

                //SceneManager.LoadScene("SampleScene");
                LoadingMenuManager.Ins.SwitchToScene(1);
            }
            else{
                SendEmailVerification();
            }

        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
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
                //User has now been created
                //Now get the result
                User = RegisterTask.Result.User;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                     var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        warningRegisterText.text = "";
                        UserData x = new UserData();
                        x.email = User.Email;
                        x.userID = User.UserId;
                        x.userName = User.DisplayName;
                        FireBaseSetting.Ins.PutToDatabase(x);
                        //StartCoroutine(Login(_email, _password));
                        if (User.IsEmailVerified)
                        {
                            LoginBackButton();
                        }
                        else
                        {
                            SendEmailVerification();
                        }
                    }
                }
            }
        }
    }

    public void SendEmailVerification()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(SendEmailVerificationAsync());
    }

    private IEnumerator SendEmailVerificationAsync()
    {
        //Call the Firebase auth signin function passing the email and password
        var RegisterTask = User.SendEmailVerificationAsync();
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to send email task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
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
                    case AuthError.Cancelled:
                        message = "Email VerifiCation Was Canelled";
                        break;
                    case AuthError.TooManyRequests:
                        message = "Too Many Request";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        message = "The Email You Entered Is Invalid";
                        break;
                }
                OnShowEmail(false, User.Email, message);
            }
            else
            {
                Debug.Log("Email Has Success Send.");
                OnShowEmail(true, User.Email, null);
            }
    }

    private IEnumerator ForgetPassword(string _email)
    {
        //Call the Firebase auth signin function passing the email and password
        var ForgetPasswordTask = auth.SendPasswordResetEmailAsync(_email);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ForgetPasswordTask.IsCompleted);

        // Wait until the task completes
        yield return new WaitUntil(() => ForgetPasswordTask.IsCompleted);
        infor.SetActive(true);
        if (ForgetPasswordTask.IsFaulted)
        {
            warForgetPasswordText.text = "Password reset email sending failed.";
            foreach (var exception in ForgetPasswordTask.Exception.Flatten().InnerExceptions)
            {
                Debug.LogError("Error message: " + exception.Message);
            }
        }
        else if (ForgetPasswordTask.IsCanceled)
        {
            warForgetPasswordText.text = "Password reset email sending was canceled.";
        }
        else
        {
            comForgetPasswordText.text = "Password reset email sent successfully.";
        }
    }
}
