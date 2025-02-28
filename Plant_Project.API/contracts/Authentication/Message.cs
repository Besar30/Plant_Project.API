namespace Plant_Project.API.contracts.Authentication
{
    public class Message
    {
        public List<string> To { get; }
        public string Subject { get; }
        public string Content { get; }
        public byte[] Attachment { get; }

        public Message(IEnumerable<string> to, string subject, string content, byte[] attachment = null)
        {
            To = to.ToList();
            Subject = subject;
            Content = content;
            Attachment = attachment;
        }
    }
}
