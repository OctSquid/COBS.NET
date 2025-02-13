using System;
using System.Collections.Generic;

namespace COBS.NET
{
    public static class COBS
    {
        /// <summary>
        /// Encodes the given data using COBS.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <param name="addZeroByte">If true, appends a zero byte at the end of the encoded data.</param>
        /// <returns>The COBS-encoded byte array.</returns>
        public static byte[] Encode(byte[] data, bool addZeroByte = true)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Data to encode cannot be null or empty.", nameof(data));
            }

            var result = new List<byte>(data.Length + 1);
            var blockStartIndex = 0;

            for (var i = 0; i < data.Length; i++)
            {
                if (data[i] == 0)
                {
                    while (i - blockStartIndex >= 254)
                    {
                        result.Add(0xFF);
                        var block = new byte[254];
                        Array.Copy(data, blockStartIndex, block, 0, 254);
                        result.AddRange(block);
                        blockStartIndex += 254;
                    }

                    result.Add((byte)(i - blockStartIndex + 1));
                    var block = new byte[i - blockStartIndex];
                    Array.Copy(data, blockStartIndex, block, 0, block.Length);
                    result.AddRange(block);
                    blockStartIndex = i + 1;
                }
            }

            while (data.Length - blockStartIndex >= 254)
            {
                result.Add(0xFF);
                var block = new byte[254];
                Array.Copy(data, blockStartIndex, block, 0, 254);
                result.AddRange(block);
                blockStartIndex += 254;
            }

            result.Add((byte)(data.Length - blockStartIndex + 1));
            var finalBlock = new byte[data.Length - blockStartIndex];
            Array.Copy(data, blockStartIndex, finalBlock, 0, finalBlock.Length);
            result.AddRange(finalBlock);

            if (addZeroByte)
            {
                result.Add(0);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Decodes the given COBS-encoded data.
        /// </summary>
        /// <param name="data">The COBS-encoded data to decode.</param>
        /// <param name="withZeroByte">If true, removes the trailing zero byte from the decoded data.</param>
        /// <returns>The decoded byte array.</returns>
        /// <exception cref="ArgumentException">Thrown when the COBS encoded data is invalid.</exception>
        public static byte[] Decode(byte[] data, bool withZeroByte = false)
        {
            var result = new List<byte>();
            var blockStartIndex = 0;
            byte distance;

            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Data to decode cannot be null or empty.", nameof(data));
            }

            // Check if the data is COBS encoded
            if (withZeroByte)
            {
                if (data[data.Length - 1] != 0x00) {
                    throw new ArgumentException("Invalid COBS encoded data.", nameof(data));
                }
            }

            // Remove the trailing zero byte if it exists
            if (data[data.Length - 1] == 0x00)
            {
                var temp = new byte[data.Length - 1];
                Array.Copy(data, temp, data.Length - 1);
                data = temp;
            }

            while (blockStartIndex < data.Length)
            {
                distance = data[blockStartIndex];

                if (data.Length < blockStartIndex + distance || distance < 1)
                {
                    throw new ArgumentException("Invalid COBS encoded data.", nameof(data));
                }

                if (distance > 1)
                {
                    for (byte i = 1; i < distance; i++)
                        result.Add(data[blockStartIndex + i]);
                }

                blockStartIndex += distance;

                if (distance < 0xFF && blockStartIndex < data.Length)
                    result.Add(0);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Encodes the given data using COBS.
        /// </summary>
        /// <param name="data">The data to encode, as a List of bytes.</param>
        /// <param name="addZeroByte">If true, appends a zero byte at the end of the encoded data.</param>
        /// <returns>The COBS-encoded byte array.</returns>
        public static byte[] Encode(List<byte> data, bool addZeroByte = true)
        {
            return Encode(data.ToArray(), addZeroByte);
        }

        /// <summary>
        /// Decodes the given COBS-encoded data.
        /// </summary>
        /// <param name="data">The COBS-encoded data to decode, as a List of bytes.</param>
        /// <param name="withZeroByte">If true, removes the trailing zero byte from the decoded data.</param>
        /// <returns>The decoded byte array.</returns>
        public static byte[] Decode(List<byte> data, bool withZeroByte = false)
        {
            return Decode(data.ToArray(), withZeroByte);
        }
    }
}
