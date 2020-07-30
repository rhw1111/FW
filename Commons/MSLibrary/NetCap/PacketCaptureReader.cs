using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MSLibrary.NetCap
{
    /// <summary>
    /// Pcap 包捕获读取器。兼容 Tcpdump 及 Ethereal。
    /// </summary>
    public class PacketCaptureReader
    {
        #region Fields
        private const uint MAGIC = 0xA1B2C3D4;
        private readonly LinkLayerType _LinkLayerType;
        private readonly int _MaxPacketLength;
        private readonly Stream _BaseStream;
        private readonly BinaryReader m_Reader;
        private readonly int _TimeZone;
        private readonly int _CaptureTimestamp;

        #endregion

        #region Constructors
        /// <summary>
        /// 构造一个 Pcap 包捕获读取器。
        /// </summary>
        /// <param name="baseStream">要读取的基础流</param>
        public PacketCaptureReader(Stream baseStream)
        {
            if (baseStream == null) throw new ArgumentNullException("baseStream");
            if (!baseStream.CanRead) throw new ArgumentException("传入的流必须为可读。", "baseStream");
            _BaseStream = baseStream;
            m_Reader = new BinaryReader(_BaseStream);
            if (m_Reader.ReadUInt32() != MAGIC)
            {

            }
             //   throw new FormatException("无效的 PCAP 格式。");
            short versionMajor = m_Reader.ReadInt16();
            short versionMinjor = m_Reader.ReadInt16();
            /*if (versionMajor != 2 || VersionMinjor != 4)
            {
                throw new FormatException(string.Format("无法处理的 PCAP 版本 {0}.{1}", versionMajor, versionMinjor));
            }*/
            _TimeZone = m_Reader.ReadInt32();
            _CaptureTimestamp = m_Reader.ReadInt32();
            _MaxPacketLength = m_Reader.ReadInt32();
            _LinkLayerType = (LinkLayerType)m_Reader.ReadInt32();
        }

        #endregion

        #region Properties
        /// <summary>
        /// 主版本号，总是为 2
        /// </summary>
        public short VersionMajor
        {
            get { return 2; }
        }

        /// <summary>
        /// 子版本号，总是为 4
        /// </summary>
        public short VersionMinjor
        {
            get { return 4; }
        }

        /// <summary>
        /// 时区，该字段没有使用
        /// </summary>
        public int TimeZone
        {
            get { return _TimeZone; }
        }

        /// <summary>
        /// 捕获时的时间戳，该字段没有使用
        /// </summary>
        public int CaptureTimestamp
        {
            get { return _CaptureTimestamp; }
        }

        /// <summary>
        /// 基础流
        /// </summary>
        public Stream BaseStream
        {
            get { return _BaseStream; }
        }

        /// <summary>
        /// 数据链路类型
        /// </summary>
        public LinkLayerType LinkLaterType
        {
            get { return _LinkLayerType; }
        }

        /// <summary>
        /// 最大包大小
        /// </summary>
        public int MaxPacketLength
        {
            get { return _MaxPacketLength; }
        }

        #endregion

        /// <summary>
        /// 读取下一个捕获包
        /// </summary>
        /// <returns>读取的捕获包，如果已经到达尾端，返回 null</returns>
        public PacketCapture Read()
        {
            if (_BaseStream.Position == _BaseStream.Length) return null;
            UnixTime timestamp = new UnixTime(m_Reader.ReadInt32());
            int millseconds = m_Reader.ReadInt32();
            if (millseconds > 1000000)
                throw new InvalidDataException("读取到无效的数据格式。");
            int len = m_Reader.ReadInt32();
            int rawLen = m_Reader.ReadInt32();
            if (len > rawLen)
                throw new InvalidDataException("读取到无效的数据格式。");
            byte[] buff = m_Reader.ReadBytes(len);
            return new PacketCapture(buff, rawLen, timestamp, millseconds);
        }

    }
}
