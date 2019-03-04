using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ResultOf;
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

// ReSharper disable IdentifierTypo

namespace SharkBank
{
    public class HttpServer
    {
        private readonly HttpListener Listener;
        private HttpListenerRequest currentRequest;
        private HttpListenerResponse currentResponse;
        private readonly Db dataBase;
        private bool accesPermitted;
        private string currentSession;
        
        public HttpServer(string prefix, Db database)
        {
            dataBase = database;
            Listener = new HttpListener();
            Listener.Prefixes.Add(prefix);
        }

        public void Run()
        {
            Listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for request...");
                var context = Listener.GetContext();
                currentRequest = context.Request;
                currentResponse = context.Response;
                HandleCOR();
                accesPermitted = false;
                if (currentRequest.Cookies.Count != 0)
                    PermitAcces(currentRequest.Cookies[0])
                        .Then(s => accesPermitted = true)
                        .OnFail(s => accesPermitted = false);
                if (currentRequest.HttpMethod == HttpMethod.Post.Method)
                {
                    if (currentRequest.Url.OriginalString.EndsWith("auth"))
                        Auth();
                    else
                    {
                        InsertInDataBase()
                            .Then(s => currentResponse.StatusCode = 200)
                            .OnFail(f => currentResponse.StatusCode = f=="Forbidden" ? 403 : 500);
                    }
                    currentResponse.SendChunked = false;
                }
                if(currentRequest.Url.OriginalString.EndsWith("admin/request"))
                    if (!accesPermitted)
                        currentResponse.StatusCode = 403;
                    else
                    {
                        var data = dataBase.GetAllRequests();
                        currentResponse.StatusCode = 200;
                        currentResponse.ContentType = "application/json";
                        currentResponse.ContentLength64 = Encoding.UTF8.GetByteCount(data);
                        using (var stream = currentResponse.OutputStream)
                        {
                            using(var writer = new StreamWriter(stream))
                                writer.Write(data);
                        }
                    }

                currentResponse.Close();
            }
        }

        private void Auth()
        {
            if (accesPermitted)
            {
                currentResponse.StatusCode = 200;
                return;
            }
            var formContent = ReadContent();
            var username = formContent["username"];
            var password = formContent["password"];
            Login(username, password)
                .Then(s => SetCookies(s, username))
                .OnFail(f => currentResponse.StatusCode = 500);
        }

        private Result<None> PermitAcces(Cookie cookie)
        {
            if(cookie.Expired) 
                return Result.Fail<None>("Cookie expired");
            if(cookie.Name != "SESS_ID" || cookie.Value.Split("|").Length != 2) 
                return Result.Fail<None>("Cookie is not from this domain");
            var splittedCookies = cookie.Value.Split("|");
            currentSession = splittedCookies[0];
            var signature = splittedCookies[1];
            if (SessionsRepo.CheckCookieSignature(currentSession, signature)) return Result.Ok();
            return Result.Fail<None>("Cookie either expired either modified");
        }


        private void SetCookies(bool userPresent, string username)
        {
            if (userPresent)
            {
                var edded = SessionsRepo.AddSession(username);
                var cookie = new Cookie("SESS_ID", edded.Item1, "/", "api.sharkbank.ru");
                cookie.Expires = edded.Item2;
                currentResponse.AppendHeader("X-CSRF-Token", edded.Item3);
                currentResponse.SetCookie(cookie);
                currentResponse.StatusCode = 200;
            }
            else{
                currentResponse.StatusCode = 401;
            }
        }

        private Result<bool> Login(string username, string password)
        {
            return dataBase.CheckUser(username, password);
        }
        
        private bool CheckCsrf()
        {
            var token = currentRequest.Headers.GetValues("X-Csrf-Token");
            return accesPermitted && token != null && SessionsRepo.CheckFormToken(currentSession, token[0]);
        }

        private Result<int> InsertInDataBase()
        {
            if(!CheckCsrf()) return Result.Fail<int>("Forbidden");
            if (currentRequest.Url.OriginalString.EndsWith("card-payment"))
                return PayWithCredit();
            if (currentRequest.Url.OriginalString.EndsWith("request-payment"))
                return RequestPayment();
            return Result.Fail<int>("Not supported");
        }

        private void HandleCOR()
        {
            currentResponse.AppendHeader("Access-Control-Allow-Origin", "http://localhost:8080");
            currentResponse.AddHeader("Access-Control-Allow-Credentials", "true");
            currentResponse.SendChunked = false;
            currentResponse.AddHeader("Access-Control-Expose-Headers", "X-CSRF-Token");
            currentResponse.AddHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, X-CSRF-Token");
            currentResponse.AddHeader("Access-Control-Allow-Methods", "GET, POST");
            currentResponse.AddHeader("Access-Control-Max-Age", "1728000");
            
        }

        private Result<int> RequestPayment()
        {
            var formContent = ReadContent();
            return dataBase.InsertRequest(formContent["reqFromPayer"], formContent["bicForReq"], 
                formContent["reqAccountNumber"], formContent["reqPaymentPurpose"], int.Parse(formContent["reqTotalCost"]),
                formContent["reqPhoneNumber"], formContent["reqEmail"], DateTime.Now);

        }

        private Result<int> PayWithCredit()
        {
            var formContent = ReadContent();
            var splittedDate = formContent["expirationDate"].Split("/").Select(int.Parse).ToArray();
            return dataBase.InsertPayments(formContent["cardNumber"], new DateTime(2000 + splittedDate[1], splittedDate[0], 1),
                int.Parse(formContent["totalCost"]), DateTime.Now);
        }

        private Dictionary<string, string> ReadContent()
        {
            var form = new Dictionary<string, string>();
            if (currentRequest.ContentLength64 == 0) return form;
            using (var reader = new StreamReader(currentRequest.InputStream,
                currentRequest.ContentEncoding))
            {
                var between = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(reader.ReadToEnd());
                form = between["body"];
            }

            return form; 
        }
    }
}