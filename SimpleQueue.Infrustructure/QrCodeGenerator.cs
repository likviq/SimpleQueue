using Net.Codecrete.QrCodeGenerator;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Infrastructure
{
    public class QrCodeGenerator : IQrCodeGenerator
    {
        public Task<string> GenerateQrCodeAsync(string url)
        {
            var qr = QrCode.EncodeText(url, QrCode.Ecc.Medium);
            
            var borderSize = 4;
            var svgResult = qr.ToSvgString(borderSize);

            return Task.FromResult(svgResult);
        }
    }
}
