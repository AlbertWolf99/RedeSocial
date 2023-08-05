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

    /// <summary>
    /// Verifica se o nome de usuario ja existe.
    /// </summary>
    /// <param name="userName"> nome de usuario a ser verificado </param>
    /// <returns>
    /// True caso o nome ja exista, False caso contrario.
    /// </returns>
    public static bool ExistingUserName(string userName)
    {
        return (from u in _db.Users where u.UserName.ToLower() == userName.ToLower() select u).Any();
    }

    /// <summary>
    /// Gera o sal para a criptografia da senha.
    /// </summary>
    /// <returns>
    /// Um array de bytes aleatórios.
    /// </returns>
    public static byte[] GenerateSalt()
    {        
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        return salt;
    }

    /// <summary>
    /// Verifica se a senha segue os requisitos necessarios:
    /// - pelo menos 8 caracteres
    /// - uma letra minuscula
    /// - uma letra maiuscula
    /// - um numero
    /// - um caractere especial
    /// </summary>
    /// <param name="password"> senha a ser verificada. </param>
    /// <returns>
    /// True caso a senha siga os requisitos, False caso contrario.
    /// </returns>
    public static bool MatchPasswordRequirements(string password)
    {    
        return Regex.IsMatch(password, "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{8,})");
    }

    /// <summary>
    /// Busca um usuario pelo seu nome de usuario.
    /// </summary>
    /// <param name="userName"> nome de usuario a ser buscado. </param>
    /// <returns>
    /// O usuario se encontrado, null caso contrario.
    /// </returns>
    public static User? FindUserByName(string userName)
    {
        return (from u in _db.Users where u.UserName.ToLower() == userName.ToLower() select u).FirstOrDefault();
    }

    /// <summary>
    /// Autentica o login de um usuario
    /// </summary>
    /// <param name="userName"> nome do usuario tentando fazer login. </param>
    /// <param name="password"> senha do usuario tentando fazer login. </param>
    /// <returns>
    /// True caso o conjunta usuario e senha estejam corretos, False caso contrario.
    /// </returns>
    public static bool Authenticate(string userName, string password)
    {
        var query = from u in _db.Users where u.UserName.ToLower() == userName.ToLower() select u;
        if(query.Any() && query.First().ValidatePassword(password))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Aplica criptografia Hash a senha fornecida
    /// </summary>
    /// <param name="salt"> conjunto de bytes a ser usado para a criptografia. </param>
    /// <param name="password"> senha a ser criptografada. </param>
    /// <returns>
    /// Array de bytes criados a partir da criptografia da senha.
    /// </returns>
    public static byte[] HashPassword(byte[] salt, string password)
    {
        
        List<byte> ret = new List<byte>();
        // Adicona os primeiros 128 bits do hash
        if(salt.Length != (128/8)) throw new ArgumentOutOfRangeException("Invalid Salt size");
        ret.AddRange(salt);
        // Adiciona a senha de 256 bits posterior;
        ret.AddRange(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 1000000, 256/8));
        return ret.ToArray();
    }

    /// <summary>
    /// Verifica se a senha fornecida é igual a senha cadastrada do usuario.
    /// </summary>
    /// <param name="password"> senha a ser comparada. </param>
    /// <returns>
    /// True se a senha for igual a cadastrada, False caso contrario.
    /// </returns>
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