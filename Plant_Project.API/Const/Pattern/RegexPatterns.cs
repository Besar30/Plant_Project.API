namespace Plant_Project.API.Const.Pattern
{
    public  static class RegexPatterns
    {
        public const string Password = "^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@#$%^&*!])[A-Za-z\\d@#$%^&*!]{8,}$";
    }
}
