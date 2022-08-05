using CryptoAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntegrationTests.Helpers
{
    public class UserWithTokenConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(UserWithToken);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            User user = new User
            {
                UserId = (int)jo["userId"],
                RoleId = (short)jo["roleId"],
                Name = (string)jo["name"],
                Email = (string)jo["email"],
                Password = (string)jo["password"],
                ThemeId = (short)jo["themeId"],
                //CreatedAt = (DateTime)jo["createdAt"],
                //ReceiveNotifications = (bool)jo["receiveNotifications"],
            };
            UserWithToken userwithToken = new UserWithToken(user);
            userwithToken.AccessToken = (string)jo["accessToken"];
            userwithToken.RefreshToken = (string)jo["refreshToken"];
            return userwithToken;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
