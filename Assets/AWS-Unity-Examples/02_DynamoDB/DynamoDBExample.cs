using Amazon.Extensions.CognitoAuthentication;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AWSUnity
{
    public class DynamoDBExample : MonoBehaviour
    {
        [SerializeField] private AWSConfigs Configs;

        [SerializeField] private InputField Username;
        [SerializeField] private InputField Password;
        [SerializeField] private Button SignInButton;
        [SerializeField] private Text MessageText;
        [SerializeField] private Button QueryButton;

        List<UserGroup> userGroups = new List<UserGroup>();
        List<Book> books = new List<Book>();

        private CognitoClient CognitoClient;
        private string IdToken;

        private UserGroupRepository _UserGroupRepository;
        private UserGroupRepository UserGroupRepository
        {
            get
            {
                if (_UserGroupRepository == null)
                {
                    _UserGroupRepository = new UserGroupRepository(Configs, IdToken);
                }

                return _UserGroupRepository;
            }
        }

        private BookRepository _BookRepository;
        private BookRepository BookRepository
        {
            get
            {
                if (_BookRepository == null)
                {
                    _BookRepository = new BookRepository(Configs, IdToken);
                }

                return _BookRepository;
            }
        }

        void Awake()
        {
            CognitoClient = new CognitoClient(Configs.UserPoolId, Configs.UserPoolRegion, Configs.ClientId, Configs.ClientSecret);
            SignInButton.onClick.AddListener(OnClickSignIn);
            QueryButton.onClick.AddListener(OnClickQuery);
        }

        private async void OnClickSignIn()
        {
            try
            {
                AuthFlowResponse response = await CognitoClient.StartWithSrpAuthAsync(Username.text, Password.text);
                IdToken = response.AuthenticationResult.IdToken;

                if (!string.IsNullOrEmpty(IdToken))
                {
                    MessageText.text = "Login success";
                }
            }
            catch(Exception ex)
            {
                MessageText.text = ex.ToString();
            }
        }

        private async void OnClickQuery()
        {
            string userId = Username.text;

            try
            {
                userGroups = await UserGroupRepository.FindByUserId(userId);
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
            }

            try
            {
                if (userGroups[0] != null)
                {
                    string groupId = userGroups[0].GroupId;
                    books = await BookRepository.FindByGroupId(groupId);
                }
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
            }

            foreach(var ug in userGroups)
            {
                Debug.Log("UserGroup: " + ug.Id + ", GroupId: " + ug.GroupId + ", UserId: " + ug.UserId);
            }
            foreach(var book in books)
            {
                Debug.Log("Book: " + book.Id + ", GroupId: " + book.GroupId + ", Title: " + book.Title);
            }
        }
    }
}
