using System.Security.Cryptography;
using System.Text;
using Jose;
using Microsoft.VisualBasic;

namespace WebApp.Library.Extension;

public static class CryptoExtension
{
    public static string Md5(this string str)
    {
        using var provider = MD5.Create();
        var builder = new StringBuilder();
        foreach (var b in provider.ComputeHash(Encoding.UTF8.GetBytes(str)))
            builder.Append(b.ToString("x2").ToLower());

        return builder.ToString();
    }
    
    public static string EncodeBase64(this string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        return Convert.ToBase64String(bytes);
    }

    public static string DecodeBase64(this string str)
    {
        var bytes = Convert.FromBase64String(str);
        return Encoding.UTF8.GetString(bytes);
    }
    
    private static string GetStringFromHash(byte[] hash)
    {
        var result = new StringBuilder();
        foreach (var t in hash)
            result.Append(t.ToString("X2"));

        return result.ToString();
    }
    
    public static string Sha256(this string str)
    {
        var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(str);
        var hash = sha256.ComputeHash(bytes);
        return GetStringFromHash(hash);
    }
    
    public static string Sha512(this string str)
    {
        var sha512 = SHA512.Create();
        var bytes = Encoding.UTF8.GetBytes(str);
        var hash = sha512.ComputeHash(bytes);
        return GetStringFromHash(hash);
    }

    public static string Encrypt(this string str, string securityKey, bool useHashing = true)
    {
        byte[] keyArray;
        var toEncryptArray = Encoding.UTF8.GetBytes(str);
        if (useHashing)
        {
            var hashMd5 = new MD5CryptoServiceProvider();
            keyArray = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(securityKey));
            hashMd5.Clear();
        }
        else
            keyArray = Encoding.UTF8.GetBytes(securityKey);
        
        var tdes = new TripleDESCryptoServiceProvider
        {
            Key = keyArray,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };

        var cTransform = tdes.CreateEncryptor();
        var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        tdes.Clear();
        
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    [Obsolete("Obsolete")]
    public static string Decrypt(this string str, string securityKey, bool useHashing = true)
    {
        byte[] keyArray;
        var toEncryptArray = Convert.FromBase64String(str);
        if (useHashing)
        {
            var hashMd5 = new MD5CryptoServiceProvider();
            keyArray = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(securityKey));
            hashMd5.Clear();
        }
        else
            keyArray = Encoding.UTF8.GetBytes(securityKey);
        
        var tdes = new TripleDESCryptoServiceProvider
        {
            Key = keyArray,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        
        var cTransform = tdes.CreateDecryptor();
        var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        tdes.Clear();
        return Encoding.UTF8.GetString(resultArray);
    }
    
    public static string HashByBcrypt(this string str)
    {
        return BCrypt.Net.BCrypt.HashPassword(str.ToLower());
    }
        
    public static bool VerifyBcrypt(this string str, string bcryptStr)
    {
        return !string.IsNullOrEmpty(str) && BCrypt.Net.BCrypt.Verify(str.ToLower(), bcryptStr);
    }
    
    public static string JwtEncode(this string str, string hashKey, JwsAlgorithm alg = JwsAlgorithm.HS256)
    {
        return string.IsNullOrEmpty(str) ? string.Empty : JWT.Encode(str, Encoding.UTF8.GetBytes(hashKey), alg);
    }
        
    public static string JwtDecode(this string str, string hashKey, JwsAlgorithm alg = JwsAlgorithm.HS256)
    {
        return string.IsNullOrEmpty(str) ? string.Empty : JWT.Decode(str, Encoding.UTF8.GetBytes(hashKey), alg);
    }
}