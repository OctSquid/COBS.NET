using System.Collections.Generic;

using Xunit;

namespace COBS.NET.Tests
{
    public class EncodeTests
    {
        /// <summary>
        /// Test data for COBS encoding and decoding.
        /// </summary>
        private readonly COBSExample[] TestCases = {
            new(new byte[] {  0x00 }, new byte[] { 0x01, 0x01, 0x00 }),
            new(new byte[] {  0x00, 0x00 }, new byte[] { 0x01, 0x01, 0x01, 0x00 }),
            new(new byte[] {  0x00, 0x11, 0x00 }, new byte[] {  0x01, 0x02, 0x11, 0x01, 0x00 }),
            new(new byte[] {  0x11, 0x22, 0x00, 0x33 }, new byte[] {  0x03, 0x11, 0x22, 0x02, 0x33, 0x00 }),
            new(new byte[] {  0x03, 0x04, 0x05, 0xFF, 0x00, 0x01 }, new byte[] {  0x05, 0x03, 0x04, 0x05, 0xFF, 0x02, 0x01, 0x00 })
        };

        /// <summary>
        /// Tests the COBS encoding.
        /// </summary>
        /// <param name="index"></param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void EncodeTest(int index)
        {
            // Act
            var encodedData = COBS.Encode(TestCases[index].Unencoded, true);

            // Assert
            Assert.Equal(TestCases[index].Encoded, encodedData);
        }


        /// <summary>
        /// Tests the COBS decoding.
        /// </summary>
        /// <param name="index"></param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void DecodeTest(int index)
        {
            List<byte> encodedData = new(TestCases[index].Encoded);

            // Act
            var decodedData = COBS.Decode(encodedData.ToArray(), true);

            // Assert
            Assert.Equal(TestCases[index].Unencoded, decodedData);
        }
    }
}
