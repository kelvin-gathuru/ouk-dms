using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DocumentManagement.API.Helpers;

public class OnlyOfficeTokenService
{
    private readonly string _secret = "This*Is&A!Long)Key(For%Creating@A$SymmetricKey"; // Same as Docker

    public string GenerateToken(object payload)
    {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(creds);
            var payloadJson = Newtonsoft.Json.Linq.JObject.FromObject(payload);
            var jwtPayload = new JwtPayload();

            foreach (var item in payloadJson)
            {
                if (item.Key != "documentType")
                {
                    jwtPayload.Add(item.Key, item.Value);
                }
            }
            var token = new JwtSecurityToken(header, jwtPayload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw ex;
        }
    }
}
