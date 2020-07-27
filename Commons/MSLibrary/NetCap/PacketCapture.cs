using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.NetCap
{
    /// <summary>
    /// 表示捕获到的包。
    /// </summary>
    public sealed class PacketCapture
    {
        private readonly UnixTime _Timestamp;
        private readonly ArraySegment<byte> _Packet;
        private readonly int _RawLength;
        private readonly int _Millseconds;

        /// <summary>
        /// 构造一个捕获包
        /// </summary>
        /// <param name="packet">包</param>
        /// <param name="rawLength">原始长度</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="millseconds">时间戳的微秒值</param>
        public PacketCapture(ArraySegment<byte> packet, int rawLength, UnixTime timestamp, int millseconds)
        {
            if (packet.Count > rawLength)
                throw new ArgumentException("包内容长度不能大于原始长度。", "rawLength");
            _Packet = packet;
            _Timestamp = timestamp;
            _RawLength = rawLength;
            _Millseconds = millseconds;
        }

        /// <summary>
        /// 构造一个捕获包
        /// </summary>
        /// <param name="packet">包</param>
        /// <param name="rawLength">原始长度</param>
        /// <param name="timestamp">时间戳</param>
        public PacketCapture(ArraySegment<byte> packet, int rawLength, DateTime timestamp)
            : this(packet, rawLength, UnixTime.FromDateTime(timestamp), 0)
        {
        }

        /// <summary>
        /// 构造一个捕获包
        /// </summary>
        /// <param name="packet">包</param>
        /// <param name="rawLength">原始长度</param>
        public PacketCapture(ArraySegment<byte> packet, int rawLength)
            : this(packet, rawLength, UnixTime.FromDateTime(DateTime.Today), 0)
        {
        }

        /// <summary>
        /// 构造一个捕获包
        /// </summary>
        /// <param name="packet">包</param>
        public PacketCapture(ArraySegment<byte> packet)
            : this(packet, packet.Count)
        {
        }

        /// <summary>
        /// 构造一个捕获包
        /// </summary>
        /// <param name="packetData">包数据</param>
        /// <param name="offset">数据的偏移。</param>
        /// <param name="count">数据的大小。</param>
        /// <param name="rawLength">原始长度</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="millseconds">时间戳的微秒值</param>
        public PacketCapture(byte[] packetData, int offset, int count, int rawLength, UnixTime timestamp, int millseconds)
            : this(new ArraySegment<byte>(packetData, offset, count), rawLength, timestamp, millseconds)
        {
        }

        /// <summary>
        /// 构造一个捕获包
        /// </summary>
        /// <param name="packetData">包数据</param>
        /// <param name="offset">数据的偏移。</param>
        /// <param name="count">数据的大小。</param>
        /// <param name="rawLength">原始长度</param>
        /// <param name="timestamp">时间戳</param>
        public PacketCapture(byte[] packetData, int offset, int count, int rawLength, DateTime timestamp)
            : this(new ArraySegment<byte>(packetData, offset, count), rawLength, UnixTime.FromDateTime(timestamp), 0)
        {
        }

        /// <summary>
        /// 构造一个捕获包
        /// </summary>
        /// <param name="packetData">包数据</param>
        /// <param name="rawLength">原始长度</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="millseconds">时间戳的微秒值</param>
        public PacketCapture(byte[] packetData, int rawLength, UnixTime timestamp, int millseconds)
            : this(new ArraySegment<byte>(packetData), rawLength, timestamp, millseconds)
        {
        }

        /// <summary>
        /// 构造一个捕获包
        /// </summary>
        /// <param name="packetData">包数据</param>
        /// <param name="rawLength">原始长度</param>
        /// <param name="timestamp">时间戳</param>
        public PacketCapture(byte[] packetData, int rawLength, DateTime timestamp)
            : this(new ArraySegment<byte>(packetData), rawLength, UnixTime.FromDateTime(timestamp), 0)
        {
        }

        /// <summary>
        /// 构造一个捕获包
        /// </summary>
        /// <param name="packetData">包数据</param>
        /// <param name="rawLength">原始长度</param>
        public PacketCapture(byte[] packetData, int rawLength)
            : this(new ArraySegment<byte>(packetData), rawLength, UnixTime.FromDateTime(DateTime.Today), 0)
        {
        }

        /// <summary>
        /// 构造一个捕获包
        /// </summary>
        /// <param name="packetData">包数据</param>
        public PacketCapture(byte[] packetData)
            : this(packetData, packetData.Length)
        {
        }

        /// <summary>
        /// 数据包内容
        /// </summary>
        public ArraySegment<byte> Packet
        {
            get { return _Packet; }
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        public UnixTime Timestamp
        {
            get { return _Timestamp; }
        }

        /// <summary>
        /// 表示时间戳的微秒
        /// </summary>
        public int Millseconds
        {
            get { return _Millseconds; }
        }

        /// <summary>
        /// 原始长度
        /// </summary>
        public int RawLength
        {
            get { return _RawLength; }
        }

    }
}
