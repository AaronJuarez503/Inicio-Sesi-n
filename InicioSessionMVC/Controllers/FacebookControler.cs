using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json; 
public class FacebookController : Controller
{
    private static readonly HttpClient client = new HttpClient();
    private readonly string _facebookAppId = "2746408812225571";
    private readonly string _redirectUri = "https://n3jgpcxx-5162.use2.devtunnels.ms/signin-facebook"; // Asegúrate de que esta URL esté configurada en Facebook
    private const string _facebookApiVersion = "v22.0";


    public IActionResult Index()
    {
        return View();
    }

    public IActionResult FacebookLogin()
    {
        // Generar un valor de estado aleatorio para prevenir ataques CSRF
        // string state = Guid.NewGuid().ToString();

        // // Guardar el estado en la sesión o en una cookie para verificarlo en la devolución de llamada
        // HttpContext.Session.SetString("FacebookAuthState", state);

        // Construir la URL de inicio de sesión de Facebook
        string facebookLoginUrl = $"https://www.facebook.com/{_facebookApiVersion}/dialog/oauth?" +
                                  $"client_id={_facebookAppId}&" +
                                  $"redirect_uri={HttpUtility.UrlEncode(_redirectUri)}&" +
                                  $"scope=public_profile&" + // Solicita los permisos que necesites
                                  $"response_type=code";

        // Redirigir al usuario a la página de inicio de sesión de Facebook
        return Redirect(facebookLoginUrl);
    }

    // Acción para manejar la devolución de llamada de Facebook
   [HttpGet("/signin-facebook")]
     public async Task<IActionResult> FacebookCallback(string code,string error)
    {
       if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine($"Error de Facebook: {error}");
            ViewBag.ErrorMessage = $"Error al iniciar sesión con Facebook: {error}";
            return View("LoginFailed");
        }
        else if (!string.IsNullOrEmpty(code))
        {
            string apiUrl = $"https://graph.facebook.com/v2.0/me?fields=id,name,email&access_token={code}";

        try
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                return View("FacebookCallbackCodeReceived");
            }
            else
            {
                Console.WriteLine($"Error al obtener el perfil: {response.StatusCode} - {response.ReasonPhrase}");
                return View("FacebookCallbackCodeReceived");
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Excepción HTTP: {e.Message}");
            return View("FacebookCallbackCodeReceived");
        }


            
        }
        else
        {
            Console.WriteLine("No se recibieron parámetros esperados de Facebook.");
            ViewBag.ErrorMessage = "No se recibieron parámetros esperados de Facebook.";
            return View("LoginFailed");
        }
    }

}