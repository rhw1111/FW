using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.Xrm.Message.FileAttributeDownloadChunking;

namespace MSLibrary.Xrm.IOExtensions
{
    /// <summary>
    /// 基于Crm文件块的流
    /// </summary>
    public class CrmFileBlocksStream:Stream
    {
        private string _entityName;
        private Guid _entityID;
        private string _fileAttributeName;
        private long _fileSize;
        private long _perSize = (long)1024 * 1024 * 3;
        private long _position = 0;
        private bool _complete = false;
        private Guid? _proxyUserId = null;
        private ICrmService _crmService;

        private List<byte> _currentBytes = new List<byte>();

        public CrmFileBlocksStream(string entityName,Guid entityID, string fileAttributeName, long fileSize, ICrmService crmService, Guid? proxyUserId)
        {
            _entityID = entityID;
            _fileAttributeName = fileAttributeName;
            _entityID = entityID;
            _fileSize = fileSize;
            _crmService = crmService;
            _proxyUserId = proxyUserId;
        }
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                return _fileSize;
            }
        }

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (offset < 0 || count < 0 || (offset + count > buffer.Count()))
            {
                throw new IndexOutOfRangeException("buffer out of count in Read of PAFileBlocksStream");
            }

            
            //本次操作总共读取的字节数
            List<byte> currentBytes = new List<byte>();
            //还需要从服务中读取的字节数
            int stillReadSize = 0;
            int resultSize = 0;
            if (_currentBytes.Count() >= count)
            {
               
                currentBytes.AddRange(_currentBytes.Take(count).ToList());
                _currentBytes.RemoveRange(0, count);
                resultSize = count;
            }
            else
            {
                currentBytes.AddRange(_currentBytes);
                _currentBytes.Clear();
                stillReadSize = count - currentBytes.Count();
                resultSize = currentBytes.Count();
            }


            while (!_complete && stillReadSize != 0)
            {

                CrmFileAttributeDownloadChunkingRequestMessage request = new CrmFileAttributeDownloadChunkingRequestMessage()
                {
                    EntityName = _entityName,
                    EntityId = _entityID,
                    AttributeName = _fileAttributeName,
                    Start = _position,
                    End = _position + _perSize,
                    ProxyUserId = _proxyUserId
                };

                
                var response=(CrmFileAttributeDownloadChunkingResponseMessage) await _crmService.Execute(request);

                
                
                _position = _position + response.Data.Count();
                _currentBytes.AddRange(response.Data);

                if (_currentBytes.Count < _perSize)
                {
                    _complete = true;
                }

                if (stillReadSize >= _currentBytes.Count())
                {
                    currentBytes.AddRange(_currentBytes);
                    stillReadSize = stillReadSize - _currentBytes.Count();
                    _currentBytes.Clear();
                }
                else
                {
                    currentBytes.AddRange(_currentBytes.Take(stillReadSize).ToList());
                    _currentBytes.RemoveRange(0, count);
                    stillReadSize = 0;
                }

                if (stillReadSize == 0 || _complete)
                {
                    break;
                }
            }

            currentBytes.CopyTo(buffer, offset);

            return currentBytes.Count;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
