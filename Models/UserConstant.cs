namespace webAPI.JWTTOken.Models
{
    public class UserConstant
    {
        public static List<UserModel> Users = new()
            {
                    new UserModel(){ Username="naeem",Password="naeem_admin",Role="Admin"}
            };
    }
}
