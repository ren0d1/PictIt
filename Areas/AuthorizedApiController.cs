namespace PictIt.Areas
{
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class AuthorizedApiController : ApiController
    {
    }
}
