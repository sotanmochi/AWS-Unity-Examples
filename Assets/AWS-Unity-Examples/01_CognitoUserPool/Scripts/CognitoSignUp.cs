using Amazon.CognitoIdentityProvider.Model;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AWSUnity
{
    public class CognitoSignUp : MonoBehaviour
    {
        [SerializeField] private AWSConfigs Configs;

        [SerializeField] private InputField Email;
        [SerializeField] private InputField Username;
        [SerializeField] private InputField Password;
        [SerializeField] private Button SignUpButton;
        [SerializeField] private Text MessageText;
        [SerializeField] private InputField ConfirmationCode;
        [SerializeField] private Button ConfirmButton;

        private CognitoClient CognitoClient;

        void Awake()
        {
            CognitoClient = new CognitoClient(Configs.IdentityPoolId, Configs.IdentityRegion, Configs.ClientId, Configs.ClientSecret);
            SignUpButton.onClick.AddListener(OnClickSignUp);
            ConfirmButton.onClick.AddListener(OnClickConfirm);
        }

        private async void OnClickSignUp()
        {
            SignUpResponse response = null;
            try
            {
                response = await CognitoClient.SignUpAsync(Email.text, Username.text, Password.text);
            }
            catch(Exception ex)
            {
                MessageText.text  = ex.Message;
            }

            if(response != null)
            {
                MessageText.text = "Status: " + response.HttpStatusCode.ToString()
                                + ", UserConfirmed: " + response.UserConfirmed;
            }
        }

        private async void OnClickConfirm()
        {
            ConfirmSignUpResponse response = null;
            try
            {
                response = await CognitoClient.ConfirmSignUpAsync(Username.text, ConfirmationCode.text);
            }
            catch(Exception ex)
            {
                MessageText.text  = ex.Message;
            }

            if(response != null)
            {
                MessageText.text = "Status: " + response.HttpStatusCode.ToString();
            }
        }
    }
}
