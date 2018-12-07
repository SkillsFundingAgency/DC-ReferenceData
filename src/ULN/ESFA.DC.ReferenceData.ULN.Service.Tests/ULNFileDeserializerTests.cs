using ESFA.DC.ReferenceData.ULN.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ReferenceData.ULN.Service.Tests
{
    public class ULNFileDeserializerTests
    {
        [Fact]
        public void Deserialize_Success()
        {
            using (var stream = GetStream("Files/Success.txt"))
            {
                var ulnFile = NewDeserializer().Deserialize(stream);

                ulnFile.Count.Should().Be(10);

                ulnFile.ULNs.Should().HaveCount(10);
                ulnFile.ULNs.Should().ContainInOrder
                (
                    1234567890,
                    2345678901,
                    3456789012,
                    4567890123,
                    5678901234,
                    6789012345,
                    7890123456,
                    8901234567,
                    9012345678,
                    1234567890
                );
            }
        }

        [Theory]
        [InlineData("Files/Fail_NoHeader.txt")]
        [InlineData("Files/Fail_NotLong.txt")]
        public void Deserialize_Fail(string filePath)
        {
            using (var stream = GetStream(filePath))
            {
                Action action = () => NewDeserializer().Deserialize(stream);

                action.Should().Throw<SerializationException>();
            }
        }


        private Stream GetStream(string filePath)
        {
            return File.OpenRead(filePath);
        }

        private ULNFileDeserializer NewDeserializer()
        {
            return new ULNFileDeserializer();
        }
    }
}
