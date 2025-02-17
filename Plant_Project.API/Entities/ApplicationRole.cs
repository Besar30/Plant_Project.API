namespace Plant_Project.API.Entities
{
    public class ApplicationRole:IdentityRole
    {
        // defult Role =Member
        public bool IsDefault { get; set; }

        public bool IsDeleted { get; set; }
    }
}
