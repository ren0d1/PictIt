namespace PictIt.Areas.User
{
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class AuthorizedApiController : ApiController
    {
    }
}
