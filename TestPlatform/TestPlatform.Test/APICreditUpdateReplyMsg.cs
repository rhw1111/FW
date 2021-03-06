// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: APICreditUpdateReplyMsg.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ctrade.Message {

  /// <summary>Holder for reflection information generated from APICreditUpdateReplyMsg.proto</summary>
  public static partial class APICreditUpdateReplyMsgReflection {

    #region Descriptor
    /// <summary>File descriptor for APICreditUpdateReplyMsg.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static APICreditUpdateReplyMsgReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch1BUElDcmVkaXRVcGRhdGVSZXBseU1zZy5wcm90bxIOY3RyYWRlLm1lc3Nh",
            "Z2UaDnN0YW5kYXJkLnByb3RvGgxIZWFkZXIucHJvdG8aClVzZXIucHJvdG8i",
            "+AEKFEFQSUNyZWRpdFVwZGF0ZVJlcGx5EhkKEXJpc2tfbGltaXRfcmVxX2lk",
            "GAEgAigJEhwKEG1hcmtldF9pbmRpY2F0b3IYAiACKAU6AjExEigKGGluc3Rf",
            "c2NvcGVfc2VjdXJpdHlfdHlwZRgDIAEoCToGRlhTV0FQEhkKEXJpc2tfbGlt",
            "aXRfcmVzdWx0GAQgAigFEhMKC3JlamVjdF90ZXh0GAUgASgJEhMKC2V4ZWNf",
            "b3JnX2lkGAYgAigJEhQKDGV4ZWNfdXNlcl9pZBgHIAIoCRIiCgR1c2VyGGQg",
            "AigLMhQuY3RyYWRlLm1lc3NhZ2UuVXNlciJ1ChdBUElDcmVkaXRVcGRhdGVS",
            "ZXBseU1zZxImCgZoZWFkZXIYASABKAsyFi5jdHJhZGUubWVzc2FnZS5IZWFk",
            "ZXISMgoEYm9keRgCIAEoCzIkLmN0cmFkZS5tZXNzYWdlLkFQSUNyZWRpdFVw",
            "ZGF0ZVJlcGx5Ql0KIGNuLmNvbS5jZmV0cy5kYXRhLmN0cmFkZS5tZXNzYWdl",
            "Qh1BUElDcmVkaXRVcGRhdGVSZXBseU1zZ1Byb2J1ZoK1GBhDVFJBREUtT1JE",
            "RVItREFUQS1TVUJNSVQ="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::StandardReflection.Descriptor, global::Ctrade.Message.HeaderReflection.Descriptor, global::Ctrade.Message.UserReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.APICreditUpdateReply), global::Ctrade.Message.APICreditUpdateReply.Parser, new[]{ "RiskLimitReqId", "MarketIndicator", "InstScopeSecurityType", "RiskLimitResult", "RejectText", "ExecOrgId", "ExecUserId", "User" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.APICreditUpdateReplyMsg), global::Ctrade.Message.APICreditUpdateReplyMsg.Parser, new[]{ "Header", "Body" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// 授信修改
  /// </summary>
  public sealed partial class APICreditUpdateReply : pb::IMessage<APICreditUpdateReply> {
    private static readonly pb::MessageParser<APICreditUpdateReply> _parser = new pb::MessageParser<APICreditUpdateReply>(() => new APICreditUpdateReply());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<APICreditUpdateReply> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.APICreditUpdateReplyMsgReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APICreditUpdateReply() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APICreditUpdateReply(APICreditUpdateReply other) : this() {
      _hasBits0 = other._hasBits0;
      riskLimitReqId_ = other.riskLimitReqId_;
      marketIndicator_ = other.marketIndicator_;
      instScopeSecurityType_ = other.instScopeSecurityType_;
      riskLimitResult_ = other.riskLimitResult_;
      rejectText_ = other.rejectText_;
      execOrgId_ = other.execOrgId_;
      execUserId_ = other.execUserId_;
      user_ = other.user_ != null ? other.user_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APICreditUpdateReply Clone() {
      return new APICreditUpdateReply(this);
    }

    /// <summary>Field number for the "risk_limit_req_id" field.</summary>
    public const int RiskLimitReqIdFieldNumber = 1;
    private readonly static string RiskLimitReqIdDefaultValue = "";

    private string riskLimitReqId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string RiskLimitReqId {
      get { return riskLimitReqId_ ?? RiskLimitReqIdDefaultValue; }
      set {
        riskLimitReqId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "risk_limit_req_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasRiskLimitReqId {
      get { return riskLimitReqId_ != null; }
    }
    /// <summary>Clears the value of the "risk_limit_req_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearRiskLimitReqId() {
      riskLimitReqId_ = null;
    }

    /// <summary>Field number for the "market_indicator" field.</summary>
    public const int MarketIndicatorFieldNumber = 2;
    private readonly static int MarketIndicatorDefaultValue = 11;

    private int marketIndicator_;
    /// <summary>
    //// 市场标识
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MarketIndicator {
      get { if ((_hasBits0 & 1) != 0) { return marketIndicator_; } else { return MarketIndicatorDefaultValue; } }
      set {
        _hasBits0 |= 1;
        marketIndicator_ = value;
      }
    }
    /// <summary>Gets whether the "market_indicator" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMarketIndicator {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "market_indicator" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMarketIndicator() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "inst_scope_security_type" field.</summary>
    public const int InstScopeSecurityTypeFieldNumber = 3;
    private readonly static string InstScopeSecurityTypeDefaultValue = global::System.Text.Encoding.UTF8.GetString(global::System.Convert.FromBase64String("RlhTV0FQ"), 0, 6);

    private string instScopeSecurityType_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string InstScopeSecurityType {
      get { return instScopeSecurityType_ ?? InstScopeSecurityTypeDefaultValue; }
      set {
        instScopeSecurityType_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "inst_scope_security_type" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasInstScopeSecurityType {
      get { return instScopeSecurityType_ != null; }
    }
    /// <summary>Clears the value of the "inst_scope_security_type" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearInstScopeSecurityType() {
      instScopeSecurityType_ = null;
    }

    /// <summary>Field number for the "risk_limit_result" field.</summary>
    public const int RiskLimitResultFieldNumber = 4;
    private readonly static int RiskLimitResultDefaultValue = 0;

    private int riskLimitResult_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int RiskLimitResult {
      get { if ((_hasBits0 & 2) != 0) { return riskLimitResult_; } else { return RiskLimitResultDefaultValue; } }
      set {
        _hasBits0 |= 2;
        riskLimitResult_ = value;
      }
    }
    /// <summary>Gets whether the "risk_limit_result" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasRiskLimitResult {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "risk_limit_result" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearRiskLimitResult() {
      _hasBits0 &= ~2;
    }

    /// <summary>Field number for the "reject_text" field.</summary>
    public const int RejectTextFieldNumber = 5;
    private readonly static string RejectTextDefaultValue = "";

    private string rejectText_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string RejectText {
      get { return rejectText_ ?? RejectTextDefaultValue; }
      set {
        rejectText_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "reject_text" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasRejectText {
      get { return rejectText_ != null; }
    }
    /// <summary>Clears the value of the "reject_text" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearRejectText() {
      rejectText_ = null;
    }

    /// <summary>Field number for the "exec_org_id" field.</summary>
    public const int ExecOrgIdFieldNumber = 6;
    private readonly static string ExecOrgIdDefaultValue = "";

    private string execOrgId_;
    /// <summary>
    ////执行方机构ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ExecOrgId {
      get { return execOrgId_ ?? ExecOrgIdDefaultValue; }
      set {
        execOrgId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "exec_org_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasExecOrgId {
      get { return execOrgId_ != null; }
    }
    /// <summary>Clears the value of the "exec_org_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearExecOrgId() {
      execOrgId_ = null;
    }

    /// <summary>Field number for the "exec_user_id" field.</summary>
    public const int ExecUserIdFieldNumber = 7;
    private readonly static string ExecUserIdDefaultValue = "";

    private string execUserId_;
    /// <summary>
    ////执行方用户ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ExecUserId {
      get { return execUserId_ ?? ExecUserIdDefaultValue; }
      set {
        execUserId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "exec_user_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasExecUserId {
      get { return execUserId_ != null; }
    }
    /// <summary>Clears the value of the "exec_user_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearExecUserId() {
      execUserId_ = null;
    }

    /// <summary>Field number for the "user" field.</summary>
    public const int UserFieldNumber = 100;
    private global::Ctrade.Message.User user_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ctrade.Message.User User {
      get { return user_; }
      set {
        user_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as APICreditUpdateReply);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(APICreditUpdateReply other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RiskLimitReqId != other.RiskLimitReqId) return false;
      if (MarketIndicator != other.MarketIndicator) return false;
      if (InstScopeSecurityType != other.InstScopeSecurityType) return false;
      if (RiskLimitResult != other.RiskLimitResult) return false;
      if (RejectText != other.RejectText) return false;
      if (ExecOrgId != other.ExecOrgId) return false;
      if (ExecUserId != other.ExecUserId) return false;
      if (!object.Equals(User, other.User)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HasRiskLimitReqId) hash ^= RiskLimitReqId.GetHashCode();
      if (HasMarketIndicator) hash ^= MarketIndicator.GetHashCode();
      if (HasInstScopeSecurityType) hash ^= InstScopeSecurityType.GetHashCode();
      if (HasRiskLimitResult) hash ^= RiskLimitResult.GetHashCode();
      if (HasRejectText) hash ^= RejectText.GetHashCode();
      if (HasExecOrgId) hash ^= ExecOrgId.GetHashCode();
      if (HasExecUserId) hash ^= ExecUserId.GetHashCode();
      if (user_ != null) hash ^= User.GetHashCode();
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
      if (HasRiskLimitReqId) {
        output.WriteRawTag(10);
        output.WriteString(RiskLimitReqId);
      }
      if (HasMarketIndicator) {
        output.WriteRawTag(16);
        output.WriteInt32(MarketIndicator);
      }
      if (HasInstScopeSecurityType) {
        output.WriteRawTag(26);
        output.WriteString(InstScopeSecurityType);
      }
      if (HasRiskLimitResult) {
        output.WriteRawTag(32);
        output.WriteInt32(RiskLimitResult);
      }
      if (HasRejectText) {
        output.WriteRawTag(42);
        output.WriteString(RejectText);
      }
      if (HasExecOrgId) {
        output.WriteRawTag(50);
        output.WriteString(ExecOrgId);
      }
      if (HasExecUserId) {
        output.WriteRawTag(58);
        output.WriteString(ExecUserId);
      }
      if (user_ != null) {
        output.WriteRawTag(162, 6);
        output.WriteMessage(User);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (HasRiskLimitReqId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RiskLimitReqId);
      }
      if (HasMarketIndicator) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MarketIndicator);
      }
      if (HasInstScopeSecurityType) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(InstScopeSecurityType);
      }
      if (HasRiskLimitResult) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RiskLimitResult);
      }
      if (HasRejectText) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RejectText);
      }
      if (HasExecOrgId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ExecOrgId);
      }
      if (HasExecUserId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ExecUserId);
      }
      if (user_ != null) {
        size += 2 + pb::CodedOutputStream.ComputeMessageSize(User);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(APICreditUpdateReply other) {
      if (other == null) {
        return;
      }
      if (other.HasRiskLimitReqId) {
        RiskLimitReqId = other.RiskLimitReqId;
      }
      if (other.HasMarketIndicator) {
        MarketIndicator = other.MarketIndicator;
      }
      if (other.HasInstScopeSecurityType) {
        InstScopeSecurityType = other.InstScopeSecurityType;
      }
      if (other.HasRiskLimitResult) {
        RiskLimitResult = other.RiskLimitResult;
      }
      if (other.HasRejectText) {
        RejectText = other.RejectText;
      }
      if (other.HasExecOrgId) {
        ExecOrgId = other.ExecOrgId;
      }
      if (other.HasExecUserId) {
        ExecUserId = other.ExecUserId;
      }
      if (other.user_ != null) {
        if (user_ == null) {
          User = new global::Ctrade.Message.User();
        }
        User.MergeFrom(other.User);
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
          case 10: {
            RiskLimitReqId = input.ReadString();
            break;
          }
          case 16: {
            MarketIndicator = input.ReadInt32();
            break;
          }
          case 26: {
            InstScopeSecurityType = input.ReadString();
            break;
          }
          case 32: {
            RiskLimitResult = input.ReadInt32();
            break;
          }
          case 42: {
            RejectText = input.ReadString();
            break;
          }
          case 50: {
            ExecOrgId = input.ReadString();
            break;
          }
          case 58: {
            ExecUserId = input.ReadString();
            break;
          }
          case 802: {
            if (user_ == null) {
              User = new global::Ctrade.Message.User();
            }
            input.ReadMessage(User);
            break;
          }
        }
      }
    }

  }

  public sealed partial class APICreditUpdateReplyMsg : pb::IMessage<APICreditUpdateReplyMsg> {
    private static readonly pb::MessageParser<APICreditUpdateReplyMsg> _parser = new pb::MessageParser<APICreditUpdateReplyMsg>(() => new APICreditUpdateReplyMsg());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<APICreditUpdateReplyMsg> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.APICreditUpdateReplyMsgReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APICreditUpdateReplyMsg() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APICreditUpdateReplyMsg(APICreditUpdateReplyMsg other) : this() {
      header_ = other.header_ != null ? other.header_.Clone() : null;
      body_ = other.body_ != null ? other.body_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APICreditUpdateReplyMsg Clone() {
      return new APICreditUpdateReplyMsg(this);
    }

    /// <summary>Field number for the "header" field.</summary>
    public const int HeaderFieldNumber = 1;
    private global::Ctrade.Message.Header header_;
    /// <summary>
    ///TODO 待确定
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ctrade.Message.Header Header {
      get { return header_; }
      set {
        header_ = value;
      }
    }

    /// <summary>Field number for the "body" field.</summary>
    public const int BodyFieldNumber = 2;
    private global::Ctrade.Message.APICreditUpdateReply body_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ctrade.Message.APICreditUpdateReply Body {
      get { return body_; }
      set {
        body_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as APICreditUpdateReplyMsg);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(APICreditUpdateReplyMsg other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Header, other.Header)) return false;
      if (!object.Equals(Body, other.Body)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (header_ != null) hash ^= Header.GetHashCode();
      if (body_ != null) hash ^= Body.GetHashCode();
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
      if (header_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Header);
      }
      if (body_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Body);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (header_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Header);
      }
      if (body_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Body);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(APICreditUpdateReplyMsg other) {
      if (other == null) {
        return;
      }
      if (other.header_ != null) {
        if (header_ == null) {
          Header = new global::Ctrade.Message.Header();
        }
        Header.MergeFrom(other.Header);
      }
      if (other.body_ != null) {
        if (body_ == null) {
          Body = new global::Ctrade.Message.APICreditUpdateReply();
        }
        Body.MergeFrom(other.Body);
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
          case 10: {
            if (header_ == null) {
              Header = new global::Ctrade.Message.Header();
            }
            input.ReadMessage(Header);
            break;
          }
          case 18: {
            if (body_ == null) {
              Body = new global::Ctrade.Message.APICreditUpdateReply();
            }
            input.ReadMessage(Body);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
