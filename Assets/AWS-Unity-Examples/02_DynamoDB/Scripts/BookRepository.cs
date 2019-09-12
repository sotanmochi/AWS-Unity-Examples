using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWSUnity
{
    [DynamoDBTable("Book")]
    public class Book
    {
        [DynamoDBHashKey]
        public int Id { get; set; }
        [DynamoDBProperty]
        public string ISBN { get; set; }
        [DynamoDBProperty]
        public string GroupId { get; set; }
        [DynamoDBProperty]
        public string Title { get; set; }
    }

    public class BookRepository : DynamoDBRepository
    {
        public BookRepository(AWSConfigs configs, string idToken) : base(configs, idToken)
        {
        }

        public async Task<List<Book>> FindByGroupId(string groupId)
        {
            string indexName = "GroupId-index";
            string attributeName = "GroupId";
            DynamoDBEntry value = groupId;

            try
            {
                var search = Context.FromQueryAsync<Book>(new QueryOperationConfig()
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