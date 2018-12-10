using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ESFA.DC.ReferenceData.ULN.Model.ULN;
using ESFA.DC.ReferenceData.ULN.Service.Interface;

namespace ESFA.DC.ReferenceData.ULN.Service
{
    public class UlnFileDeserializer : IUlnFileDeserializer
    {
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

                ulnFile.ULNs = ulns;
            }

            return ulnFile;
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
