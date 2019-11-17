using System;
using UnityEngine;

namespace AWSUnity
{
    [Serializable]
    [CreateAssetMenu(menuName = "AWS for Unity/Create Configs", fileName = "AWSConfigs")]
    public class AWSConfigs : ScriptableObject
    {
        public string UserPoolId;
        public string UserPoolRegion;
        public string ClientId;
        public string ClientSecret;
        public string IdentityPoolId;
        public string DynamoDBRegion;
        public string S3Region;
    }
}
