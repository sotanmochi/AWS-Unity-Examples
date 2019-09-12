using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWSUnity
{
    [DynamoDBTable("UserGroup")]
    public class UserGroup
    {
        [DynamoDBHashKey]
        public int Id { get; set; }
        [DynamoDBProperty]
        public string GroupId { get; set; }
        [DynamoDBProperty]
        public string UserId { get; set; }
    }

    public class UserGroupRepository : DynamoDBRepository
    {
        public UserGroupRepository(AWSConfigs configs, string idToken) : base(configs, idToken)
        {
        }

        public async Task<List<UserGroup>> FindByUserId(string userId)
        {
            string indexName = "UserId-index";
            string attributeName = "UserId";
            DynamoDBEntry value = userId;

            try
            {
                var search = Context.FromQueryAsync<UserGroup>(new QueryOperationConfig()
                {
                    IndexName = indexName,
                    Filter = new QueryFilter(attributeName, QueryOperator.Equal, value)
                });
                return await search.GetRemainingAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<List<UserGroup>> FindByGroupId(string groupId)
        {
            string indexName = "GroupId-index";
            string attributeName = "GroupId";
            DynamoDBEntry value = groupId;

            try
            {
                var search = Context.FromQueryAsync<UserGroup>(new QueryOperationConfig()
                {
                    IndexName = indexName,
                    Filter = new QueryFilter(attributeName, QueryOperator.Equal, value)
                });
                return await search.GetRemainingAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}