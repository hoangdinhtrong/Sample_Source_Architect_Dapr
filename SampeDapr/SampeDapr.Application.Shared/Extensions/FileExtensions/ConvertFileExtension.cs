using DocumentFormat.OpenXml.Packaging;

namespace SampeDapr.Application.Shared.Extensions
{
    public class ConvertFileExtension
    {
        public MemoryStream GetFileTream(string inputFile)
        {
            OpenSettings settings = new()
            {
                RelationshipErrorHandlerFactory = package =>
                {
                    return new UriRelationShipErrorHandler();
                },
            };


            MemoryStream outputStream = new();
            using (FileStream fs = new FileStream(inputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fs.CopyTo(outputStream);
                fs.Position = 0;
                // UriFixer.FixInvalidUri(fs, brokenUri => FixUri(brokenUri));
            }

            using (var doc = SpreadsheetDocument.Open(outputStream, true, settings))
            {
                doc.Save();
            }
            outputStream.Position = 0;
            return outputStream;
        }
    }

    public class UriRelationShipErrorHandler : RelationshipErrorHandler
    {
        public override string Rewrite(Uri partUri, string? id, string? uri)
        {
            return "http://link-invalido";
        }
    }
}
