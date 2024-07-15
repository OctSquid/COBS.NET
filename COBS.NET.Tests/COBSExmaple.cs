namespace COBS.NET.Tests
{
    public class COBSExample
    {
        public byte[] Unencoded;
        public byte[] Encoded;

        public COBSExample(byte[] unencoded, byte[] encoded)
        {
            Encoded = encoded;
            Unencoded = unencoded;
        }

        public override string ToString()
        {
            return $"({Unencoded}, {Encoded})";
        }
    }
}
