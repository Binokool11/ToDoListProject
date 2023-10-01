using CsvHelper;
using CsvHelper.Configuration;
using System.Collections;
using System.Text;

namespace ToDoList.Services.Services
{
    public class CsvBaseService<T>
    {
        private readonly CsvConfiguration _configuration;

        public CsvBaseService()
        {
            _configuration = GetConfiguration();
        }

        public byte[] UploadFile(IEnumerable data)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, _configuration))
            {
                csvWriter.WriteRecords(data);
                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }

        private CsvConfiguration GetConfiguration()
        {
            return new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                Encoding = Encoding.UTF8, 
                NewLine = "\r\n"
            };

        }
    }
}
