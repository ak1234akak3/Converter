<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BinaryConverter.aspx.cs" Inherits="converter.BinaryConverter" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <title>Binary Converter</title>
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f7f6; display: flex; justify-content: center; padding: 20px; }
        .container { background: white; padding: 30px; border-radius: 12px; box-shadow: 0 4px 15px rgba(0,0,0,0.1); width: 500px; }
        h2 { text-align: center; color: #333; margin-bottom: 25px; }
        
        /* Navigation Styles */
        .nav-bar { display: flex; gap: 10px; margin-bottom: 20px; border-bottom: 2px solid #f0f0f0; padding-bottom: 15px; }
        .nav-btn { flex: 1; padding: 10px; background: #f8f9fa; color: #333; border: 2px solid #ddd; border-radius: 6px; cursor: pointer; font-size: 14px; font-weight: 600; transition: all 0.3s; text-align: center; text-decoration: none; display: block; }
        .nav-btn:hover { background: #e9ecef; border-color: #007bff; }
        .nav-btn.active { background: #007bff; color: white; border-color: #007bff; }
        .nav-btn.active:hover { background: #0056b3; border-color: #0056b3; }
        
        .form-group { margin-bottom: 15px; }
        label { display: block; margin-bottom: 5px; font-weight: 600; color: #555; }
        select, input[type="text"], textarea { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 6px; box-sizing: border-box; font-family: 'Courier New', monospace; }
        .btn-convert { width: 100%; padding: 12px; background: #007bff; color: white; border: none; border-radius: 6px; cursor: pointer; font-size: 16px; }
        .btn-convert:hover { background: #0056b3; }
        .btn-convert:disabled { background: #6c757d; cursor: not-allowed; }
        
        .result-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 15px; margin-top: 20px; }
        .result-item { background: #f8f9fa; padding: 12px; border-radius: 6px; border: 1px solid #e9ecef; }
        .result-item label { font-weight: 600; color: #495057; font-size: 0.9em; }
        .result-item .value { font-family: 'Courier New', monospace; font-size: 1.1em; color: #007bff; word-break: break-all; margin-top: 5px; }
        
        .error-box { margin-top: 15px; padding: 15px; background: #f8d7da; border-left: 5px solid #dc3545; border-radius: 6px; color: #721c24; }
        .explanation-box { margin-top: 15px; padding: 15px; background: #fff3cd; border-left: 5px solid #ffc107; border-radius: 6px; font-size: 0.95em; color: #856404; line-height: 1.6; }
        .history-box { margin-top: 25px; border-top: 1px solid #eee; padding-top: 15px; }
        .history-item { font-size: 0.9em; color: #666; margin-bottom: 5px; border-bottom: 1px dashed #eee; padding-bottom: 3px; font-family: 'Courier New', monospace; }
        
        .validation-text { font-size: 0.85em; color: #6c757d; margin-top: 5px; }
        .bits-display { display: flex; flex-wrap: wrap; gap: 5px; margin-top: 10px; }
        .bit { width: 30px; height: 30px; display: flex; align-items: center; justify-content: center; background: #e9ecef; border-radius: 4px; font-weight: bold; font-family: 'Courier New', monospace; }
        .bit-1 { background: #007bff; color: white; }
        .bit-0 { background: #6c757d; color: white; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
        <div class="container">
            <h2>Binary Converter</h2>
            
            <!-- Navigation Bar -->
            <div class="nav-bar">
                <asp:LinkButton ID="btnUnitConverter" runat="server" CssClass="nav-btn" OnClick="btnUnitConverter_Click" CausesValidation="false">
                    Unit Converter
                </asp:LinkButton>
                <asp:LinkButton ID="btnBinaryConverter" runat="server" CssClass="nav-btn active" OnClick="btnBinaryConverter_Click" CausesValidation="false">
                    Binary Converter
                </asp:LinkButton>
            </div>
            
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-group">
                        <label>Select Conversion Type</label>
                        <asp:DropDownList ID="ddlConversionType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlConversionType_SelectedIndexChanged">
                            <asp:ListItem Value="BinaryToDecimal">Binary to Decimal</asp:ListItem>
                            <asp:ListItem Value="BinaryToOctal">Binary to Octal</asp:ListItem>
                            <asp:ListItem Value="BinaryToHex">Binary to Hexadecimal</asp:ListItem>
                            <asp:ListItem Value="DecimalToBinary">Decimal to Binary</asp:ListItem>
                            <asp:ListItem Value="DecimalToOctal">Decimal to Octal</asp:ListItem>
                            <asp:ListItem Value="DecimalToHex">Decimal to Hexadecimal</asp:ListItem>
                            <asp:ListItem Value="OctalToBinary">Octal to Binary</asp:ListItem>
                            <asp:ListItem Value="OctalToDecimal">Octal to Decimal</asp:ListItem>
                            <asp:ListItem Value="OctalToHex">Octal to Hexadecimal</asp:ListItem>
                            <asp:ListItem Value="HexToBinary">Hex to Binary</asp:ListItem>
                            <asp:ListItem Value="HexToDecimal">Hex to Decimal</asp:ListItem>
                            <asp:ListItem Value="HexToOctal">Hex to Octal</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label id="lblInputLabel" runat="server">Enter Value</label>
                        <asp:TextBox ID="txtInput" runat="server" placeholder="Enter value to convert..." MaxLength="50"></asp:TextBox>
                        <div class="validation-text" id="lblValidationHint" runat="server">Enter a valid binary number (0s and 1s)</div>
                    </div>

                    <asp:Button ID="btnConvert" runat="server" Text="Convert" CssClass="btn-convert" OnClick="btnConvert_Click" />

                    <!-- Results -->
                    <asp:Panel ID="pnlResults" runat="server" Visible="false">
                        <div class="result-grid">
                            <div class="result-item">
                                <label>Binary</label>
                                <div class="value" id="divBinaryResult" runat="server">-</div>
                            </div>
                            <div class="result-item">
                                <label>Decimal</label>
                                <div class="value" id="divDecimalResult" runat="server">-</div>
                            </div>
                            <div class="result-item">
                                <label>Octal</label>
                                <div class="value" id="divOctalResult" runat="server">-</div>
                            </div>
                            <div class="result-item">
                                <label>Hexadecimal</label>
                                <div class="value" id="divHexResult" runat="server">-</div>
                            </div>
                        </div>
                        
                        <!-- Bit Visualization for Binary -->
                        <div class="result-item" style="margin-top: 15px;">
                            <label>Binary Representation (Visual)</label>
                            <div class="bits-display" id="divBitsDisplay" runat="server"></div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="error-box">
                        <asp:Label ID="lblError" runat="server"></asp:Label>
                    </asp:Panel>

                    <asp:Panel ID="pnlExplanation" runat="server" Visible="false" CssClass="explanation-box">
                        <strong>How it was calculated:</strong><br />
                        <asp:Label ID="lblExplanation" runat="server"></asp:Label>
                    </asp:Panel>

                    <div class="history-box">
                        <label>Recent Conversions</label>
                        <asp:Repeater ID="rptHistory" runat="server">
                            <ItemTemplate>
                                <div class="history-item"><%# Container.DataItem %></div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>