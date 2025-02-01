namespace Plant_Project.API.Const.Pattern
{
    public  static class RegexPattern
    {
        public const string pattern = "^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@#$%^&*!])[A-Za-z\\d@#$%^&*!]{8,}$";
    }
}
