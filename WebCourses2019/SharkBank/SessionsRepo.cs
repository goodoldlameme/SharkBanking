using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
// ReSharper disable StringLiteralTypo

namespace SharkBank
{
    public static class SessionsRepo
    { 
        private static Dictionary<string, (DateTime, string, string)> sessions = new Dictionary<string, (DateTime, string, string)>();

        public static (string, DateTime, string) AddSession(string username)
        {
            var session = $"{GenerateSecurString()}{username}";
            var secret = GenerateSecurString();
            var expiration = DateTime.Now + TimeSpan.FromMinutes(30);
            // ReSharper disable once HeapView.BoxingAllocation
            var sessionHash = ComputeSha($"{session}|{secret}");
            var csrfToken = GenerateSecurString();
            var tokenHash = ComputeSha($"{csrfToken}|{secret}");
            sessions.Add(session, (expiration, csrfToken, secret));
            return ($"{session}|{sessionHash}", expiration, $"{csrfToken}|{tokenHash}");
        }
        
        private static string GenerateSecurString() {
            var res = new byte[32];
            using (var rnd = new RNGCryptoServiceProvider()) {
                rnd.GetBytes(res);
            }
            return Convert.ToBase64String(res);
        }

        public static bool CheckCookieSignature(string session, string signature)
        {
            if (!sessions.TryGetValue(session, out var secret)) return false;
            return ComputeSha($"{session}|{secret.Item3}") == signature;
        }

        public static bool CheckFormToken(string session, string token)
        {
            var info = sessions[session];
            var splitted = token.Split("|");
            return splitted.Length == 2 && info.Item2 == splitted[0]
                && ComputeSha($"{splitted[0]}|{info.Item3}") == splitted[1];
        }

        private static string ComputeSha(string rawData)
        {
            using (var sha256Hash = SHA256.Create())  
            {  
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));  
  
                var builder = new StringBuilder();  
                foreach (var t in bytes)
                    builder.Append(t.ToString("x2"));
                
                return builder.ToString();  
            }  
        }
    }
}