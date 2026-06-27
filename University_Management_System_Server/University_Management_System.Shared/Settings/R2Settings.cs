namespace University_Management_System.Shared.Settings
{
    public class R2Settings
    {
        public string BucketName { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public bool EnableSsl { get; set; } = true;
        public int TimeoutMinutes { get; set; } = 5;
    }
}