namespace webAPI.JWTTOken.Models
{
    public class UserConstant
    {
        public static List<UserModel> Users = new()
            {
                    new UserModel(){ Username="Test",Password="Admin",Role="Admin"}
            };
    }
}
