using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AWSUnity
{
    public class CognitoClient
    {
        string UserPoolId;
        string Region;
        string ClientId;
        string ClientSecret;
        
        private RegionEndpoint RegionEndpoint
        {
            get { return RegionEndpoint.GetBySystemName(Region); }
        }

        public CognitoClient(string userPoolId, string region, string clientId, string clientSecret = null)
        {
            this.UserPoolId = userPoolId;
            this.Region = region;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }

        public string CalculateSecretHash(string userPoolClientId, string userPoolClientSecret, string userName)
        {
            byte[] key = System.Text.Encoding.UTF8.GetBytes(userPoolClientSecret);
            using(var hmac = new HMACSHA256(key))
            {
                byte[] rawHmac = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userName + userPoolClientId));
                return System.Convert.ToBase64String(rawHmac);
            }
        }

        public async Task<SignUpResponse> SignUpAsync(string email, string username, string password)
        {
            var client = new AmazonCognitoIdentityProviderClient(null, this.RegionEndpoint);
            SignUpRequest sr = new SignUpRequest();

            sr.ClientId = this.ClientId;
            if(!string.IsNullOrEmpty(this.ClientSecret))
            {
                sr.SecretHash = CalculateSecretHash(this.ClientId, this.ClientSecret, username);
            }
            sr.Username = username;
            sr.Password = password;
            sr.UserAttributes = new List<AttributeType>
            {
                new AttributeType
                {
                    Name = "email",
                    Value = email
                }
            };

            try
            {
                SignUpResponse result = await client.SignUpAsync(sr);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<ConfirmSignUpResponse> ConfirmSignUpAsync(string username, string confirmationCode)
        {
            var client = new AmazonCognitoIdentityProviderClient (null, this.RegionEndpoint);
            ConfirmSignUpRequest confirmSignUpRequest = new ConfirmSignUpRequest();

            confirmSignUpRequest.Username = username;
            confirmSignUpRequest.ConfirmationCode = confirmationCode;
            confirmSignUpRequest.ClientId = this.ClientId;
            if(!string.IsNullOrEmpty(this.ClientSecret))
            {
                confirmSignUpRequest.SecretHash = CalculateSecretHash(this.ClientId, this.ClientSecret, username);
            }

            try
            {
                ConfirmSignUpResponse confirmSignUpResult = await client.ConfirmSignUpAsync(confirmSignUpRequest);
                return confirmSignUpResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<AuthFlowResponse> StartWithSrpAuthAsync(string userId, string password)
        {
            var provider = new AmazonCognitoIdentityProviderClient(null, this.RegionEndpoint);
            CognitoUserPool userPool = new CognitoUserPool(
                UserPoolId,
                ClientId,
                provider,
                ClientSecret
            );
            CognitoUser user = new CognitoUser(
                userId,
                ClientId,
                userPool,
                provider,
                ClientSecret
            );
    
            try
            {
                AuthFlowResponse response = await user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest(){
                    Password = password
                }).ConfigureAwait(false);

                return response;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
