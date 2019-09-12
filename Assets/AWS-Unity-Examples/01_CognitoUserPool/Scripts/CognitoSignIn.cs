using Amazon.Extensions.CognitoAuthentication;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AWSUnity
{
    public class CognitoSignIn : MonoBehaviour
    {
        [SerializeField] private AWSConfigs Configs;

        [SerializeField] private InputField Username;
        [SerializeField] private InputField Password;
        [SerializeField] private Button SignInButton;
        [SerializeField] private Text MessageText;

        private CognitoClient CognitoClient;

        void Awake()
        {
            CognitoClient = new CognitoClient(Configs.UserPoolId, Configs.UserPoolRegion, Configs.ClientId, Configs.ClientSecret);
            SignInButton.onClick.AddListener(OnClickSignIn);
        }

        private async void OnClickSignIn()
        {
            try
            {
                AuthFlowResponse response = await CognitoClient.StartWithSrpAuthAsync(Username.text, Password.text);
                MessageText.text = "TokenType: " + response.AuthenticationResult.TokenType;
                Debug.Log("IdToken: " + response.AuthenticationResult.IdToken);
            }
            catch(Exception ex)
            {
                MessageText.text = ex.ToString();
            }
        }
    }
}
