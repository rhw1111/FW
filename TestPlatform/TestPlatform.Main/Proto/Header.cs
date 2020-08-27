// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Header.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ctrade.Message {

  /// <summary>Holder for reflection information generated from Header.proto</summary>
  public static partial class HeaderReflection {

    #region Descriptor
    /// <summary>File descriptor for Header.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static HeaderReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgxIZWFkZXIucHJvdG8SDmN0cmFkZS5tZXNzYWdlGg5zdGFuZGFyZC5wcm90",
            "byK/AQoGSGVhZGVyEhMKC21zZ19zZW5kX3RtGAEgASgFEhAKCG1zZ19zbmRy",
            "GAIgASgJEhEKCW1zZ19hY3B0chgDIAEoCRIVCg1zbmRyX2NtcG50X2lkGAQg",
            "ASgJEhMKC3NuZHJfc3ViX2lkGAUgASgJEg4KBm1zZ19jZBgGIAEoAxIPCgdz",
            "cnZjX2lkGAcgASgFEg0KBXRva2VuGAkgASgJEg4KBmVycl9jZBgKIAEoBRIP",
            "CgdlcnJfbXNnGAsgASgJQjAKIGNuLmNvbS5jZmV0cy5kYXRhLmN0cmFkZS5t",
            "ZXNzYWdlQgxIZWFkZXJQcm9idWY="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::StandardReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.Header), global::Ctrade.Message.Header.Parser, new[]{ "MsgSendTm", "MsgSndr", "MsgAcptr", "SndrCmpntId", "SndrSubId", "MsgCd", "SrvcId", "Token", "ErrCd", "ErrMsg" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Header : pb::IMessage<Header> {
    private static readonly pb::MessageParser<Header> _parser = new pb::MessageParser<Header>(() => new Header());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Header> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.HeaderReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Header() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Header(Header other) : this() {
      _hasBits0 = other._hasBits0;
      msgSendTm_ = other.msgSendTm_;
      msgSndr_ = other.msgSndr_;
      msgAcptr_ = other.msgAcptr_;
      sndrCmpntId_ = other.sndrCmpntId_;
      sndrSubId_ = other.sndrSubId_;
      msgCd_ = other.msgCd_;
      srvcId_ = other.srvcId_;
      token_ = other.token_;
      errCd_ = other.errCd_;
      errMsg_ = other.errMsg_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Header Clone() {
      return new Header(this);
    }

    /// <summary>Field number for the "msg_send_tm" field.</summary>
    public const int MsgSendTmFieldNumber = 1;
    private readonly static int MsgSendTmDefaultValue = 0;

    private int msgSendTm_;
    /// <summary>
    /// Sending Time, 应用层发送消息的时间timestamp格式
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MsgSendTm {
      get { if ((_hasBits0 & 1) != 0) { return msgSendTm_; } else { return MsgSendTmDefaultValue; } }
      set {
        _hasBits0 |= 1;
        msgSendTm_ = value;
      }
    }
    /// <summary>Gets whether the "msg_send_tm" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMsgSendTm {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "msg_send_tm" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMsgSendTm() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "msg_sndr" field.</summary>
    public const int MsgSndrFieldNumber = 2;
    private readonly static string MsgSndrDefaultValue = "";

    private string msgSndr_;
    /// <summary>
    ///消息发送方 {客户端、场务、API、adaptor}
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MsgSndr {
      get { return msgSndr_ ?? MsgSndrDefaultValue; }
      set {
        msgSndr_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "msg_sndr" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMsgSndr {
      get { return msgSndr_ != null; }
    }
    /// <summary>Clears the value of the "msg_sndr" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMsgSndr() {
      msgSndr_ = null;
    }

    /// <summary>Field number for the "msg_acptr" field.</summary>
    public const int MsgAcptrFieldNumber = 3;
    private readonly static string MsgAcptrDefaultValue = "";

    private string msgAcptr_;
    /// <summary>
    ///消息接收方{ME、BTP、QPR、Adaptor}
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MsgAcptr {
      get { return msgAcptr_ ?? MsgAcptrDefaultValue; }
      set {
        msgAcptr_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "msg_acptr" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMsgAcptr {
      get { return msgAcptr_ != null; }
    }
    /// <summary>Clears the value of the "msg_acptr" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMsgAcptr() {
      msgAcptr_ = null;
    }

    /// <summary>Field number for the "sndr_cmpnt_id" field.</summary>
    public const int SndrCmpntIdFieldNumber = 4;
    private readonly static string SndrCmpntIdDefaultValue = "";

    private string sndrCmpntId_;
    /// <summary>
    ///消息提交机构{机构6位码 or 21位码，行情header时，行情消息接收方机构复用字段}
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string SndrCmpntId {
      get { return sndrCmpntId_ ?? SndrCmpntIdDefaultValue; }
      set {
        sndrCmpntId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "sndr_cmpnt_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasSndrCmpntId {
      get { return sndrCmpntId_ != null; }
    }
    /// <summary>Clears the value of the "sndr_cmpnt_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearSndrCmpntId() {
      sndrCmpntId_ = null;
    }

    /// <summary>Field number for the "sndr_sub_id" field.</summary>
    public const int SndrSubIdFieldNumber = 5;
    private readonly static string SndrSubIdDefaultValue = "";

    private string sndrSubId_;
    /// <summary>
    ///消息提交交易员{交易员登录名， 行情header时，行情消息接收方用户复用字段}
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string SndrSubId {
      get { return sndrSubId_ ?? SndrSubIdDefaultValue; }
      set {
        sndrSubId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "sndr_sub_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasSndrSubId {
      get { return sndrSubId_ != null; }
    }
    /// <summary>Clears the value of the "sndr_sub_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearSndrSubId() {
      sndrSubId_ = null;
    }

    /// <summary>Field number for the "msg_cd" field.</summary>
    public const int MsgCdFieldNumber = 6;
    private readonly static long MsgCdDefaultValue = 0L;

    private long msgCd_;
    /// <summary>
    ///全局消息id   全局唯一
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long MsgCd {
      get { if ((_hasBits0 & 2) != 0) { return msgCd_; } else { return MsgCdDefaultValue; } }
      set {
        _hasBits0 |= 2;
        msgCd_ = value;
      }
    }
    /// <summary>Gets whether the "msg_cd" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMsgCd {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "msg_cd" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMsgCd() {
      _hasBits0 &= ~2;
    }

    /// <summary>Field number for the "srvc_id" field.</summary>
    public const int SrvcIdFieldNumber = 7;
    private readonly static int SrvcIdDefaultValue = 0;

    private int srvcId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int SrvcId {
      get { if ((_hasBits0 & 4) != 0) { return srvcId_; } else { return SrvcIdDefaultValue; } }
      set {
        _hasBits0 |= 4;
        srvcId_ = value;
      }
    }
    /// <summary>Gets whether the "srvc_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasSrvcId {
      get { return (_hasBits0 & 4) != 0; }
    }
    /// <summary>Clears the value of the "srvc_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearSrvcId() {
      _hasBits0 &= ~4;
    }

    /// <summary>Field number for the "token" field.</summary>
    public const int TokenFieldNumber = 9;
    private readonly static string TokenDefaultValue = "";

    private string token_;
    /// <summary>
    ///optional int32 msg_rsnd_f = 8;		//消息重发标志位
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Token {
      get { return token_ ?? TokenDefaultValue; }
      set {
        token_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "token" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasToken {
      get { return token_ != null; }
    }
    /// <summary>Clears the value of the "token" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearToken() {
      token_ = null;
    }

    /// <summary>Field number for the "err_cd" field.</summary>
    public const int ErrCdFieldNumber = 10;
    private readonly static int ErrCdDefaultValue = 0;

    private int errCd_;
    /// <summary>
    ///错误码
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ErrCd {
      get { if ((_hasBits0 & 8) != 0) { return errCd_; } else { return ErrCdDefaultValue; } }
      set {
        _hasBits0 |= 8;
        errCd_ = value;
      }
    }
    /// <summary>Gets whether the "err_cd" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasErrCd {
      get { return (_hasBits0 & 8) != 0; }
    }
    /// <summary>Clears the value of the "err_cd" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearErrCd() {
      _hasBits0 &= ~8;
    }

    /// <summary>Field number for the "err_msg" field.</summary>
    public const int ErrMsgFieldNumber = 11;
    private readonly static string ErrMsgDefaultValue = "";

    private string errMsg_;
    /// <summary>
    ///错误信息
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ErrMsg {
      get { return errMsg_ ?? ErrMsgDefaultValue; }
      set {
        errMsg_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "err_msg" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasErrMsg {
      get { return errMsg_ != null; }
    }
    /// <summary>Clears the value of the "err_msg" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearErrMsg() {
      errMsg_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Header);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Header other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MsgSendTm != other.MsgSendTm) return false;
      if (MsgSndr != other.MsgSndr) return false;
      if (MsgAcptr != other.MsgAcptr) return false;
      if (SndrCmpntId != other.SndrCmpntId) return false;
      if (SndrSubId != other.SndrSubId) return false;
      if (MsgCd != other.MsgCd) return false;
      if (SrvcId != other.SrvcId) return false;
      if (Token != other.Token) return false;
      if (ErrCd != other.ErrCd) return false;
      if (ErrMsg != other.ErrMsg) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HasMsgSendTm) hash ^= MsgSendTm.GetHashCode();
      if (HasMsgSndr) hash ^= MsgSndr.GetHashCode();
      if (HasMsgAcptr) hash ^= MsgAcptr.GetHashCode();
      if (HasSndrCmpntId) hash ^= SndrCmpntId.GetHashCode();
      if (HasSndrSubId) hash ^= SndrSubId.GetHashCode();
      if (HasMsgCd) hash ^= MsgCd.GetHashCode();
      if (HasSrvcId) hash ^= SrvcId.GetHashCode();
      if (HasToken) hash ^= Token.GetHashCode();
      if (HasErrCd) hash ^= ErrCd.GetHashCode();
      if (HasErrMsg) hash ^= ErrMsg.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (HasMsgSendTm) {
        output.WriteRawTag(8);
        output.WriteInt32(MsgSendTm);
      }
      if (HasMsgSndr) {
        output.WriteRawTag(18);
        output.WriteString(MsgSndr);
      }
      if (HasMsgAcptr) {
        output.WriteRawTag(26);
        output.WriteString(MsgAcptr);
      }
      if (HasSndrCmpntId) {
        output.WriteRawTag(34);
        output.WriteString(SndrCmpntId);
      }
      if (HasSndrSubId) {
        output.WriteRawTag(42);
        output.WriteString(SndrSubId);
      }
      if (HasMsgCd) {
        output.WriteRawTag(48);
        output.WriteInt64(MsgCd);
      }
      if (HasSrvcId) {
        output.WriteRawTag(56);
        output.WriteInt32(SrvcId);
      }
      if (HasToken) {
        output.WriteRawTag(74);
        output.WriteString(Token);
      }
      if (HasErrCd) {
        output.WriteRawTag(80);
        output.WriteInt32(ErrCd);
      }
      if (HasErrMsg) {
        output.WriteRawTag(90);
        output.WriteString(ErrMsg);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (HasMsgSendTm) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MsgSendTm);
      }
      if (HasMsgSndr) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MsgSndr);
      }
      if (HasMsgAcptr) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MsgAcptr);
      }
      if (HasSndrCmpntId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(SndrCmpntId);
      }
      if (HasSndrSubId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(SndrSubId);
      }
      if (HasMsgCd) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(MsgCd);
      }
      if (HasSrvcId) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SrvcId);
      }
      if (HasToken) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Token);
      }
      if (HasErrCd) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ErrCd);
      }
      if (HasErrMsg) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ErrMsg);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Header other) {
      if (other == null) {
        return;
      }
      if (other.HasMsgSendTm) {
        MsgSendTm = other.MsgSendTm;
      }
      if (other.HasMsgSndr) {
        MsgSndr = other.MsgSndr;
      }
      if (other.HasMsgAcptr) {
        MsgAcptr = other.MsgAcptr;
      }
      if (other.HasSndrCmpntId) {
        SndrCmpntId = other.SndrCmpntId;
      }
      if (other.HasSndrSubId) {
        SndrSubId = other.SndrSubId;
      }
      if (other.HasMsgCd) {
        MsgCd = other.MsgCd;
      }
      if (other.HasSrvcId) {
        SrvcId = other.SrvcId;
      }
      if (other.HasToken) {
        Token = other.Token;
      }
      if (other.HasErrCd) {
        ErrCd = other.ErrCd;
      }
      if (other.HasErrMsg) {
        ErrMsg = other.ErrMsg;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            MsgSendTm = input.ReadInt32();
            break;
          }
          case 18: {
            MsgSndr = input.ReadString();
            break;
          }
          case 26: {
            MsgAcptr = input.ReadString();
            break;
          }
          case 34: {
            SndrCmpntId = input.ReadString();
            break;
          }
          case 42: {
            SndrSubId = input.ReadString();
            break;
          }
          case 48: {
            MsgCd = input.ReadInt64();
            break;
          }
          case 56: {
            SrvcId = input.ReadInt32();
            break;
          }
          case 74: {
            Token = input.ReadString();
            break;
          }
          case 80: {
            ErrCd = input.ReadInt32();
            break;
          }
          case 90: {
            ErrMsg = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code