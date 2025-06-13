using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;
using RestaurantGLB_Webserver.Models;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace RestaurantGLB_Webserver.Controllers
{
    [ApiController]
    [Route("api/napas")]
    public class NAPASController : Controller
    {
        private readonly UnionPOSContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public static readonly string HYOJUNG_ID = "971216";  // Hyojung
        public static readonly string NAPAS_ID   = "970411";  // NAPAS
        public NAPASController(UnionPOSContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient("NapasClient");
            _configuration = configuration;
        }
        [HttpGet("token")]
        public async Task<IActionResult> GetAccessToken()
        {
            string url = _configuration["NAPAS:BaseURL"] + "/apg/oauth2/token";
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _configuration["NAPAS:ClientID"]),
                new KeyValuePair<string, string>("client_secret", _configuration["NAPAS:ClientSecret"])
            };
            var body = new FormUrlEncodedContent(data);
            
            try
            {
                var response = await _httpClient.PostAsync(url, body);
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("investigation/{f103}/{jwt}/{amount}")]
        public async Task<IActionResult> Investigation(string f103, string jwt, long amount)
        {
            string url = _configuration["NAPAS:BaseURL"] + "/apg/investigation";

            string senderId = HYOJUNG_ID;
            string receiverId = NAPAS_ID;
            
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            string nowFormatted1 = localTime.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
            string nowFormatted2 = localTime.ToString("yyyyMMdd");
            string nowFormatted3 = localTime.ToString("yyyy-MM-dd");

            string refId = localTime.ToString("HHmmssfff");
            refId = refId.PadLeft(12, '0');
            string senderReference = $"{nowFormatted2}{senderId}{"02"}{refId}";

            NAPASRequest body = new NAPASRequest
            {
                payload = new NAPASRequestPayload()
                {
                    caseId = senderReference,
                    creationDateTime = nowFormatted1,
                    amount = amount,
                    issueDate = nowFormatted3,
                    transDateTime = nowFormatted1,
                    id = f103
                },
                header = new NAPASRequestHeader()
                {
                    messageIdentifier = "investrequest",
                    senderReference = $"{nowFormatted2}{senderId}{"02"}{refId}",
                    creationDateTime = nowFormatted1,
                    senderId = senderId,
                    receiverId = receiverId
                }
            };
            string unsignedJson = JsonSerializer.Serialize(body.payload, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            body.header.signature = SignData(unsignedJson);
            string signedJson = JsonSerializer.Serialize(body, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false
            });
            var content = new StringContent(signedJson, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var response = await _httpClient.PostAsync(url, content);
            string responseBody = await response.Content.ReadAsStringAsync();
            return Ok(responseBody);
        }
        [NonAction]
        public string SignData(string payload)
        {
            using (var rsa = LoadPrivateKey())
            {
                var dataBytes = Encoding.UTF8.GetBytes(payload);
                var signedBytes = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                return Convert.ToBase64String(signedBytes);
            }
        }
        [NonAction]
        public bool VerifySignature(string originalPayload, string base64Signature)
        {
            RSA publicKey = LoadPublicKey();
            var dataBytes = Encoding.UTF8.GetBytes(originalPayload);
            var signatureBytes = Convert.FromBase64String(base64Signature);

            return publicKey.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        [NonAction]
        public RSA LoadPublicKey()
        {
            var cert = new X509Certificate2(System.IO.File.ReadAllBytes(_configuration["NAPAS:publicKeyPath"]));
            return cert.GetRSAPublicKey();
        }
        [NonAction]
        public RSA LoadPrivateKey()
        {
            using (var reader = System.IO.File.OpenText(_configuration["NAPAS:privateKeyPath"]))
            {
                PemReader pemReader = new PemReader(reader);
                object keyObject = pemReader.ReadObject();

                if (keyObject is AsymmetricCipherKeyPair keyPair)
                {
                    return DotNetUtilities.ToRSA((RsaPrivateCrtKeyParameters)keyPair.Private);
                }
                else if (keyObject is RsaPrivateCrtKeyParameters rsaPrivateKey)
                {
                    return DotNetUtilities.ToRSA(rsaPrivateKey);
                }
                else
                {
                    throw new Exception("Unsupported key format.");
                }
            }
        }
    }
}
