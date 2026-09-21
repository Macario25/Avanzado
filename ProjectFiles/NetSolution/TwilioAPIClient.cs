using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FTOptix.Core;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using UAManagedCore;
using System.Collections.Generic;
using FTOptix.SQLiteStore;
using FTOptix.ODBCStore;
using FTOptix.MQTTClient;

public class TwilioAPIClient : BaseNetLogic
{
    [ExportMethod]
    public void EnviarSMS(string accountId, string token, string fromNumber, string toNumber, string mensaje)
    {
        try
        {
            Task.Run(async () =>
            {
                string url = $"https://api.twilio.com/2010-04-01/Accounts/{accountId}/Messages.json";

                using (var client = new HttpClient())
                {
                    // Autenticación Básica
                    var byteArray = Encoding.ASCII.GetBytes($"{accountId}:{token}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    // Contenido de la petición POST (FormUrlEncoded)
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("To", toNumber),
                        new KeyValuePair<string, string>("From", fromNumber),
                        new KeyValuePair<string, string>("Body", mensaje)
                    });

                    // Enviar petición POST a la API de Twilio
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Muestra el código de respuesta en la consola/emulador
                    Log.Info($"Twilio API Status Code: {(int)response.StatusCode}");
                    Log.Info($"Response: {responseBody}");
                }
            }).Wait();
        }
        catch (Exception ex)
        {
            Log.Error($"Error al enviar SMS: {ex.Message}");
        }
    }
}
