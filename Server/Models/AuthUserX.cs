using AliaaCommon.Models;
using EasyMongoNet;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Linq;
using TciCommon.Models;
using TciPM.Blazor.Shared.Models;

namespace TciPM.Blazor.Server.Models
{
    [CollectionOptions(Name = nameof(AuthUser))]
    [CollectionSave(WriteLog = true)]
    [CollectionIndex(Fields: new string[] { nameof(Username) }, Unique = true)]
    [BsonIgnoreExtraElements]
    public class AuthUserX : VisibleAuthUser, IMongoEntity
    {
        public string HashedPassword { get; set; }

        [BsonIgnore]
        public string Password
        {
            set
            {
                HashedPassword = AuthUserDBExtention.GetHash(value);
            }
        }

        public static AuthUserX CheckAuthentication(IDbContext db, string username, string password, bool passwordIsHashed = false)
        {
            string hash;
            if (passwordIsHashed)
                hash = password;
            else
                hash = AuthUserDBExtention.GetHash(password);
            string ip = null; //TODO
            AuthUserX user = db.Find<AuthUserX>(u => u.Username == username && u.HashedPassword == hash && u.Disabled != true).FirstOrDefault();

            if (user != null && (string.IsNullOrEmpty(user.RestrictedIP) || ip == user.RestrictedIP))
            {
                db.Save(new LoginLog { Sucess = true, UserId = user.Id, Username = user.Username, IP = ip });
                return user;
            }

            db.Save(new LoginLog { Sucess = false, Username = username, IP = ip });
            return null;
        }

        public static AuthUserX GetByUsername(IReadOnlyDbContext db, string username)
        {
            return db.Find<AuthUserX>(u => u.Username == username).FirstOrDefault();
        }
    }
}