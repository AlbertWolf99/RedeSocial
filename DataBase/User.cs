using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RedeSocial.DataBase;

public class User
{
    static DB _db = new DB();
    public long UserId { get; set; }
    public string UserName { get; set; } = "";
    public Byte[] Password { get; set; } = new byte[]{};
    public string Email { get; set; } = "";
    public DateTime BirthDay { get; set; }

    public void Update()
    {
        _db.Update<User>(this);
        _db.SaveChanges();
    }

    public void Insert()
    {
        _db.Add<User>(this);
        _db.SaveChanges();
    }

    public void Delete()
    {
        _db.Remove<User>(this);
        _db.SaveChanges();
    }

    public static bool ExistingUserName(string userName)
    {
        return (from u in _db.Users where u.UserName.ToLower() == userName.ToLower() select u).Any();
    }

    public static byte[] GenerateSalt()
    {        
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        return salt;
    }

    public static bool MatchPasswordRequirements(string password)
    {    
        return Regex.IsMatch(password, "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{8,})");
    }

    public static User? FindUserByName(string userName)
    {
        return (from u in _db.Users where u.UserName.ToLower() == userName.ToLower() select u).FirstOrDefault();
    }

    public static bool Authenticate(string userName, string password)
    {
        var query = from u in _db.Users where u.UserName.ToLower() == userName.ToLower() select u;
        if(query.Any() && query.First().ValidatePassword(password))
        {
            return true;
        }
        return false;
    }

    public byte[] HashPassword(byte[] salt, string password)
    {
        
        List<byte> ret = new List<byte>();
        // Adicona os primeiros 128 bits do hash
        if(salt.Length != (128/8)) throw new ArgumentOutOfRangeException("Invalid Salt size");
        ret.AddRange(salt);
        // Adiciona a senha de 256 bits posterior;
        ret.AddRange(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 1000000, 256/8));
        return ret.ToArray();
    }

    public bool ValidatePassword(string password)
    {
        // Dados para comparativo
        byte[] salt = new byte[128/8];
        byte[] hash_db = new byte[256/8];
        byte[] hash_cmp = new byte[256/8];
        // Copia os 128 bits iniciais para o salt
        Array.Copy(this.Password, salt, 128/8);
        // Copia os 256 bits após o salt para o hash_db
        Array.Copy(this.Password, 128/8, hash_db, 0, 256/8);
        // Cria um hash da senha fornecida para comparacao e armazena no hash_cmp
        Array.Copy(HashPassword(salt, password), 128/8, hash_cmp, 0, 256/8);
        // Compara e retorna
        return Enumerable.SequenceEqual<byte>(hash_db, hash_cmp);

    }

}