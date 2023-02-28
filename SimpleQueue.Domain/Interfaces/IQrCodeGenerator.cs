namespace SimpleQueue.Domain.Interfaces
{
    public interface IQrCodeGenerator
    {
        Task<string> GenerateQrCode(string url);
    }
}
