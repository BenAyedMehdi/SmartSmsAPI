namespace SmartSms.Service
{
    public interface IGptService
    {
        public Task<string> GenerateTextAsync(string input);
    }
}
