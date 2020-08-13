// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: ApiMarketDataRequest.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ctrade.Message {

  /// <summary>Holder for reflection information generated from ApiMarketDataRequest.proto</summary>
  public static partial class ApiMarketDataRequestReflection {

    #region Descriptor
    /// <summary>File descriptor for ApiMarketDataRequest.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ApiMarketDataRequestReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChpBcGlNYXJrZXREYXRhUmVxdWVzdC5wcm90bxIOY3RyYWRlLm1lc3NhZ2Ua",
            "DnN0YW5kYXJkLnByb3RvGgxIZWFkZXIucHJvdG8iTgoPSW5zdHJ1bWVudE1k",
            "UmVxEg4KBnN5bWJvbBgBIAIoCRIYChBtYXJrZXRfaW5kaWNhdG9yGAIgAigF",
            "EhEKCWNudHJjdF9ubRgDIAIoCSL+AQoYQXBpTWFya2V0RGF0YVJlcXVlc3RC",
            "b2R5EhEKCW1kX3JlcV9pZBgBIAIoCRISCgphcHBsX3Rva2VuGAIgAigJEiEK",
            "GXN1YnNjcmlwdGlvbl9yZXF1ZXN0X3R5cGUYAyACKAUSFAoMbWRfYm9va190",
            "eXBlGAQgAigFEhQKDG1hcmtldF9kZXB0aBgFIAIoCRIVCg10cmFuc2FjdF90",
            "aW1lGAYgAigJEg4KBm9yZ19pZBgHIAIoCRIPCgd1c2VyX2lkGAggAigJEjQK",
            "C2luc3RydW1lbnRzGAkgAygLMh8uY3RyYWRlLm1lc3NhZ2UuSW5zdHJ1bWVu",
            "dE1kUmVxInYKFEFwaU1hcmtldERhdGFSZXF1ZXN0EiYKBmhlYWRlchgBIAEo",
            "CzIWLmN0cmFkZS5tZXNzYWdlLkhlYWRlchI2CgRib2R5GAIgASgLMiguY3Ry",
            "YWRlLm1lc3NhZ2UuQXBpTWFya2V0RGF0YVJlcXVlc3RCb2R5Ql8KIGNuLmNv",
            "bS5jZmV0cy5kYXRhLmN0cmFkZS5tZXNzYWdlQhpBcGlNYXJrZXREYXRhUmVx",
            "dWVzdFByb2J1ZoK1GB1DVFJBREUtQVBJLU1BUktFVERBVEEtUkVRVUVTVA=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::StandardReflection.Descriptor, global::Ctrade.Message.HeaderReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.InstrumentMdReq), global::Ctrade.Message.InstrumentMdReq.Parser, new[]{ "Symbol", "MarketIndicator", "CntrctNm" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.ApiMarketDataRequestBody), global::Ctrade.Message.ApiMarketDataRequestBody.Parser, new[]{ "MdReqId", "ApplToken", "SubscriptionRequestType", "MdBookType", "MarketDepth", "TransactTime", "OrgId", "UserId", "Instruments" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.ApiMarketDataRequest), global::Ctrade.Message.ApiMarketDataRequest.Parser, new[]{ "Header", "Body" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// Market date of price level
  /// </summary>
  public sealed partial class InstrumentMdReq : pb::IMessage<InstrumentMdReq> {
    private static readonly pb::MessageParser<InstrumentMdReq> _parser = new pb::MessageParser<InstrumentMdReq>(() => new InstrumentMdReq());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<InstrumentMdReq> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.ApiMarketDataRequestReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public InstrumentMdReq() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public InstrumentMdReq(InstrumentMdReq other) : this() {
      _hasBits0 = other._hasBits0;
      symbol_ = other.symbol_;
      marketIndicator_ = other.marketIndicator_;
      cntrctNm_ = other.cntrctNm_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public InstrumentMdReq Clone() {
      return new InstrumentMdReq(this);
    }

    /// <summary>Field number for the "symbol" field.</summary>
    public const int SymbolFieldNumber = 1;
    private readonly static string SymbolDefaultValue = "";

    private string symbol_;
    /// <summary>
    /// currency pair name
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Symbol {
      get { return symbol_ ?? SymbolDefaultValue; }
      set {
        symbol_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "symbol" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasSymbol {
      get { return symbol_ != null; }
    }
    /// <summary>Clears the value of the "symbol" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearSymbol() {
      symbol_ = null;
    }

    /// <summary>Field number for the "market_indicator" field.</summary>
    public const int MarketIndicatorFieldNumber = 2;
    private readonly static int MarketIndicatorDefaultValue = 0;

    private int marketIndicator_;
    /// <summary>
    /// market indicator, 11-fx swap
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

    /// <summary>Field number for the "cntrct_nm" field.</summary>
    public const int CntrctNmFieldNumber = 3;
    private readonly static string CntrctNmDefaultValue = "";

    private string cntrctNm_;
    /// <summary>
    /// contract name
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string CntrctNm {
      get { return cntrctNm_ ?? CntrctNmDefaultValue; }
      set {
        cntrctNm_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "cntrct_nm" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasCntrctNm {
      get { return cntrctNm_ != null; }
    }
    /// <summary>Clears the value of the "cntrct_nm" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearCntrctNm() {
      cntrctNm_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as InstrumentMdReq);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(InstrumentMdReq other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Symbol != other.Symbol) return false;
      if (MarketIndicator != other.MarketIndicator) return false;
      if (CntrctNm != other.CntrctNm) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HasSymbol) hash ^= Symbol.GetHashCode();
      if (HasMarketIndicator) hash ^= MarketIndicator.GetHashCode();
      if (HasCntrctNm) hash ^= CntrctNm.GetHashCode();
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
      if (HasSymbol) {
        output.WriteRawTag(10);
        output.WriteString(Symbol);
      }
      if (HasMarketIndicator) {
        output.WriteRawTag(16);
        output.WriteInt32(MarketIndicator);
      }
      if (HasCntrctNm) {
        output.WriteRawTag(26);
        output.WriteString(CntrctNm);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (HasSymbol) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Symbol);
      }
      if (HasMarketIndicator) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MarketIndicator);
      }
      if (HasCntrctNm) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(CntrctNm);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(InstrumentMdReq other) {
      if (other == null) {
        return;
      }
      if (other.HasSymbol) {
        Symbol = other.Symbol;
      }
      if (other.HasMarketIndicator) {
        MarketIndicator = other.MarketIndicator;
      }
      if (other.HasCntrctNm) {
        CntrctNm = other.CntrctNm;
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
            Symbol = input.ReadString();
            break;
          }
          case 16: {
            MarketIndicator = input.ReadInt32();
            break;
          }
          case 26: {
            CntrctNm = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Market data body
  /// </summary>
  public sealed partial class ApiMarketDataRequestBody : pb::IMessage<ApiMarketDataRequestBody> {
    private static readonly pb::MessageParser<ApiMarketDataRequestBody> _parser = new pb::MessageParser<ApiMarketDataRequestBody>(() => new ApiMarketDataRequestBody());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ApiMarketDataRequestBody> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.ApiMarketDataRequestReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataRequestBody() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataRequestBody(ApiMarketDataRequestBody other) : this() {
      _hasBits0 = other._hasBits0;
      mdReqId_ = other.mdReqId_;
      applToken_ = other.applToken_;
      subscriptionRequestType_ = other.subscriptionRequestType_;
      mdBookType_ = other.mdBookType_;
      marketDepth_ = other.marketDepth_;
      transactTime_ = other.transactTime_;
      orgId_ = other.orgId_;
      userId_ = other.userId_;
      instruments_ = other.instruments_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataRequestBody Clone() {
      return new ApiMarketDataRequestBody(this);
    }

    /// <summary>Field number for the "md_req_id" field.</summary>
    public const int MdReqIdFieldNumber = 1;
    private readonly static string MdReqIdDefaultValue = "";

    private string mdReqId_;
    /// <summary>
    /// request id which is from subscribe request
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MdReqId {
      get { return mdReqId_ ?? MdReqIdDefaultValue; }
      set {
        mdReqId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "md_req_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMdReqId {
      get { return mdReqId_ != null; }
    }
    /// <summary>Clears the value of the "md_req_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMdReqId() {
      mdReqId_ = null;
    }

    /// <summary>Field number for the "appl_token" field.</summary>
    public const int ApplTokenFieldNumber = 2;
    private readonly static string ApplTokenDefaultValue = "";

    private string applToken_;
    /// <summary>
    /// token which is from logon request
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ApplToken {
      get { return applToken_ ?? ApplTokenDefaultValue; }
      set {
        applToken_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "appl_token" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasApplToken {
      get { return applToken_ != null; }
    }
    /// <summary>Clears the value of the "appl_token" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearApplToken() {
      applToken_ = null;
    }

    /// <summary>Field number for the "subscription_request_type" field.</summary>
    public const int SubscriptionRequestTypeFieldNumber = 3;
    private readonly static int SubscriptionRequestTypeDefaultValue = 0;

    private int subscriptionRequestType_;
    /// <summary>
    /// subscribe request type, 1-subscribe; 2-unsubscribe
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int SubscriptionRequestType {
      get { if ((_hasBits0 & 1) != 0) { return subscriptionRequestType_; } else { return SubscriptionRequestTypeDefaultValue; } }
      set {
        _hasBits0 |= 1;
        subscriptionRequestType_ = value;
      }
    }
    /// <summary>Gets whether the "subscription_request_type" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasSubscriptionRequestType {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "subscription_request_type" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearSubscriptionRequestType() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "md_book_type" field.</summary>
    public const int MdBookTypeFieldNumber = 4;
    private readonly static int MdBookTypeDefaultValue = 0;

    private int mdBookType_;
    /// <summary>
    /// market data type, 1-best market data; 2-price level market data; 105-public best market data;
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MdBookType {
      get { if ((_hasBits0 & 2) != 0) { return mdBookType_; } else { return MdBookTypeDefaultValue; } }
      set {
        _hasBits0 |= 2;
        mdBookType_ = value;
      }
    }
    /// <summary>Gets whether the "md_book_type" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMdBookType {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "md_book_type" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMdBookType() {
      _hasBits0 &= ~2;
    }

    /// <summary>Field number for the "market_depth" field.</summary>
    public const int MarketDepthFieldNumber = 5;
    private readonly static string MarketDepthDefaultValue = "";

    private string marketDepth_;
    /// <summary>
    /// the price level number, such as 5
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MarketDepth {
      get { return marketDepth_ ?? MarketDepthDefaultValue; }
      set {
        marketDepth_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "market_depth" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMarketDepth {
      get { return marketDepth_ != null; }
    }
    /// <summary>Clears the value of the "market_depth" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMarketDepth() {
      marketDepth_ = null;
    }

    /// <summary>Field number for the "transact_time" field.</summary>
    public const int TransactTimeFieldNumber = 6;
    private readonly static string TransactTimeDefaultValue = "";

    private string transactTime_;
    /// <summary>
    /// request time, the format: YYYYMMDD-HH:MM:SS.sss
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string TransactTime {
      get { return transactTime_ ?? TransactTimeDefaultValue; }
      set {
        transactTime_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "transact_time" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasTransactTime {
      get { return transactTime_ != null; }
    }
    /// <summary>Clears the value of the "transact_time" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearTransactTime() {
      transactTime_ = null;
    }

    /// <summary>Field number for the "org_id" field.</summary>
    public const int OrgIdFieldNumber = 7;
    private readonly static string OrgIdDefaultValue = "";

    private string orgId_;
    /// <summary>
    /// org id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string OrgId {
      get { return orgId_ ?? OrgIdDefaultValue; }
      set {
        orgId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "org_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasOrgId {
      get { return orgId_ != null; }
    }
    /// <summary>Clears the value of the "org_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearOrgId() {
      orgId_ = null;
    }

    /// <summary>Field number for the "user_id" field.</summary>
    public const int UserIdFieldNumber = 8;
    private readonly static string UserIdDefaultValue = "";

    private string userId_;
    /// <summary>
    /// user id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UserId {
      get { return userId_ ?? UserIdDefaultValue; }
      set {
        userId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "user_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasUserId {
      get { return userId_ != null; }
    }
    /// <summary>Clears the value of the "user_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearUserId() {
      userId_ = null;
    }

    /// <summary>Field number for the "instruments" field.</summary>
    public const int InstrumentsFieldNumber = 9;
    private static readonly pb::FieldCodec<global::Ctrade.Message.InstrumentMdReq> _repeated_instruments_codec
        = pb::FieldCodec.ForMessage(74, global::Ctrade.Message.InstrumentMdReq.Parser);
    private readonly pbc::RepeatedField<global::Ctrade.Message.InstrumentMdReq> instruments_ = new pbc::RepeatedField<global::Ctrade.Message.InstrumentMdReq>();
    /// <summary>
    /// subscribe instruments
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Ctrade.Message.InstrumentMdReq> Instruments {
      get { return instruments_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ApiMarketDataRequestBody);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ApiMarketDataRequestBody other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MdReqId != other.MdReqId) return false;
      if (ApplToken != other.ApplToken) return false;
      if (SubscriptionRequestType != other.SubscriptionRequestType) return false;
      if (MdBookType != other.MdBookType) return false;
      if (MarketDepth != other.MarketDepth) return false;
      if (TransactTime != other.TransactTime) return false;
      if (OrgId != other.OrgId) return false;
      if (UserId != other.UserId) return false;
      if(!instruments_.Equals(other.instruments_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HasMdReqId) hash ^= MdReqId.GetHashCode();
      if (HasApplToken) hash ^= ApplToken.GetHashCode();
      if (HasSubscriptionRequestType) hash ^= SubscriptionRequestType.GetHashCode();
      if (HasMdBookType) hash ^= MdBookType.GetHashCode();
      if (HasMarketDepth) hash ^= MarketDepth.GetHashCode();
      if (HasTransactTime) hash ^= TransactTime.GetHashCode();
      if (HasOrgId) hash ^= OrgId.GetHashCode();
      if (HasUserId) hash ^= UserId.GetHashCode();
      hash ^= instruments_.GetHashCode();
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
      if (HasMdReqId) {
        output.WriteRawTag(10);
        output.WriteString(MdReqId);
      }
      if (HasApplToken) {
        output.WriteRawTag(18);
        output.WriteString(ApplToken);
      }
      if (HasSubscriptionRequestType) {
        output.WriteRawTag(24);
        output.WriteInt32(SubscriptionRequestType);
      }
      if (HasMdBookType) {
        output.WriteRawTag(32);
        output.WriteInt32(MdBookType);
      }
      if (HasMarketDepth) {
        output.WriteRawTag(42);
        output.WriteString(MarketDepth);
      }
      if (HasTransactTime) {
        output.WriteRawTag(50);
        output.WriteString(TransactTime);
      }
      if (HasOrgId) {
        output.WriteRawTag(58);
        output.WriteString(OrgId);
      }
      if (HasUserId) {
        output.WriteRawTag(66);
        output.WriteString(UserId);
      }
      instruments_.WriteTo(output, _repeated_instruments_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (HasMdReqId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MdReqId);
      }
      if (HasApplToken) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ApplToken);
      }
      if (HasSubscriptionRequestType) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SubscriptionRequestType);
      }
      if (HasMdBookType) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MdBookType);
      }
      if (HasMarketDepth) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MarketDepth);
      }
      if (HasTransactTime) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(TransactTime);
      }
      if (HasOrgId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(OrgId);
      }
      if (HasUserId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UserId);
      }
      size += instruments_.CalculateSize(_repeated_instruments_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ApiMarketDataRequestBody other) {
      if (other == null) {
        return;
      }
      if (other.HasMdReqId) {
        MdReqId = other.MdReqId;
      }
      if (other.HasApplToken) {
        ApplToken = other.ApplToken;
      }
      if (other.HasSubscriptionRequestType) {
        SubscriptionRequestType = other.SubscriptionRequestType;
      }
      if (other.HasMdBookType) {
        MdBookType = other.MdBookType;
      }
      if (other.HasMarketDepth) {
        MarketDepth = other.MarketDepth;
      }
      if (other.HasTransactTime) {
        TransactTime = other.TransactTime;
      }
      if (other.HasOrgId) {
        OrgId = other.OrgId;
      }
      if (other.HasUserId) {
        UserId = other.UserId;
      }
      instruments_.Add(other.instruments_);
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
            MdReqId = input.ReadString();
            break;
          }
          case 18: {
            ApplToken = input.ReadString();
            break;
          }
          case 24: {
            SubscriptionRequestType = input.ReadInt32();
            break;
          }
          case 32: {
            MdBookType = input.ReadInt32();
            break;
          }
          case 42: {
            MarketDepth = input.ReadString();
            break;
          }
          case 50: {
            TransactTime = input.ReadString();
            break;
          }
          case 58: {
            OrgId = input.ReadString();
            break;
          }
          case 66: {
            UserId = input.ReadString();
            break;
          }
          case 74: {
            instruments_.AddEntriesFrom(input, _repeated_instruments_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// API market data request message
  /// </summary>
  public sealed partial class ApiMarketDataRequest : pb::IMessage<ApiMarketDataRequest> {
    private static readonly pb::MessageParser<ApiMarketDataRequest> _parser = new pb::MessageParser<ApiMarketDataRequest>(() => new ApiMarketDataRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ApiMarketDataRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.ApiMarketDataRequestReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataRequest(ApiMarketDataRequest other) : this() {
      header_ = other.header_ != null ? other.header_.Clone() : null;
      body_ = other.body_ != null ? other.body_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataRequest Clone() {
      return new ApiMarketDataRequest(this);
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
    private global::Ctrade.Message.ApiMarketDataRequestBody body_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ctrade.Message.ApiMarketDataRequestBody Body {
      get { return body_; }
      set {
        body_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ApiMarketDataRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ApiMarketDataRequest other) {
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
    public void MergeFrom(ApiMarketDataRequest other) {
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
          Body = new global::Ctrade.Message.ApiMarketDataRequestBody();
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
              Body = new global::Ctrade.Message.ApiMarketDataRequestBody();
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
