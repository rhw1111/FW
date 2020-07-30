using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MSLibrary.NetCap
{
    /// <summary>
    /// Pcap 包捕获写入器。兼容 Tcpdump 及 Ethereal。
    /// </summary>
    public sealed class PacketCaptureWriter
    {
        #region Fields
        private const uint MAGIC = 0xA1B2C3D4;
        private readonly Stream _BaseStream;
        private readonly LinkLayerType _LinkLayerType;
        private readonly int _MaxPacketLength;
        private readonly BinaryWriter m_Writer;
        private bool m_ExistHeader = false;
        private int _TimeZone;
        private int _CaptureTimestamp;

        #endregion

        #region Constructors
        /// <summary>
        /// 构造一个 Pcap 包捕获写入器。
        /// </summary>
        /// <param name="baseStream">要写入的基础流</param>
        /// <param name="linkLayerType">数据链路类型。</param>
        /// <param name="maxPacketLength">最大包长度。</param>
        /// <param name="captureTimestamp">开始捕获时间。</param>
        public PacketCaptureWriter(
            Stream baseStream, LinkLayerType linkLayerType,
            int maxPacketLength, int captureTimestamp)
        {
            if (baseStream == null) throw new ArgumentNullException("baseStream");
            if (maxPacketLength < 0) throw new ArgumentOutOfRangeException("maxPacketLength");
            if (!baseStream.CanWrite) throw new ArgumentException("传入的流必须为可写。", "baseStream");
            _BaseStream = baseStream;
            _LinkLayerType = linkLayerType;
            _MaxPacketLength = maxPacketLength;
            _CaptureTimestamp = captureTimestamp;
            m_Writer = new BinaryWriter(_BaseStream);
        }

        /// <summary>
        /// 构造一个 Pcap 包捕获写入器。
        /// </summary>
        /// <param name="baseStream">要写入的基础流</param>
        /// <param name="linkLayerType">数据链路类型。</param>
        /// <param name="captureTimestamp">开始捕获时间。</param>
        public PacketCaptureWriter(Stream baseStream, LinkLayerType linkLayerType, int captureTimestamp)
            : this(baseStream, linkLayerType, 0xFFFF, captureTimestamp)
        {
        }

        /// <summary>
        /// 构造一个 Pcap 包捕获写入器。
        /// </summary>
        /// <param name="baseStream">要写入的基础流</param>
        /// <param name="linkLayerType">数据链路类型。</param>
        public PacketCaptureWriter(Stream baseStream, LinkLayerType linkLayerType)
            : this(baseStream, linkLayerType, 0xFFFF, UnixTime.FromDateTime(DateTime.Now).Value)
        {
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
            set { _TimeZone = value; }
        }

        /// <summary>
        /// 捕获时的时间戳，该字段没有使用
        /// </summary>
        public int CaptureTimestamp
        {
            get { return _CaptureTimestamp; }
            set { _CaptureTimestamp = value; }
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
        /// 写入一个捕获包
        /// </summary>
        /// <param name="packet">捕获包</param>
        public void Write(PacketCapture packet)
        {
            CheckHeader();
            m_Writer.Write(packet.Timestamp.Value);
            m_Writer.Write(packet.Millseconds);
            m_Writer.Write(packet.Packet.Count);
            m_Writer.Write(packet.RawLength);
            m_Writer.Write(packet.Packet.Array, packet.Packet.Offset, packet.Packet.Count);
        }

        /// <summary>
        /// 刷新基础流
        /// </summary>
        public void Flush()
        {
            BaseStream.Flush();
        }

        private void CheckHeader()
        {
            if (!m_ExistHeader)
            {
                m_Writer.Write(MAGIC);
                m_Writer.Write(VersionMajor);
                m_Writer.Write(VersionMinjor);
                m_Writer.Write(TimeZone);
                m_Writer.Write(CaptureTimestamp);
                m_Writer.Write(MaxPacketLength);
                m_Writer.Write((uint)LinkLaterType);
                m_ExistHeader = true;
            }
        }

    }
}
