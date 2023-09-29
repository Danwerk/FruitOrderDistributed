using System.Diagnostics;
using System.Net;

namespace App.Public.DTO.v1;

public class RestApiErrorResponse
{
    public HttpStatusCode Status { get; set; }
    public string Error { get; set; } = default!;

    
   

}