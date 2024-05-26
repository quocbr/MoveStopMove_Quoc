using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class UILogin : MonoBehaviour
{
    public static UILogin Ins;
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

    [Header("Logut")]
    public GameObject LogoutPanel;
    public TMP_Text inforGameText;

    private void Awake()
    {
        if (Ins != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Ins = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ResetUI()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(false);
        ForgetPasswordPanel.SetActive(false);
        SendEmailPanel.SetActive(false);
        LogoutPanel.SetActive(false);
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
        inforGameText.text = "";
        infor.SetActive(false);
    }

    public void LoginBackButton()
    {
        ResetAttibutes();
        ResetUI();
        LoginPanel.SetActive(true);

    }

    public void RegisterBackButton()
    {
        ResetAttibutes();
        ResetUI();
        RegisterPanel.SetActive(true);
    }

    public void ForgetBackButton()
    {
        ResetAttibutes();
        ForgetPasswordPanel.SetActive(true);
    }

    public void OffShowEmail()
    {
        SendEmailPanel.SetActive(false);
    }

    public void OpenGamePanel(string email)
    {
        ResetUI();
        inforGameText.text = $"You are logged into the account\n<color=#00FFFF>{email}</color>\nAre you sure you want to sign in?";
        LogoutPanel.SetActive(true);
    }

    public void Logout()
    {
        AuthFirebase.Ins.LogOut();
    }
}
