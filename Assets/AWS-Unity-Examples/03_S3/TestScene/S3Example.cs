using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace AWSUnity
{
    public class S3Example : MonoBehaviour
    {
        [SerializeField] private AWSConfigs _Configs;

        [SerializeField] private string _BucketName;
        [SerializeField] private string _DirName;
        [SerializeField] private Texture2D _SampleTexture;
        [SerializeField] private Button _UploadButton;
        [SerializeField] private Text _MessageText;

        private CognitoClient _CognitoClient;

        private S3Storage _StorageClient;
        private S3Storage StorageClient
        {
            get
            {
                if (_StorageClient == null)
                {
                    _StorageClient = new S3Storage(_Configs);
                }

                return _StorageClient;
            }
        }

        void Awake()
        {
            _CognitoClient = new CognitoClient(_Configs.UserPoolId, _Configs.UserPoolRegion, _Configs.ClientId, _Configs.ClientSecret);
            _UploadButton.onClick.AddListener(OnClickUpload);
        }

        async void OnClickUpload()
        {
            byte[] pngData = _SampleTexture.EncodeToPNG();
            MemoryStream memoryStream = new MemoryStream(pngData);
            string filename = _SampleTexture.name + ".png";

            var response = await StorageClient.PubObjectAsync(_BucketName,  _DirName + "/" + filename, memoryStream, S3CannedACL.PublicRead);
            _MessageText.text = "Status code: " + response.HttpStatusCode;
            Debug.Log("Response: " + response.HttpStatusCode);
        }
    }
}
