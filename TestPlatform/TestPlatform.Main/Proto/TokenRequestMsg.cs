// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: TokenRequestMsg.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ctrade.Message {

  /// <summary>Holder for reflection information generated from TokenRequestMsg.proto</summary>
  public static partial class TokenRequestMsgReflection {

    #region Descriptor
    /// <summary>File descriptor for TokenRequestMsg.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static TokenRequestMsgReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChVUb2tlblJlcXVlc3RNc2cucHJvdG8SDmN0cmFkZS5tZXNzYWdlGg5zdGFu",
            "ZGFyZC5wcm90bxoMSGVhZGVyLnByb3RvGgpVc2VyLnByb3RvIrMBCgxUb2tl",
            "blJlcXVlc3QSEQoJdXNlcl9uYW1lGAEgAigJEhQKDHJlcXVlc3RfdHlwZRgC",
            "IAIoBRISCgpyZXF1ZXN0X2lkGAMgAigJEhMKC3VzZXJfc3RhdHVzGAQgAigJ",
            "EhkKEWxvZ2luX2NlcnRpZmljYXRlGAUgAigJEhIKCmxvZ2luX3R5cGUYBiAC",
            "KAkSIgoEdXNlchhkIAIoCzIULmN0cmFkZS5tZXNzYWdlLlVzZXIiZQoPVG9r",
            "ZW5SZXF1ZXN0TXNnEiYKBmhlYWRlchgBIAEoCzIWLmN0cmFkZS5tZXNzYWdl",
            "LkhlYWRlchIqCgRib2R5GAIgASgLMhwuY3RyYWRlLm1lc3NhZ2UuVG9rZW5S",
            "ZXF1ZXN0QlYKFWNuLmNvbS5jZmV0cy5kYXRhLm1zZ0IVVG9rZW5SZXF1ZXN0",
            "TXNnUHJvYnVmgrUYEUNUUkFERS1PUkRFUi1EQVRBirUYD1Rva2VuUmVxdWVz",
            "dE1zZw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::StandardReflection.Descriptor, global::Ctrade.Message.HeaderReflection.Descriptor, global::Ctrade.Message.UserReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.TokenRequest), global::Ctrade.Message.TokenRequest.Parser, new[]{ "UserName", "RequestType", "RequestId", "UserStatus", "LoginCertificate", "LoginType", "User" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.TokenRequestMsg), global::Ctrade.Message.TokenRequestMsg.Parser, new[]{ "Header", "Body" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  //// Token申请
  /// </summary>
  public sealed partial class TokenRequest : pb::IMessage<TokenRequest> {
    private static readonly pb::MessageParser<TokenRequest> _parser = new pb::MessageParser<TokenRequest>(() => new TokenRequest());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<TokenRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.TokenRequestMsgReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TokenRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TokenRequest(TokenRequest other) : this() {
      _hasBits0 = other._hasBits0;
      userName_ = other.userName_;
      requestType_ = other.requestType_;
      requestId_ = other.requestId_;
      userStatus_ = other.userStatus_;
      loginCertificate_ = other.loginCertificate_;
      loginType_ = other.loginType_;
      user_ = other.user_ != null ? other.user_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TokenRequest Clone() {
      return new TokenRequest(this);
    }

    /// <summary>Field number for the "user_name" field.</summary>
    public const int UserNameFieldNumber = 1;
    private readonly static string UserNameDefaultValue = "";

    private string userName_;
    /// <summary>
    //// 用户名
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UserName {
      get { return userName_ ?? UserNameDefaultValue; }
      set {
        userName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "user_name" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasUserName {
      get { return userName_ != null; }
    }
    /// <summary>Clears the value of the "user_name" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearUserName() {
      userName_ = null;
    }

    /// <summary>Field number for the "request_type" field.</summary>
    public const int RequestTypeFieldNumber = 2;
    private readonly static int RequestTypeDefaultValue = 0;

    private int requestType_;
    /// <summary>
    //// 100-Token申请
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int RequestType {
      get { if ((_hasBits0 & 1) != 0) { return requestType_; } else { return RequestTypeDefaultValue; } }
      set {
        _hasBits0 |= 1;
        requestType_ = value;
      }
    }
    /// <summary>Gets whether the "request_type" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasRequestType {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "request_type" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearRequestType() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "request_id" field.</summary>
    public const int RequestIdFieldNumber = 3;
    private readonly static string RequestIdDefaultValue = "";

    private string requestId_;
    /// <summary>
    //// IH用于分配登录消息
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string RequestId {
      get { return requestId_ ?? RequestIdDefaultValue; }
      set {
        requestId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "request_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasRequestId {
      get { return requestId_ != null; }
    }
    /// <summary>Clears the value of the "request_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearRequestId() {
      requestId_ = null;
    }

    /// <summary>Field number for the "user_status" field.</summary>
    public const int UserStatusFieldNumber = 4;
    private readonly static string UserStatusDefaultValue = "";

    private string userStatus_;
    /// <summary>
    //// 登录状态
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UserStatus {
      get { return userStatus_ ?? UserStatusDefaultValue; }
      set {
        userStatus_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "user_status" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasUserStatus {
      get { return userStatus_ != null; }
    }
    /// <summary>Clears the value of the "user_status" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearUserStatus() {
      userStatus_ = null;
    }

    /// <summary>Field number for the "login_certificate" field.</summary>
    public const int LoginCertificateFieldNumber = 5;
    private readonly static string LoginCertificateDefaultValue = "";

    private string loginCertificate_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string LoginCertificate {
      get { return loginCertificate_ ?? LoginCertificateDefaultValue; }
      set {
        loginCertificate_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "login_certificate" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasLoginCertificate {
      get { return loginCertificate_ != null; }
    }
    /// <summary>Clears the value of the "login_certificate" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearLoginCertificate() {
      loginCertificate_ = null;
    }

    /// <summary>Field number for the "login_type" field.</summary>
    public const int LoginTypeFieldNumber = 6;
    private readonly static string LoginTypeDefaultValue = "";

    private string loginType_;
    /// <summary>
    ////登录类型 1-GW_QUOTE 2-GW_DEAL 3-GW_CRDT
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string LoginType {
      get { return loginType_ ?? LoginTypeDefaultValue; }
      set {
        loginType_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "login_type" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasLoginType {
      get { return loginType_ != null; }
    }
    /// <summary>Clears the value of the "login_type" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearLoginType() {
      loginType_ = null;
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
      return Equals(other as TokenRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(TokenRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserName != other.UserName) return false;
      if (RequestType != other.RequestType) return false;
      if (RequestId != other.RequestId) return false;
      if (UserStatus != other.UserStatus) return false;
      if (LoginCertificate != other.LoginCertificate) return false;
      if (LoginType != other.LoginType) return false;
      if (!object.Equals(User, other.User)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HasUserName) hash ^= UserName.GetHashCode();
      if (HasRequestType) hash ^= RequestType.GetHashCode();
      if (HasRequestId) hash ^= RequestId.GetHashCode();
      if (HasUserStatus) hash ^= UserStatus.GetHashCode();
      if (HasLoginCertificate) hash ^= LoginCertificate.GetHashCode();
      if (HasLoginType) hash ^= LoginType.GetHashCode();
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
      if (HasUserName) {
        output.WriteRawTag(10);
        output.WriteString(UserName);
      }
      if (HasRequestType) {
        output.WriteRawTag(16);
        output.WriteInt32(RequestType);
      }
      if (HasRequestId) {
        output.WriteRawTag(26);
        output.WriteString(RequestId);
      }
      if (HasUserStatus) {
        output.WriteRawTag(34);
        output.WriteString(UserStatus);
      }
      if (HasLoginCertificate) {
        output.WriteRawTag(42);
        output.WriteString(LoginCertificate);
      }
      if (HasLoginType) {
        output.WriteRawTag(50);
        output.WriteString(LoginType);
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
      if (HasUserName) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UserName);
      }
      if (HasRequestType) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RequestType);
      }
      if (HasRequestId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RequestId);
      }
      if (HasUserStatus) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UserStatus);
      }
      if (HasLoginCertificate) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(LoginCertificate);
      }
      if (HasLoginType) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(LoginType);
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
    public void MergeFrom(TokenRequest other) {
      if (other == null) {
        return;
      }
      if (other.HasUserName) {
        UserName = other.UserName;
      }
      if (other.HasRequestType) {
        RequestType = other.RequestType;
      }
      if (other.HasRequestId) {
        RequestId = other.RequestId;
      }
      if (other.HasUserStatus) {
        UserStatus = other.UserStatus;
      }
      if (other.HasLoginCertificate) {
        LoginCertificate = other.LoginCertificate;
      }
      if (other.HasLoginType) {
        LoginType = other.LoginType;
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
            UserName = input.ReadString();
            break;
          }
          case 16: {
            RequestType = input.ReadInt32();
            break;
          }
          case 26: {
            RequestId = input.ReadString();
            break;
          }
          case 34: {
            UserStatus = input.ReadString();
            break;
          }
          case 42: {
            LoginCertificate = input.ReadString();
            break;
          }
          case 50: {
            LoginType = input.ReadString();
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

  public sealed partial class TokenRequestMsg : pb::IMessage<TokenRequestMsg> {
    private static readonly pb::MessageParser<TokenRequestMsg> _parser = new pb::MessageParser<TokenRequestMsg>(() => new TokenRequestMsg());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<TokenRequestMsg> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.TokenRequestMsgReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TokenRequestMsg() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TokenRequestMsg(TokenRequestMsg other) : this() {
      header_ = other.header_ != null ? other.header_.Clone() : null;
      body_ = other.body_ != null ? other.body_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TokenRequestMsg Clone() {
      return new TokenRequestMsg(this);
    }

    /// <summary>Field number for the "header" field.</summary>
    public const int HeaderFieldNumber = 1;
    private global::Ctrade.Message.Header header_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ctrade.Message.Header Header {
      get { return header_; }
      set {
        header_ = value;
      }
    }

    /// <summary>Field number for the "body" field.</summary>
    public const int BodyFieldNumber = 2;
    private global::Ctrade.Message.TokenRequest body_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ctrade.Message.TokenRequest Body {
      get { return body_; }
      set {
        body_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as TokenRequestMsg);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(TokenRequestMsg other) {
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
    public void MergeFrom(TokenRequestMsg other) {
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
          Body = new global::Ctrade.Message.TokenRequest();
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
              Body = new global::Ctrade.Message.TokenRequest();
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