namespace InventoryApiProject.Auth
{
    public static class UserConstants
    {
        public static List<UserModel> Users = new()
        {
            new UserModel()
            {
                Username = "admin",
                Password = "admin",
                Email = "admin@inventory.com",
                FullName = "System Admin",
                Role = "Administrator"
            },

            new UserModel()
            {
                Username = "staff",
                Password = "staff",
                Email = "staff@inventory.com",
                FullName = "Staff User",
                Role = "Staff"
            },
            new UserModel()
            {
                Username = "viewer",
                Password = "viewer",
                Email = "viewer@inventory.com",
                FullName = "Viewer User",
                Role = "Viewer"
            }
        };
    }
}