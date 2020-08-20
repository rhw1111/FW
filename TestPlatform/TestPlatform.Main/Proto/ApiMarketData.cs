// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: ApiMarketData.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ctrade.Message {

  /// <summary>Holder for reflection information generated from ApiMarketData.proto</summary>
  public static partial class ApiMarketDataReflection {

    #region Descriptor
    /// <summary>File descriptor for ApiMarketData.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ApiMarketDataReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChNBcGlNYXJrZXREYXRhLnByb3RvEg5jdHJhZGUubWVzc2FnZRoOc3RhbmRh",
            "cmQucHJvdG8aDEhlYWRlci5wcm90byKFAQoSQXBpTWFya2V0RGF0YUxldmVs",
            "EhUKDW1kX2VudHJ5X3R5cGUYASACKAUSFgoObWRfcHJpY2VfbGV2ZWwYAiAC",
            "KAUSEwoLbWRfZW50cnlfcHgYAyACKAkSFAoMdHJhZGVfdm9sdW1lGAQgAigJ",
            "EhUKDW1kX2VudHJ5X3NpemUYBSACKAki1wEKEUFwaU1hcmtldERhdGFCb2R5",
            "EhEKCW1kX3JlcV9pZBgBIAEoCRIVCg1tZF9lbnRyeV9kYXRlGAIgAigJEhUK",
            "DW1kX2VudHJ5X3RpbWUYAyACKAkSFwoGc3ltYm9sGAQgAigJOgdVU0QuQ05Z",
            "EhIKCnNldHRsX3R5cGUYBSACKAkSFAoMbWRfYm9va190eXBlGAYgAigFEj4K",
            "Em1hcmtldF9kYXRhX2xldmVscxgHIAMoCzIiLmN0cmFkZS5tZXNzYWdlLkFw",
            "aU1hcmtldERhdGFMZXZlbCJoCg1BcGlNYXJrZXREYXRhEiYKBmhlYWRlchgB",
            "IAEoCzIWLmN0cmFkZS5tZXNzYWdlLkhlYWRlchIvCgRib2R5GAIgAygLMiEu",
            "Y3RyYWRlLm1lc3NhZ2UuQXBpTWFya2V0RGF0YUJvZHlCUAogY24uY29tLmNm",
            "ZXRzLmRhdGEuY3RyYWRlLm1lc3NhZ2VCE0FwaU1hcmtldERhdGFQcm9idWaC",
            "tRgVQ1RSQURFLUFQSS1NQVJLRVREQVRB"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::StandardReflection.Descriptor, global::Ctrade.Message.HeaderReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.ApiMarketDataLevel), global::Ctrade.Message.ApiMarketDataLevel.Parser, new[]{ "MdEntryType", "MdPriceLevel", "MdEntryPx", "TradeVolume", "MdEntrySize" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.ApiMarketDataBody), global::Ctrade.Message.ApiMarketDataBody.Parser, new[]{ "MdReqId", "MdEntryDate", "MdEntryTime", "Symbol", "SettlType", "MdBookType", "MarketDataLevels" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ctrade.Message.ApiMarketData), global::Ctrade.Message.ApiMarketData.Parser, new[]{ "Header", "Body" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// Market date of price level
  /// </summary>
  public sealed partial class ApiMarketDataLevel : pb::IMessage<ApiMarketDataLevel> {
    private static readonly pb::MessageParser<ApiMarketDataLevel> _parser = new pb::MessageParser<ApiMarketDataLevel>(() => new ApiMarketDataLevel());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ApiMarketDataLevel> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.ApiMarketDataReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataLevel() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataLevel(ApiMarketDataLevel other) : this() {
      _hasBits0 = other._hasBits0;
      mdEntryType_ = other.mdEntryType_;
      mdPriceLevel_ = other.mdPriceLevel_;
      mdEntryPx_ = other.mdEntryPx_;
      tradeVolume_ = other.tradeVolume_;
      mdEntrySize_ = other.mdEntrySize_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataLevel Clone() {
      return new ApiMarketDataLevel(this);
    }

    /// <summary>Field number for the "md_entry_type" field.</summary>
    public const int MdEntryTypeFieldNumber = 1;
    private readonly static int MdEntryTypeDefaultValue = 0;

    private int mdEntryType_;
    /// <summary>
    /// trade size, 0-bid; 1-offer
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MdEntryType {
      get { if ((_hasBits0 & 1) != 0) { return mdEntryType_; } else { return MdEntryTypeDefaultValue; } }
      set {
        _hasBits0 |= 1;
        mdEntryType_ = value;
      }
    }
    /// <summary>Gets whether the "md_entry_type" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMdEntryType {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "md_entry_type" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMdEntryType() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "md_price_level" field.</summary>
    public const int MdPriceLevelFieldNumber = 2;
    private readonly static int MdPriceLevelDefaultValue = 0;

    private int mdPriceLevel_;
    /// <summary>
    /// price level, such as 1, 2, 3, 4, 5
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MdPriceLevel {
      get { if ((_hasBits0 & 2) != 0) { return mdPriceLevel_; } else { return MdPriceLevelDefaultValue; } }
      set {
        _hasBits0 |= 2;
        mdPriceLevel_ = value;
      }
    }
    /// <summary>Gets whether the "md_price_level" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMdPriceLevel {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "md_price_level" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMdPriceLevel() {
      _hasBits0 &= ~2;
    }

    /// <summary>Field number for the "md_entry_px" field.</summary>
    public const int MdEntryPxFieldNumber = 3;
    private readonly static string MdEntryPxDefaultValue = "";

    private string mdEntryPx_;
    /// <summary>
    /// the price in the current price level
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MdEntryPx {
      get { return mdEntryPx_ ?? MdEntryPxDefaultValue; }
      set {
        mdEntryPx_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "md_entry_px" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMdEntryPx {
      get { return mdEntryPx_ != null; }
    }
    /// <summary>Clears the value of the "md_entry_px" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMdEntryPx() {
      mdEntryPx_ = null;
    }

    /// <summary>Field number for the "trade_volume" field.</summary>
    public const int TradeVolumeFieldNumber = 4;
    private readonly static string TradeVolumeDefaultValue = "";

    private string tradeVolume_;
    /// <summary>
    /// total volume of all orders in the current price level
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string TradeVolume {
      get { return tradeVolume_ ?? TradeVolumeDefaultValue; }
      set {
        tradeVolume_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "trade_volume" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasTradeVolume {
      get { return tradeVolume_ != null; }
    }
    /// <summary>Clears the value of the "trade_volume" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearTradeVolume() {
      tradeVolume_ = null;
    }

    /// <summary>Field number for the "md_entry_size" field.</summary>
    public const int MdEntrySizeFieldNumber = 5;
    private readonly static string MdEntrySizeDefaultValue = "";

    private string mdEntrySize_;
    /// <summary>
    /// tradable volume for the user who are pushed in the current price level
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MdEntrySize {
      get { return mdEntrySize_ ?? MdEntrySizeDefaultValue; }
      set {
        mdEntrySize_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "md_entry_size" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMdEntrySize {
      get { return mdEntrySize_ != null; }
    }
    /// <summary>Clears the value of the "md_entry_size" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMdEntrySize() {
      mdEntrySize_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ApiMarketDataLevel);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ApiMarketDataLevel other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MdEntryType != other.MdEntryType) return false;
      if (MdPriceLevel != other.MdPriceLevel) return false;
      if (MdEntryPx != other.MdEntryPx) return false;
      if (TradeVolume != other.TradeVolume) return false;
      if (MdEntrySize != other.MdEntrySize) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HasMdEntryType) hash ^= MdEntryType.GetHashCode();
      if (HasMdPriceLevel) hash ^= MdPriceLevel.GetHashCode();
      if (HasMdEntryPx) hash ^= MdEntryPx.GetHashCode();
      if (HasTradeVolume) hash ^= TradeVolume.GetHashCode();
      if (HasMdEntrySize) hash ^= MdEntrySize.GetHashCode();
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
      if (HasMdEntryType) {
        output.WriteRawTag(8);
        output.WriteInt32(MdEntryType);
      }
      if (HasMdPriceLevel) {
        output.WriteRawTag(16);
        output.WriteInt32(MdPriceLevel);
      }
      if (HasMdEntryPx) {
        output.WriteRawTag(26);
        output.WriteString(MdEntryPx);
      }
      if (HasTradeVolume) {
        output.WriteRawTag(34);
        output.WriteString(TradeVolume);
      }
      if (HasMdEntrySize) {
        output.WriteRawTag(42);
        output.WriteString(MdEntrySize);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (HasMdEntryType) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MdEntryType);
      }
      if (HasMdPriceLevel) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MdPriceLevel);
      }
      if (HasMdEntryPx) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MdEntryPx);
      }
      if (HasTradeVolume) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(TradeVolume);
      }
      if (HasMdEntrySize) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MdEntrySize);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ApiMarketDataLevel other) {
      if (other == null) {
        return;
      }
      if (other.HasMdEntryType) {
        MdEntryType = other.MdEntryType;
      }
      if (other.HasMdPriceLevel) {
        MdPriceLevel = other.MdPriceLevel;
      }
      if (other.HasMdEntryPx) {
        MdEntryPx = other.MdEntryPx;
      }
      if (other.HasTradeVolume) {
        TradeVolume = other.TradeVolume;
      }
      if (other.HasMdEntrySize) {
        MdEntrySize = other.MdEntrySize;
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
            MdEntryType = input.ReadInt32();
            break;
          }
          case 16: {
            MdPriceLevel = input.ReadInt32();
            break;
          }
          case 26: {
            MdEntryPx = input.ReadString();
            break;
          }
          case 34: {
            TradeVolume = input.ReadString();
            break;
          }
          case 42: {
            MdEntrySize = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Market data body
  /// </summary>
  public sealed partial class ApiMarketDataBody : pb::IMessage<ApiMarketDataBody> {
    private static readonly pb::MessageParser<ApiMarketDataBody> _parser = new pb::MessageParser<ApiMarketDataBody>(() => new ApiMarketDataBody());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ApiMarketDataBody> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.ApiMarketDataReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataBody() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataBody(ApiMarketDataBody other) : this() {
      _hasBits0 = other._hasBits0;
      mdReqId_ = other.mdReqId_;
      mdEntryDate_ = other.mdEntryDate_;
      mdEntryTime_ = other.mdEntryTime_;
      symbol_ = other.symbol_;
      settlType_ = other.settlType_;
      mdBookType_ = other.mdBookType_;
      marketDataLevels_ = other.marketDataLevels_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketDataBody Clone() {
      return new ApiMarketDataBody(this);
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

    /// <summary>Field number for the "md_entry_date" field.</summary>
    public const int MdEntryDateFieldNumber = 2;
    private readonly static string MdEntryDateDefaultValue = "";

    private string mdEntryDate_;
    /// <summary>
    /// the date of the current market
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MdEntryDate {
      get { return mdEntryDate_ ?? MdEntryDateDefaultValue; }
      set {
        mdEntryDate_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "md_entry_date" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMdEntryDate {
      get { return mdEntryDate_ != null; }
    }
    /// <summary>Clears the value of the "md_entry_date" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMdEntryDate() {
      mdEntryDate_ = null;
    }

    /// <summary>Field number for the "md_entry_time" field.</summary>
    public const int MdEntryTimeFieldNumber = 3;
    private readonly static string MdEntryTimeDefaultValue = "";

    private string mdEntryTime_;
    /// <summary>
    /// the time of the current market
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MdEntryTime {
      get { return mdEntryTime_ ?? MdEntryTimeDefaultValue; }
      set {
        mdEntryTime_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "md_entry_time" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMdEntryTime {
      get { return mdEntryTime_ != null; }
    }
    /// <summary>Clears the value of the "md_entry_time" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMdEntryTime() {
      mdEntryTime_ = null;
    }

    /// <summary>Field number for the "symbol" field.</summary>
    public const int SymbolFieldNumber = 4;
    private readonly static string SymbolDefaultValue = global::System.Text.Encoding.UTF8.GetString(global::System.Convert.FromBase64String("VVNELkNOWQ=="), 0, 7);

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

    /// <summary>Field number for the "settl_type" field.</summary>
    public const int SettlTypeFieldNumber = 5;
    private readonly static string SettlTypeDefaultValue = "";

    private string settlType_;
    /// <summary>
    /// contract name
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string SettlType {
      get { return settlType_ ?? SettlTypeDefaultValue; }
      set {
        settlType_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "settl_type" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasSettlType {
      get { return settlType_ != null; }
    }
    /// <summary>Clears the value of the "settl_type" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearSettlType() {
      settlType_ = null;
    }

    /// <summary>Field number for the "md_book_type" field.</summary>
    public const int MdBookTypeFieldNumber = 6;
    private readonly static int MdBookTypeDefaultValue = 0;

    private int mdBookType_;
    /// <summary>
    /// market data type, 1-best market data; 2-price level market data; 105-public best market data;
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MdBookType {
      get { if ((_hasBits0 & 1) != 0) { return mdBookType_; } else { return MdBookTypeDefaultValue; } }
      set {
        _hasBits0 |= 1;
        mdBookType_ = value;
      }
    }
    /// <summary>Gets whether the "md_book_type" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool HasMdBookType {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "md_book_type" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMdBookType() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "market_data_levels" field.</summary>
    public const int MarketDataLevelsFieldNumber = 7;
    private static readonly pb::FieldCodec<global::Ctrade.Message.ApiMarketDataLevel> _repeated_marketDataLevels_codec
        = pb::FieldCodec.ForMessage(58, global::Ctrade.Message.ApiMarketDataLevel.Parser);
    private readonly pbc::RepeatedField<global::Ctrade.Message.ApiMarketDataLevel> marketDataLevels_ = new pbc::RepeatedField<global::Ctrade.Message.ApiMarketDataLevel>();
    /// <summary>
    /// price level market data
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Ctrade.Message.ApiMarketDataLevel> MarketDataLevels {
      get { return marketDataLevels_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ApiMarketDataBody);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ApiMarketDataBody other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MdReqId != other.MdReqId) return false;
      if (MdEntryDate != other.MdEntryDate) return false;
      if (MdEntryTime != other.MdEntryTime) return false;
      if (Symbol != other.Symbol) return false;
      if (SettlType != other.SettlType) return false;
      if (MdBookType != other.MdBookType) return false;
      if(!marketDataLevels_.Equals(other.marketDataLevels_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HasMdReqId) hash ^= MdReqId.GetHashCode();
      if (HasMdEntryDate) hash ^= MdEntryDate.GetHashCode();
      if (HasMdEntryTime) hash ^= MdEntryTime.GetHashCode();
      if (HasSymbol) hash ^= Symbol.GetHashCode();
      if (HasSettlType) hash ^= SettlType.GetHashCode();
      if (HasMdBookType) hash ^= MdBookType.GetHashCode();
      hash ^= marketDataLevels_.GetHashCode();
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
      if (HasMdEntryDate) {
        output.WriteRawTag(18);
        output.WriteString(MdEntryDate);
      }
      if (HasMdEntryTime) {
        output.WriteRawTag(26);
        output.WriteString(MdEntryTime);
      }
      if (HasSymbol) {
        output.WriteRawTag(34);
        output.WriteString(Symbol);
      }
      if (HasSettlType) {
        output.WriteRawTag(42);
        output.WriteString(SettlType);
      }
      if (HasMdBookType) {
        output.WriteRawTag(48);
        output.WriteInt32(MdBookType);
      }
      marketDataLevels_.WriteTo(output, _repeated_marketDataLevels_codec);
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
      if (HasMdEntryDate) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MdEntryDate);
      }
      if (HasMdEntryTime) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MdEntryTime);
      }
      if (HasSymbol) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Symbol);
      }
      if (HasSettlType) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(SettlType);
      }
      if (HasMdBookType) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MdBookType);
      }
      size += marketDataLevels_.CalculateSize(_repeated_marketDataLevels_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ApiMarketDataBody other) {
      if (other == null) {
        return;
      }
      if (other.HasMdReqId) {
        MdReqId = other.MdReqId;
      }
      if (other.HasMdEntryDate) {
        MdEntryDate = other.MdEntryDate;
      }
      if (other.HasMdEntryTime) {
        MdEntryTime = other.MdEntryTime;
      }
      if (other.HasSymbol) {
        Symbol = other.Symbol;
      }
      if (other.HasSettlType) {
        SettlType = other.SettlType;
      }
      if (other.HasMdBookType) {
        MdBookType = other.MdBookType;
      }
      marketDataLevels_.Add(other.marketDataLevels_);
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
            MdEntryDate = input.ReadString();
            break;
          }
          case 26: {
            MdEntryTime = input.ReadString();
            break;
          }
          case 34: {
            Symbol = input.ReadString();
            break;
          }
          case 42: {
            SettlType = input.ReadString();
            break;
          }
          case 48: {
            MdBookType = input.ReadInt32();
            break;
          }
          case 58: {
            marketDataLevels_.AddEntriesFrom(input, _repeated_marketDataLevels_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// API market data message
  /// </summary>
  public sealed partial class ApiMarketData : pb::IMessage<ApiMarketData> {
    private static readonly pb::MessageParser<ApiMarketData> _parser = new pb::MessageParser<ApiMarketData>(() => new ApiMarketData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ApiMarketData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ctrade.Message.ApiMarketDataReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketData(ApiMarketData other) : this() {
      header_ = other.header_ != null ? other.header_.Clone() : null;
      body_ = other.body_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ApiMarketData Clone() {
      return new ApiMarketData(this);
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
    private static readonly pb::FieldCodec<global::Ctrade.Message.ApiMarketDataBody> _repeated_body_codec
        = pb::FieldCodec.ForMessage(18, global::Ctrade.Message.ApiMarketDataBody.Parser);
    private readonly pbc::RepeatedField<global::Ctrade.Message.ApiMarketDataBody> body_ = new pbc::RepeatedField<global::Ctrade.Message.ApiMarketDataBody>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Ctrade.Message.ApiMarketDataBody> Body {
      get { return body_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ApiMarketData);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ApiMarketData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Header, other.Header)) return false;
      if(!body_.Equals(other.body_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (header_ != null) hash ^= Header.GetHashCode();
      hash ^= body_.GetHashCode();
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
      body_.WriteTo(output, _repeated_body_codec);
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
      size += body_.CalculateSize(_repeated_body_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ApiMarketData other) {
      if (other == null) {
        return;
      }
      if (other.header_ != null) {
        if (header_ == null) {
          Header = new global::Ctrade.Message.Header();
        }
        Header.MergeFrom(other.Header);
      }
      body_.Add(other.body_);
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
            body_.AddEntriesFrom(input, _repeated_body_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
