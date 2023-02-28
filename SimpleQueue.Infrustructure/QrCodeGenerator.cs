using Net.Codecrete.QrCodeGenerator;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Infrastructure
{
    public class QrCodeGenerator : IQrCodeGenerator
    {
        public Task<string> GenerateQrCode(string url)
        {
            var qr = QrCode.EncodeText(url, QrCode.Ecc.Medium);

            var svgResult = qr.ToSvgString(64);

            return Task.FromResult(svgResult);
        }
    }
}
