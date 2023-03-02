namespace SimpleQueue.Domain.Interfaces
{
    public interface IQrCodeGenerator
    {
        Task<string> GenerateQrCodeAsync(string url);
    }
}
