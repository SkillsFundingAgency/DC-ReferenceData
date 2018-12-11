using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ESFA.DC.ReferenceData.ULN.Model.ULN;
using ESFA.DC.ReferenceData.ULN.Service.Interface;

namespace ESFA.DC.ReferenceData.ULN.Service
{
    public class UlnFileDeserializer : IUlnFileDeserializer
    {
        private const long MinimumULN = 1000000000;
        private const long MaximumULN = 9999999999;

        public UlnFile Deserialize(Stream stream)
        {
            var ulnFile = new UlnFile();

            using (var streamReader = new StreamReader(stream))
            {
                if (streamReader.Peek() >= 0)
                {
                    ulnFile.Count = StringToLong(streamReader.ReadLine());
                }

                var ulns = new List<long>();

                while (streamReader.Peek() >= 0)
                {
                    ulns.Add(StringToLong(streamReader.ReadLine()));
                }

                ulnFile.ULNs = ulns.Where(IsValidULN).ToList();
            }

            return ulnFile;
        }

        private bool IsValidULN(long uln)
        {
            return uln >= MinimumULN && uln <= MaximumULN;
        }

        private long StringToLong(string input)
        {
            if (!long.TryParse(input, out long result))
            {
                throw new SerializationException($"Failed To Deserialize ULN Exception, {input} not parseable as a long.");
            }

            return result;
        }
    }
}
