namespace Plant_Project.API.Errors
{
    public static class PostErrors
    {
        public static readonly Error NullPost =
            new Error("You must provider either content or an image", "You must provider either content or an image");

        public static readonly Error PostNotFound =
        new Error("Post Not Found", "Post Not Found");
        public static readonly Error PostCanNotBeDeleted =
    new Error("PostDeletionFailed", "Only the post owner or an admin is authorized to delete this post.");


    }
}
