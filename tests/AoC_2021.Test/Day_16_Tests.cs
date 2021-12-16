using NUnit.Framework;

namespace AoC_2021.Test;

public class Day_16_Tests
{
    [TestCase("D2FE28", "110100101111111000101000")]
    [TestCase("38006F45291200", "00111000000000000110111101000101001010010001001000000000")]
    public void HexadecimalToBinary(string input, string expectedOutput)
    {
        Assert.AreEqual(expectedOutput, Day_16.HexadecimalToBinary(input));
    }

    [TestCase("110100101111111000101000", 6, 4, 2021)]
    public void ParsePackage_Literal(string input, int version, int typeId, int value)
    {
        var result = Day_16.ParsePacketFromBinary(input).Packet;
        Assert.AreEqual(version, result.Version);
        Assert.AreEqual(typeId, result.TypeId);
        Assert.AreEqual(value, result.Value);
    }

    [Test]
    public void ParsePackage_Operation_TotalPackageBits()
    {
        var result = Day_16.ParsePacketFromBinary("00111000000000000110111101000101001010010001001000000000").Packet;
        Assert.AreEqual(2, result.SubPackets.Count);

        Assert.AreEqual(10, result.SubPackets[0].Value);
        Assert.AreEqual(20, result.SubPackets[1].Value);
    }

    [Test]
    public void ParsePackage_Operation_PackageCount()
    {
        var result = Day_16.ParsePacketFromBinary("11101110000000001101010000001100100000100011000001100000").Packet;
        Assert.AreEqual(3, result.SubPackets.Count);

        Assert.AreEqual(1, result.SubPackets[0].Value);
        Assert.AreEqual(2, result.SubPackets[1].Value);
        Assert.AreEqual(3, result.SubPackets[2].Value);
    }

    [TestCase("8A004A801A8002F478", 16)]
    [TestCase("620080001611562C8802118E34", 12)]
    [TestCase("C0015000016115A2E0802F182340", 23)]
    [TestCase("A0016C880162017C3686B18A3D4780", 31)]
    public void VersionSum(string hexadecimalString, int expectedVersionSum)
    {
        Assert.AreEqual(expectedVersionSum, Day_16.ParsePacketFromHexadecimal(hexadecimalString).VersionSum());
    }

    [TestCase("C200B40A82", 3)]
    [TestCase("04005AC33890", 54)]
    [TestCase("880086C3E88112", 7)]
    [TestCase("CE00C43D881120", 9)]
    [TestCase("D8005AC2A8F0", 1)]
    [TestCase("F600BC2D8F", 0)]
    [TestCase("9C005AC2F8F0", 0)]
    [TestCase("9C0141080250320F1802104A08", 1)]
    public void Operate(string hexadecimalString, int expectedResult)
    {
        Assert.AreEqual(expectedResult, Day_16.ParsePacketFromHexadecimal(hexadecimalString).Operate());
    }
}
