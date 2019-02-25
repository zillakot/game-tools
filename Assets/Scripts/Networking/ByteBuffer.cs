using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ByteBuffer : IDisposable
{
    private List<byte> Buff;
    private byte[] readBuff;
    private int readPos;
    private bool buffUpdated = true;

    public ByteBuffer()
    {
        Buff = new List<byte>();
        readPos = 0;
    }

    public int GetReadPos()
    {
        return readPos;
    }

    public byte[] ToArray()
    {
        return Buff.ToArray();
    }

    public int Count()
    {
        return Buff.Count();
    }

    public int Length()
    {
        return Count() - readPos;
    }

    public void Clear(){
        Buff.Clear();
        buffUpdated = true;
    }
    public void WriteByte(byte input)
    {
        Buff.Add(input);
        buffUpdated = true;
    }
    public void WriteBytes(byte[] input)
    {
        Buff.AddRange(input);
        buffUpdated = true;
    }

    public void WriteLong(int input){
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }

    public void WriteInteger(int input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }

    public void WriteFloat(float input){
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }

    public void WriteBool(bool input){
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }
    public void WriteString(string input){
        Buff.AddRange(BitConverter.GetBytes(input.Length));
        Buff.AddRange(Encoding.UTF8.GetBytes(input));
        buffUpdated = true;
    }
    public byte ReadByte(bool Peek = true)
    {
        if(Buff.Count > readPos){
            if(buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            byte value = readBuff[readPos];
            if(Peek & Buff.Count > readPos)
            {
                readPos += 1;
            }

            return value;
        }
        else
        {
            throw new Exception("Type is not a byte");
        }
    }
    public byte[] ReadBytes(int Length, bool Peek = true)
    {
        if(Buff.Count > readPos){
            if(buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            byte[] value = Buff.GetRange(readPos, Length).ToArray();
            if(Peek)
            {
                readPos += Length;
            }

            return value;
        }
        else
        {
            throw new Exception("Type is not a byte array");
        }
    }
    public short ReadShort(bool Peek = true)
    {
        if(Buff.Count > readPos){
            if(buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            short value = BitConverter.ToInt16(readBuff, readPos);
            if(Peek  & Buff.Count > readPos)
            {
                readPos += 2;
            }

            return value;
        }
        else
        {
            throw new Exception("Type is not a short");
        }
    }
    public int ReadInteger(bool Peek = true)
    {
        if(Buff.Count > readPos){
            if(buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            int value = BitConverter.ToInt32(readBuff, readPos);
            if(Peek  & Buff.Count > readPos)
            {
                readPos += 4;
            }

            return value;
        }
        else
        {
            throw new Exception("Type is not a int");
        }
    }
    public long ReadLong(bool Peek = true)
    {
        if(Buff.Count > readPos){
            if(buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            long value = BitConverter.ToInt64(readBuff, readPos);
            if(Peek  & Buff.Count > readPos)
            {
                readPos += 8;
            }

            return value;
        }
        else
        {
            throw new Exception("Type is not a long");
        }
    }
    public float ReadFloat(bool Peek = true)
    {
        if(Buff.Count > readPos){
            if(buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            float value = BitConverter.ToSingle(readBuff, readPos);
            if(Peek  & Buff.Count > readPos)
            {
                readPos += 4;
            }

            return value;
        }
        else
        {
            throw new Exception("Type is not a single");
        }
    }
    public bool ReadBool(bool Peek = true)
    {
        if(Buff.Count > readPos){
            if(buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            bool value = BitConverter.ToBoolean(readBuff, readPos);
            if(Peek  & Buff.Count > readPos)
            {
                readPos += 1;
            }

            return value;
        }
        else
        {
            throw new Exception("Type is not a bool");
        }
    }
    public string ReadString(bool Peek = true)
    {
        try{
            int length = ReadInteger(true);
            if(buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            string value = Encoding.UTF8.GetString(readBuff, readPos, length);
            if(Peek  & Buff.Count > readPos)
            {
                if(value.Length > 0)
                    readPos += length;
            }
            
            return value;
        }
        catch
        {
            throw new Exception("Type is not a string");
        }
    }

    private bool disposedValue = false;
    protected virtual void Dispose(bool disposing)
    {
        if(!disposedValue){
            if(disposing){
                Buff.Clear();
                readPos = 0;
            }
            disposedValue = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);
    }
}
