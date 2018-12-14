using System.Collections.Generic;
using System.IO;
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
            var ulns = new List<long>();
            long headerCount = 0;
            
            using (var streamReader = new StreamReader(stream))
            {
                if (streamReader.Peek() >= 0)
                {
                    if (ConvertAndValidateHeader(streamReader.ReadLine(), out long count))
                    {
                        headerCount = count;
                    }
                }
                else
                {
                    throw new SerializationException("No Header Found in File.");
                }

                while (streamReader.Peek() >= 0)
                {
                    if (ConvertAndValidateUln(streamReader.ReadLine(), out long uln))
                    {
                        ulns.Add(uln);
                    }
                }
            }

            return new UlnFile()
            {
                Count = headerCount,
                ULNs = ulns,
            };
        }
        
        private bool ConvertAndValidateHeader(string input, out long header)
        {
            if (!long.TryParse(input, out long intermediateHeader))
            {
                throw new SerializationException($"Failed To Deserialize ULN header, {input} not parseable as a long.");
            }

            header = intermediateHeader;

            return true;
        }

        private bool ConvertAndValidateUln(string input, out long uln)
        {
            if (!long.TryParse(input, out long intermediateUln))
            {
                throw new SerializationException($"Failed To Deserialize ULN, {input} not parseable as a long.");
            }

            if (intermediateUln < MinimumULN && intermediateUln > MaximumULN)
            {
                throw new SerializationException($"ULN {intermediateUln} not between {MinimumULN} and {MaximumULN}.");
            }

            uln = intermediateUln;

            return true;
        }
    }
}
