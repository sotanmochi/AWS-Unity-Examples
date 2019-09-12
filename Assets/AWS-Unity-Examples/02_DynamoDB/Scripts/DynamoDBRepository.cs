using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace AWSUnity
{
    public class DynamoDBRepository
    {
        protected DynamoDBContext Context;
        IAmazonDynamoDB Client;
        CognitoAWSCredentials Credentials;

        string IdentityPoolId;
        string UserPoolId;
        string CognitoPoolRegion;
        string DynamoDBRegion;

        RegionEndpoint CognitoRegionEndpoint
        {
            get { return RegionEndpoint.GetBySystemName(CognitoPoolRegion); }
        }

        RegionEndpoint DynamoDBRegionEndpoint
        {
            get { return RegionEndpoint.GetBySystemName(DynamoDBRegion); }
        }

        string CognitoIdentityProviderName
        {
            get
            {
                return string.Format("cognito-idp.{0}.amazonaws.com/{1}", CognitoPoolRegion, UserPoolId);
            }
        }

        public DynamoDBRepository(AWSConfigs configs, string idToken)
        {
            IdentityPoolId = configs.IdentityPoolId;
            UserPoolId = configs.UserPoolId;
            CognitoPoolRegion = configs.UserPoolRegion;
            DynamoDBRegion = configs.DynamoDBRegion;

            Credentials = new CognitoAWSCredentials(IdentityPoolId, CognitoRegionEndpoint);
            Credentials.AddLogin(CognitoIdentityProviderName, idToken);
            Client = new AmazonDynamoDBClient(Credentials, DynamoDBRegionEndpoint);
            Context = new DynamoDBContext(Client);
        }
    }
}