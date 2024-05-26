using Firebase.Auth;
using Firebase;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthFirebase : Singleton<AuthFirebase>
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        dependencyStatus = dependencyTask.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            //If they are avalible Initialize Firebase
            InitializeFirebase();

            yield return new WaitForEndOfFrame();
            StartCoroutine(CheckForAutoLogin());
        }
        else
        {
            Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    private IEnumerator CheckForAutoLogin()
    {
        if(User != null)
        {
            var reloadUserTask = User.ReloadAsync();
            yield return new WaitUntil(()=>reloadUserTask.IsCompleted);

            AutoLogin();
        }
        else{
            //LoginBackButton();
            UILogin.Ins.LoginBackButton();
        }
    }

    private void AutoLogin()
    {
        if(User != null)
        {
            if(User.IsEmailVerified)
            {
                UILogin.Ins.OpenGamePanel(User.Email);
                FireBaseSetting.Ins.GetToDatabase(User.UserId);
            }
            else
            {
                SendEmailVerification();
            }
            
        }
        else
        {
            UILogin.Ins.LoginBackButton();
        }
    }

    void AuthStateChanged(object state, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null) 
        {
            bool singnedIn = User != auth.CurrentUser && auth.CurrentUser != null;
            if(!singnedIn && User != null)
            {
                Debug.Log("Sign out" + User.UserId);
                UILogin.Ins.LoginBackButton();
            }

            User = auth.CurrentUser;

            if(singnedIn )
            {
                Debug.Log("Sign In" + User.UserId);
            }
        }
    }

    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(UILogin.Ins.emailLoginField.text, UILogin.Ins.passwordLoginField.text));
    }

    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(UILogin.Ins.emailRegisterField.text, UILogin.Ins.passwordRegisterField.text, UILogin.Ins.usernameRegisterField.text));
    }

    public void ForgetButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(ForgetPassword(UILogin.Ins.emailForgetPassword.text));
    }

    public void OnShowEmail(bool isVer, string email, string error)
    {
        if (isVer)
        {
            UILogin.Ins.tileText.text = "Please Verify Your Email.";

            UILogin.Ins.inforText.text = $"Please verify your email address \n Verification email has been sent to {email}";
        }
        else
        {

            UILogin.Ins.tileText.text = "Don't Sent To Your Email!";

            UILogin.Ins.inforText.text = $"Couldn't sent email : {error}";
        }

        UILogin.Ins.SendEmailPanel.SetActive(true);
    }


    public void ScoreboardButton()
    {
        StartCoroutine(LoadScoreboardData());
    }

    public void LogOut()
    {
        if(auth != null && User!=null)
        {
            auth.SignOut();
            SceneManager.LoadScene(0);
            //LoadingMenuManager.Ins.SwitchToScene(0);
        }
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
            UILogin.Ins.warningLoginText.text = message;
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
                UILogin.Ins.warningLoginText.text = "";
                UILogin.Ins.confirmLoginText.text = "Logged In";

                //SaveSystem.SetString("access_token",User.UserId);
                //PlayerPrefs.SetString(Constant.ACCESS_TOKEN, User.UserId);

                //SceneManager.LoadScene("SampleScene");
                FireBaseSetting.Ins.GetToDatabase(User.UserId);
                //LoadingMenuManager.Ins.SwitchToScene(1);
                //OpenGamePanel();
                UILogin.Ins.OpenGamePanel(User.Email);
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
            UILogin.Ins.warningRegisterText.text = "Missing Username";
        }
        else if (UILogin.Ins.passwordRegisterField.text != UILogin.Ins.passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            UILogin.Ins.warningRegisterText.text = "Password Does Not Match!";
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
                UILogin.Ins.warningRegisterText.text = message;
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
                        UILogin.Ins.warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        UILogin.Ins.warningRegisterText.text = "";
                        UserData x = new UserData();
                        x.email = User.Email;
                        x.userID = User.UserId;
                        x.userName = User.DisplayName;
                        FireBaseSetting.Ins.PutToDatabase(x);
                        //StartCoroutine(Login(_email, _password));
                        if (User.IsEmailVerified)
                        {
                            //LoginBackButton();
                            UILogin.Ins.LoginBackButton();
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
        UILogin.Ins.infor.SetActive(true);
        if (ForgetPasswordTask.IsFaulted)
        {
            UILogin.Ins.warForgetPasswordText.text = "Password reset email sending failed.";
            foreach (var exception in ForgetPasswordTask.Exception.Flatten().InnerExceptions)
            {
                Debug.LogError("Error message: " + exception.Message);
            }
        }
        else if (ForgetPasswordTask.IsCanceled)
        {
            UILogin.Ins.warForgetPasswordText.text = "Password reset email sending was canceled.";
        }
        else
        {
            UILogin.Ins.warForgetPasswordText.text = "";
            UILogin.Ins.comForgetPasswordText.text = "Password reset email sent successfully.";
        }
    }

    private IEnumerator LoadScoreboardData()
    {
        //Get all the users data ordered by kills amount
        Task<DataSnapshot> DBTask = DBreference.Child("email").OrderByChild("countKill").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string email = childSnapshot.Child("email").Value.ToString();
                int kills = int.Parse(childSnapshot.Child("countKill").Value.ToString());
                int currentLevel = int.Parse(childSnapshot.Child("currentLevel").Value.ToString());

                //Instantiate new scoreboard elements
                //GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                //scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, kills, deaths, xp);
                Debug.Log($"{email} {kills} {currentLevel}");
            }

            //Go to scoareboard screen
            //UIManager.Ins.ScoreboardScreen();
        }
    }

    public void OpenGameScene()
    {
        LoadingMenuManager.Ins.SwitchToScene(1);
        UILogin.Ins.ResetUI();
    }
}
