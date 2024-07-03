using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

public class JwtHelpers
{
    private readonly IConfiguration Configuration;

    public JwtHelpers(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public string GenerateToken(string Name, int expireMinutes = 30)
    {
        var issuer = Configuration.GetValue<string>("Jwt:Issuer");
        var signKey = Configuration.GetValue<string>("Jwt:Key");
        var audience = Configuration.GetValue<string>("Jwt:Audience");

        var claims = new List<Claim>();


        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, Name));
     
        claims.Add(new Claim(ClaimTypes.Name, Name));
        // Token 的主體內容
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        // JWT ID
        //claims.Add(new Claim(JwtRegisteredClaimNames.Iss, issuer));
        claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds().ToString()));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()));

        claims.Add(new Claim("roles", "Admin"));
        claims.Add(new Claim("roles", "Users"));
        claims.Add(new Claim("roles", "123人"));

        var userClaimsIdentity = new ClaimsIdentity(claims);

        // 建立一組對稱式加密的金鑰，主要用於 JWT 簽章之用
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));

        // HmacSha256 MUST be larger than 128 bits, so the key can't be too short. At least 16 and more characters.
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // Create SecurityTokenDescriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            //Audience = issuer, 
            // 由於你的 API 受眾通常沒有區分特別對象，因此通常不太需要設定，也不太需要驗證
            //NotBefore = DateTime.Now, 預設值就是 DateTime.Now
            //IssuedAt = DateTime.Now, 預設值就是 DateTime.Now
            Subject = userClaimsIdentity,
            Expires = DateTime.Now.AddMinutes(expireMinutes),
            SigningCredentials = signingCredentials
        };

        // 產出所需要的 JWT securityToken 物件，並取得序列化後的 Token 結果(字串格式)
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var serializeToken = tokenHandler.WriteToken(securityToken);

        return serializeToken;
    }
}