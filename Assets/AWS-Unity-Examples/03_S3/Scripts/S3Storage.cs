using Amazon;
using Amazon.CognitoIdentity;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AWSUnity
{
    public class S3Storage
    {
        IAmazonS3 Client;
        CognitoAWSCredentials Credentials;

        string IdentityPoolId;
        string UserPoolId;
        string CognitoPoolRegion;
        string S3Region;

        RegionEndpoint CognitoRegionEndpoint
        {
            get { return RegionEndpoint.GetBySystemName(CognitoPoolRegion); }
        }

        RegionEndpoint S3RegionEndpoint
        {
            get { return RegionEndpoint.GetBySystemName(S3Region); }
        }

        string CognitoIdentityProviderName
        {
            get
            {
                return string.Format("cognito-idp.{0}.amazonaws.com/{1}", CognitoPoolRegion, UserPoolId);
            }
        }

        public S3Storage(AWSConfigs configs, string idToken  = null)
        {
            IdentityPoolId = configs.IdentityPoolId;
            UserPoolId = configs.UserPoolId;
            CognitoPoolRegion = configs.UserPoolRegion;
            S3Region = configs.S3Region;

            Credentials = new CognitoAWSCredentials(IdentityPoolId, CognitoRegionEndpoint);

            if (!string.IsNullOrEmpty(idToken))
            {
                Credentials.AddLogin(CognitoIdentityProviderName, idToken);
            }

            Client = new AmazonS3Client(Credentials, S3RegionEndpoint);
        }

        public async Task<PutObjectResponse> PubObjectAsync(string bucketName, string filename, MemoryStream memoryStream, S3CannedACL acl)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = filename,
                InputStream = memoryStream,
                CannedACL = acl,
            };

            try
            {
                return await Client.PutObjectAsync(request);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
