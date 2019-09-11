using System;
using UnityEngine;

namespace AWSUnity
{
    [Serializable]
    [CreateAssetMenu(menuName = "AWS for Unity/Create Configs", fileName = "AWSConfigs")]
    public class AWSConfigs : ScriptableObject
    {
        public string IdentityPoolId;
        public string IdentityRegion;
        public string ClientId;
        public string ClientSecret;
    }
}
