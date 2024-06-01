/* AutoGenerate */

using System.IO;
using GameFramework;

namespace GameFramework
{
    public class Test2 : IDataTable
    {
        public int Id { get; private set; }
        public bool Item1 { get; private set; }
        public byte Item2 { get; private set; }
        public short Item3 { get; private set; }
        public int Item4 { get; private set; }
        public long Item5 { get; private set; }
        public decimal Item6 { get; private set; }
        public float Item7 { get; private set; }
        public double Item8 { get; private set; }
        public bool[] Item9 { get; private set; }
        public byte[] Item10 { get; private set; }
        public short[] Item11 { get; private set; }
        public int[] Item12 { get; private set; }
        public long[] Item13 { get; private set; }
        public decimal[] Item14 { get; private set; }
        public float[] Item15 { get; private set; }
        public double[] Item16 { get; private set; }

        public void Read(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            Item1 = reader.ReadBoolean();
            Item2 = reader.ReadByte();
            Item3 = reader.ReadInt16();
            Item4 = reader.ReadInt32();
            Item5 = reader.ReadInt64();
            Item6 = reader.ReadDecimal();
            Item7 = reader.ReadSingle();
            Item8 = reader.ReadDouble();
            int item9Length = reader.ReadInt32();
            Item9 = new bool[item9Length];
            for (int i = 0; i < item9Length; i++)
            {
                Item9[i] = reader.ReadBoolean();
            }

            int item10Length = reader.ReadInt32();
            Item10 = new byte[item10Length];
            for (int i = 0; i < item10Length; i++)
            {
                Item10[i] = reader.ReadByte();
            }

            int item11Length = reader.ReadInt32();
            Item11 = new short[item11Length];
            for (int i = 0; i < item11Length; i++)
            {
                Item11[i] = reader.ReadInt16();
            }

            int item12Length = reader.ReadInt32();
            Item12 = new int[item12Length];
            for (int i = 0; i < item12Length; i++)
            {
                Item12[i] = reader.ReadInt32();
            }

            int item13Length = reader.ReadInt32();
            Item13 = new long[item13Length];
            for (int i = 0; i < item13Length; i++)
            {
                Item13[i] = reader.ReadInt64();
            }

            int item14Length = reader.ReadInt32();
            Item14 = new decimal[item14Length];
            for (int i = 0; i < item14Length; i++)
            {
                Item14[i] = reader.ReadDecimal();
            }

            int item15Length = reader.ReadInt32();
            Item15 = new float[item15Length];
            for (int i = 0; i < item15Length; i++)
            {
                Item15[i] = reader.ReadSingle();
            }

            int item16Length = reader.ReadInt32();
            Item16 = new double[item16Length];
            for (int i = 0; i < item16Length; i++)
            {
                Item16[i] = reader.ReadDouble();
            }
        }

        public override string ToString()
        {
            return $"Id = {Id}; Item1 = {Item1}; Item2 = {Item2}; Item3 = {Item3}; Item4 = {Item4}; Item5 = {Item5}; Item6 = {Item6}; Item7 = {Item7}; Item8 = {Item8}; Item9 = {string.Join(", ", Item9)}; Item10 = {string.Join(", ", Item10)}; Item11 = {string.Join(", ", Item11)}; Item12 = {string.Join(", ", Item12)}; Item13 = {string.Join(", ", Item13)}; Item14 = {string.Join(", ", Item14)}; Item15 = {string.Join(", ", Item15)}; Item16 = {string.Join(", ", Item16)};";
        }
    }
}