using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;

using Com.Logrocket.Core.Graphics;
using Com.Logrocket.Core.Filter;
using Com.Logrocket.Core.Performance;
using Com.Logrocket.Core.Network;
using Com.Logrocket.Core;
using System;
using Java.Net;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using Android.Webkit;
using Android.Media;
using Xamarin.Essentials;
using Java.Interop;
using Com.Logrocket.Sdk;
using Android.Nfc;
using Java.Lang;
using Android.Widget;
using System.IO;
using Java.IO;
using System.Threading.Tasks;

namespace Logrocket
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        //private SDK.IOptionsConfiguration optionsConfiguration;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Button btn = (Button)FindViewById(Resource.Id.button);
            btn.Click += OnClick;

            //"p1l1q5/firsttestlogrocket"

            SDKConfig config = new SDKConfig
            {
                AppID = "p1l1q5/firsttestlogrocket",
                ConnectionType = SDK.ConnectionType.Wifi,
                LogLevel = SDK.LogLevel.Info,
                ShouldSendCrashReport = true,
                ShouldDetectExceptions = true,
            };

            SDKInit.Init(this.Application, BaseContext, config);

            Dictionary<string, string> userData = new Dictionary<string, string>
            {
                { "name", "Dmitriy Blockiy" },
                { "email", "blockiy.dima.s@gmail.com" }
            };

            SDK.Identify("28dvm2jfa", userData);

            Logger.E("Test E", "Test message");
            Logger.W("Test W", "Test message");
            Logger.I("Test I", "Test message");
            Logger.D("Test D", "Test message");

            var myWebView = (WebView)FindViewById(Resource.Id.webview);
            SDK.RegisterWebView(myWebView);

            URL endpoint = new URL("https://example.com/");

            IResponseBuilder responseBuilder =
                   SDK.NewRequestBuilder()
                       .SetUrl(endpoint.ToString())
                       .SetMethod("GET")
                       .Capture();

            long startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            try
            {
                await Task.Run(() =>
                {
                    HttpURLConnection conn = (HttpURLConnection)endpoint.OpenConnection();
                    responseBuilder
                        .SetStatusCode((int)conn.ResponseCode)
                        .SetDuration(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startTime)
                        // The body field on both the Request and Response builders must be a String.
                        // Content must be sanitized before registering to the response builder.
                        .SetBody(ReadRedactedResponseBody(conn))
                        // Redact sensitive fields before adding to the response builder. The HttpURLConnection
                        // provides an array of values for each key, but we only accept a single value.
                        .SetHeaders(FlattenAndRedactHeaders(conn.HeaderFields))
                        .Capture();
                });

                // Work with your response!
            }
            catch (System.IO.IOException err)
            {
                // Network failures are represented as Responses with a status code of 0. If a captured
                // request does not have a matching response captured it will appear as an unending request
                // during session playback.

                responseBuilder
                    .SetStatusCode(0)
                    .SetDuration(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startTime)
                    .Capture();

                // Re-surface the actual IOException
                throw err;
            }
        }
        private string ReadRedactedResponseBody(HttpURLConnection conn)
        {
            if (conn.ResponseCode == HttpStatus.Ok)
            {
                BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(conn.InputStream));
                string inputLine;
                StringBuilder response = new StringBuilder();

                while ((inputLine = bufferedReader.ReadLine()) != null)
                {
                    response.Append(inputLine);
                }
                bufferedReader.Close();

                return RedactBody(response.ToString());
            }

            return "";
        }

        private string RedactBody(string body)
        {
            if (body.Contains("ignore"))
            {
                return "";
            }
            return body;
        }

        private IDictionary<string, string> FlattenAndRedactHeaders(IDictionary<string, IList<string>> headers)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            foreach (var entry in headers)
            {
                string key = entry.Key;

                if (!string.IsNullOrEmpty(key))
                {
                    // Do not capture auth related headers.
                    if (key.ToLower().Equals("authentication") || key.ToLower().Equals("authorization"))
                    {
                        continue;
                    }

                    result.Add(key, JoinValues(entry.Value));
                }
            }

            return result;
        }

        private string JoinValues(IList<string> values)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < values.Count; i++)
            {
                sb.Append(values[i]);
                if (i != values.Count - 1)
                {
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }
        private void OnClick(object sender, EventArgs e)
        {
            Logger.E("Test E", "Test message");
            throw new NotImplementedException();
        }
    }
}