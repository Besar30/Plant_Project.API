namespace Plant_Project.API.Errors
{
    public static class CommentErrors
    {
        public static readonly Error CommentNotFound =
          new Error("Comment Not Found", "Comment Not Found");


        public static readonly Error CommentCanNotBeDeleted =
      new Error("CommentDeletionFailed", "You are not authorized to delete this comment. You can only delete your own comments or comments on your posts or Admin.");

    }
}
