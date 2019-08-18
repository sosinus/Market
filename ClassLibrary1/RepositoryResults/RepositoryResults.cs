using System;
namespace Models.RepositoryResults
{
    public class GetJwtResult
    {
        public string token { get; set; }
        public bool success { get; set; }

        public GetJwtResult()
        {
            success = false;
        }
    }
}
