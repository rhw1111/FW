// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: APIOrderSubmitRequestMsg.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ctrade.Message {

  /// <summary>Holder for reflection information generated from APIOrderSubmitRequestMsg.proto</summary>
  public static partial class APIOrderSubmitRequestMsgReflection {

    #region Descriptor
    /// <summary>File descriptor for APIOrderSubmitRequestMsg.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static APIOrderSubmitRequestMsgReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch5BUElPcmRlclN1Ym1pdFJlcXVlc3RNc2cucHJvdG8SDmN0cmFkZS5tZXNz",
            "YWdlGg5zdGFuZGFyZC5wcm90bxoMSGVhZGVyLnByb3RvGgpVc2VyLnByb3Rv",
            "Ip8CChVBUElPcmRlclN1Ym1pdFJlcXVlc3QSFAoMT3JkZXJfcmVxX2lkGAEg",
            "AigJEhsKD01hcmtldEluZGljYXRvchgCIAIoCToCMTESKAoYaW5zdF9zY29w",
            "ZV9zZWN1cml0eV90eXBlGAMgASgJOgZGWFNXQVASFwoGc3ltYm9sGAQgAigJ",
            "OgdVU0QuQ05ZEhIKCmluc3RydW1lbnQYBSACKAkSDwoHZGVhbERpchgGIAIo",
            "BRIRCglvcmRlclR5cGUYByACKAkSEwoLZXhwaXJlX3RpbWUYCCABKAkSDQoF",
            "cHJpY2UYCSACKAMSEAoIT3JkZXJRdHkYCiACKAMSIgoEdXNlchhkIAEoCzIU",
            "LmN0cmFkZS5tZXNzYWdlLlVzZXIidwoYQVBJT3JkZXJTdWJtaXRSZXF1ZXN0",
            "TXNnEiYKBmhlYWRlchgBIAEoCzIWLmN0cmFkZS5tZXNzYWdlLkhlYWRlchIz",
            "CgRib2R5GAIgASgLMiUuY3RyYWRlLm1lc3NhZ2UuQVBJT3JkZXJTdWJtaXRS",
            "ZXF1ZXN0Ql4KIGNuLmNvbS5jZmV0cy5kYXRhLmN0cmFkZS5tZXNzYWdlQh5B",
            "UElPcmRlclN1Ym1pdFJlcXVlc3RNc2dQcm9idWaCtRgYQ1RSQURFLU9SREVS",
            "LURBVEEtU1VCTUlU"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::StandardReflection.Descriptor, global::Ctrade.Message.HeaderReflection.Descriptor, global::Ctrade.Message.UserReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.APIOrderSubmitRequest), global::Ctrade.Message.APIOrderSubmitRequest.Parser, new[]{ "OrderReqId", "MarketIndicator", "InstScopeSecurityType", "Symbol", "Instrument", "DealDir", "OrderType", "ExpireTime", "Price", "OrderQty", "User" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.APIOrderSubmitRequestMsg), global::Ctrade.Message.APIOrderSubmitRequestMsg.Parser, new[]{ "Header", "Body" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///订单提交请求
  /// </summary>
  public sealed partial class APIOrderSubmitRequest : pb::IMessage<APIOrderSubmitRequest> {
    private static readonly pb::MessageParser<APIOrderSubmitRequest> _parser = new pb::MessageParser<APIOrderSubmitRequest>(() => new APIOrderSubmitRequest());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<APIOrderSubmitRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.APIOrderSubmitRequestMsgReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APIOrderSubmitRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APIOrderSubmitRequest(APIOrderSubmitRequest other) : this() {
      _hasBits0 = other._hasBits0;
      orderReqId_ = other.orderReqId_;
      marketIndicator_ = other.marketIndicator_;
      instScopeSecurityType_ = other.instScopeSecurityType_;
      symbol_ = other.symbol_;
      instrument_ = other.instrument_;
      dealDir_ = other.dealDir_;
      orderType_ = other.orderType_;
      expireTime_ = other.expireTime_;
      price_ = other.price_;
      orderQty_ = other.orderQty_;
      user_ = other.user_ != null ? other.user_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APIOrderSubmitRequest Clone() {
      return new APIOrderSubmitRequest(this);
    }

    /// <summary>Field number for the "Order_req_id" field.</summary>
    public const int OrderReqIdFieldNumber = 1;
    private readonly static string OrderReqIdDefaultValue = "";

    private string orderReqId_;
    /// <summary>
    //// 订单操作参考序号
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string OrderReqId {
      get { return orderReqId_ ?? OrderReqIdDefaultValue; }
      set {
        orderReqId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "Order_req_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasOrderReqId {
      get { return orderReqId_ != null; }
    }
    /// <summary>Clears the value of the "Order_req_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearOrderReqId() {
      orderReqId_ = null;
    }

    /// <summary>Field number for the "MarketIndicator" field.</summary>
    public const int MarketIndicatorFieldNumber = 2;
    private readonly static string MarketIndicatorDefaultValue = global::System.Text.Encoding.UTF8.GetString(global::System.Convert.FromBase64String("MTE="), 0, 2);

    private string marketIndicator_;
    /// <summary>
    //// 金融工具
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MarketIndicator {
      get { return marketIndicator_ ?? MarketIndicatorDefaultValue; }
      set {
        marketIndicator_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "MarketIndicator" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMarketIndicator {
      get { return marketIndicator_ != null; }
    }
    /// <summary>Clears the value of the "MarketIndicator" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMarketIndicator() {
      marketIndicator_ = null;
    }

    /// <summary>Field number for the "inst_scope_security_type" field.</summary>
    public const int InstScopeSecurityTypeFieldNumber = 3;
    private readonly static string InstScopeSecurityTypeDefaultValue = global::System.Text.Encoding.UTF8.GetString(global::System.Convert.FromBase64String("RlhTV0FQ"), 0, 6);

    private string instScopeSecurityType_;
    /// <summary>
    //// 金融工具
    /// </summary>
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

    /// <summary>Field number for the "symbol" field.</summary>
    public const int SymbolFieldNumber = 4;
    private readonly static string SymbolDefaultValue = global::System.Text.Encoding.UTF8.GetString(global::System.Convert.FromBase64String("VVNELkNOWQ=="), 0, 7);

    private string symbol_;
    /// <summary>
    //// 货币对
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

    /// <summary>Field number for the "instrument" field.</summary>
    public const int InstrumentFieldNumber = 5;
    private readonly static string InstrumentDefaultValue = "";

    private string instrument_;
    /// <summary>
    ////合约品种
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Instrument {
      get { return instrument_ ?? InstrumentDefaultValue; }
      set {
        instrument_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "instrument" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasInstrument {
      get { return instrument_ != null; }
    }
    /// <summary>Clears the value of the "instrument" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearInstrument() {
      instrument_ = null;
    }

    /// <summary>Field number for the "dealDir" field.</summary>
    public const int DealDirFieldNumber = 6;
    private readonly static int DealDirDefaultValue = 0;

    private int dealDir_;
    /// <summary>
    ////交易方向
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int DealDir {
      get { if ((_hasBits0 & 1) != 0) { return dealDir_; } else { return DealDirDefaultValue; } }
      set {
        _hasBits0 |= 1;
        dealDir_ = value;
      }
    }
    /// <summary>Gets whether the "dealDir" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasDealDir {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "dealDir" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearDealDir() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "orderType" field.</summary>
    public const int OrderTypeFieldNumber = 7;
    private readonly static string OrderTypeDefaultValue = "";

    private string orderType_;
    /// <summary>
    ////订单类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string OrderType {
      get { return orderType_ ?? OrderTypeDefaultValue; }
      set {
        orderType_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "orderType" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasOrderType {
      get { return orderType_ != null; }
    }
    /// <summary>Clears the value of the "orderType" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearOrderType() {
      orderType_ = null;
    }

    /// <summary>Field number for the "expire_time" field.</summary>
    public const int ExpireTimeFieldNumber = 8;
    private readonly static string ExpireTimeDefaultValue = "";

    private string expireTime_;
    /// <summary>
    ////有效时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ExpireTime {
      get { return expireTime_ ?? ExpireTimeDefaultValue; }
      set {
        expireTime_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "expire_time" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasExpireTime {
      get { return expireTime_ != null; }
    }
    /// <summary>Clears the value of the "expire_time" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearExpireTime() {
      expireTime_ = null;
    }

    /// <summary>Field number for the "price" field.</summary>
    public const int PriceFieldNumber = 9;
    private readonly static long PriceDefaultValue = 0L;

    private long price_;
    /// <summary>
    ////价格
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Price {
      get { if ((_hasBits0 & 2) != 0) { return price_; } else { return PriceDefaultValue; } }
      set {
        _hasBits0 |= 2;
        price_ = value;
      }
    }
    /// <summary>Gets whether the "price" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasPrice {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "price" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearPrice() {
      _hasBits0 &= ~2;
    }

    /// <summary>Field number for the "OrderQty" field.</summary>
    public const int OrderQtyFieldNumber = 10;
    private readonly static long OrderQtyDefaultValue = 0L;

    private long orderQty_;
    /// <summary>
    ////报价量
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long OrderQty {
      get { if ((_hasBits0 & 4) != 0) { return orderQty_; } else { return OrderQtyDefaultValue; } }
      set {
        _hasBits0 |= 4;
        orderQty_ = value;
      }
    }
    /// <summary>Gets whether the "OrderQty" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasOrderQty {
      get { return (_hasBits0 & 4) != 0; }
    }
    /// <summary>Clears the value of the "OrderQty" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearOrderQty() {
      _hasBits0 &= ~4;
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
      return Equals(other as APIOrderSubmitRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(APIOrderSubmitRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (OrderReqId != other.OrderReqId) return false;
      if (MarketIndicator != other.MarketIndicator) return false;
      if (InstScopeSecurityType != other.InstScopeSecurityType) return false;
      if (Symbol != other.Symbol) return false;
      if (Instrument != other.Instrument) return false;
      if (DealDir != other.DealDir) return false;
      if (OrderType != other.OrderType) return false;
      if (ExpireTime != other.ExpireTime) return false;
      if (Price != other.Price) return false;
      if (OrderQty != other.OrderQty) return false;
      if (!object.Equals(User, other.User)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HasOrderReqId) hash ^= OrderReqId.GetHashCode();
      if (HasMarketIndicator) hash ^= MarketIndicator.GetHashCode();
      if (HasInstScopeSecurityType) hash ^= InstScopeSecurityType.GetHashCode();
      if (HasSymbol) hash ^= Symbol.GetHashCode();
      if (HasInstrument) hash ^= Instrument.GetHashCode();
      if (HasDealDir) hash ^= DealDir.GetHashCode();
      if (HasOrderType) hash ^= OrderType.GetHashCode();
      if (HasExpireTime) hash ^= ExpireTime.GetHashCode();
      if (HasPrice) hash ^= Price.GetHashCode();
      if (HasOrderQty) hash ^= OrderQty.GetHashCode();
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
      if (HasOrderReqId) {
        output.WriteRawTag(10);
        output.WriteString(OrderReqId);
      }
      if (HasMarketIndicator) {
        output.WriteRawTag(18);
        output.WriteString(MarketIndicator);
      }
      if (HasInstScopeSecurityType) {
        output.WriteRawTag(26);
        output.WriteString(InstScopeSecurityType);
      }
      if (HasSymbol) {
        output.WriteRawTag(34);
        output.WriteString(Symbol);
      }
      if (HasInstrument) {
        output.WriteRawTag(42);
        output.WriteString(Instrument);
      }
      if (HasDealDir) {
        output.WriteRawTag(48);
        output.WriteInt32(DealDir);
      }
      if (HasOrderType) {
        output.WriteRawTag(58);
        output.WriteString(OrderType);
      }
      if (HasExpireTime) {
        output.WriteRawTag(66);
        output.WriteString(ExpireTime);
      }
      if (HasPrice) {
        output.WriteRawTag(72);
        output.WriteInt64(Price);
      }
      if (HasOrderQty) {
        output.WriteRawTag(80);
        output.WriteInt64(OrderQty);
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
      if (HasOrderReqId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(OrderReqId);
      }
      if (HasMarketIndicator) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MarketIndicator);
      }
      if (HasInstScopeSecurityType) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(InstScopeSecurityType);
      }
      if (HasSymbol) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Symbol);
      }
      if (HasInstrument) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Instrument);
      }
      if (HasDealDir) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(DealDir);
      }
      if (HasOrderType) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(OrderType);
      }
      if (HasExpireTime) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ExpireTime);
      }
      if (HasPrice) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Price);
      }
      if (HasOrderQty) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(OrderQty);
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
    public void MergeFrom(APIOrderSubmitRequest other) {
      if (other == null) {
        return;
      }
      if (other.HasOrderReqId) {
        OrderReqId = other.OrderReqId;
      }
      if (other.HasMarketIndicator) {
        MarketIndicator = other.MarketIndicator;
      }
      if (other.HasInstScopeSecurityType) {
        InstScopeSecurityType = other.InstScopeSecurityType;
      }
      if (other.HasSymbol) {
        Symbol = other.Symbol;
      }
      if (other.HasInstrument) {
        Instrument = other.Instrument;
      }
      if (other.HasDealDir) {
        DealDir = other.DealDir;
      }
      if (other.HasOrderType) {
        OrderType = other.OrderType;
      }
      if (other.HasExpireTime) {
        ExpireTime = other.ExpireTime;
      }
      if (other.HasPrice) {
        Price = other.Price;
      }
      if (other.HasOrderQty) {
        OrderQty = other.OrderQty;
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
            OrderReqId = input.ReadString();
            break;
          }
          case 18: {
            MarketIndicator = input.ReadString();
            break;
          }
          case 26: {
            InstScopeSecurityType = input.ReadString();
            break;
          }
          case 34: {
            Symbol = input.ReadString();
            break;
          }
          case 42: {
            Instrument = input.ReadString();
            break;
          }
          case 48: {
            DealDir = input.ReadInt32();
            break;
          }
          case 58: {
            OrderType = input.ReadString();
            break;
          }
          case 66: {
            ExpireTime = input.ReadString();
            break;
          }
          case 72: {
            Price = input.ReadInt64();
            break;
          }
          case 80: {
            OrderQty = input.ReadInt64();
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

  public sealed partial class APIOrderSubmitRequestMsg : pb::IMessage<APIOrderSubmitRequestMsg> {
    private static readonly pb::MessageParser<APIOrderSubmitRequestMsg> _parser = new pb::MessageParser<APIOrderSubmitRequestMsg>(() => new APIOrderSubmitRequestMsg());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<APIOrderSubmitRequestMsg> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.APIOrderSubmitRequestMsgReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APIOrderSubmitRequestMsg() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APIOrderSubmitRequestMsg(APIOrderSubmitRequestMsg other) : this() {
      header_ = other.header_ != null ? other.header_.Clone() : null;
      body_ = other.body_ != null ? other.body_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public APIOrderSubmitRequestMsg Clone() {
      return new APIOrderSubmitRequestMsg(this);
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
    private global::Ctrade.Message.APIOrderSubmitRequest body_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ctrade.Message.APIOrderSubmitRequest Body {
      get { return body_; }
      set {
        body_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as APIOrderSubmitRequestMsg);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(APIOrderSubmitRequestMsg other) {
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
    public void MergeFrom(APIOrderSubmitRequestMsg other) {
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
          Body = new global::Ctrade.Message.APIOrderSubmitRequest();
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
              Body = new global::Ctrade.Message.APIOrderSubmitRequest();
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