using System;
namespace Models.RepositoryResults
{
    public class GetJwtResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
    }

    public class LoadPageResult
    {
        public bool HasDefaultUser { get; set; }
    }

    public class CreateUserResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public bool IsAlreadyExist { get; set; }
        public bool Other { get; set; }
    }

    public class ItemResult
    {
        public bool Success { get; set; }
    }

    public class AddOrderResult
    {
        public bool Success { get; set; }
    }
}
