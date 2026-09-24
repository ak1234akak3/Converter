<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="converter.Index" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Smart Converter</title>
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f7f6; display: flex; justify-content: center; padding: 20px; }
        .container { background: white; padding: 30px; border-radius: 12px; box-shadow: 0 4px 15px rgba(0,0,0,0.1); width: 450px; }
        h2 { text-align: center; color: #333; margin-bottom: 25px; }
        
        /* Navigation Styles */
        .nav-bar { display: flex; gap: 10px; margin-bottom: 20px; border-bottom: 2px solid #f0f0f0; padding-bottom: 15px; }
        .nav-btn { flex: 1; padding: 10px; background: #f8f9fa; color: #333; border: 2px solid #ddd; border-radius: 6px; cursor: pointer; font-size: 14px; font-weight: 600; transition: all 0.3s; text-align: center; text-decoration: none; display: block; }
        .nav-btn:hover { background: #e9ecef; border-color: #007bff; }
        .nav-btn.active { background: #007bff; color: white; border-color: #007bff; }
        .nav-btn.active:hover { background: #0056b3; border-color: #0056b3; }
        
        .form-group { margin-bottom: 15px; }
        label { display: block; margin-bottom: 5px; font-weight: 600; color: #555; }
        select, input[type="text"] { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 6px; box-sizing: border-box; }
        .btn-group { display: flex; gap: 10px; margin-top: 10px; }
        .btn-convert { flex: 2; padding: 12px; background: #007bff; color: white; border: none; border-radius: 6px; cursor: pointer; font-size: 16px; }
        .btn-explain { flex: 1; padding: 12px; background: #6c757d; color: white; border: none; border-radius: 6px; cursor: pointer; font-size: 16px; }
        .btn-convert:hover { background: #0056b3; }
        .btn-explain:hover { background: #5a6268; }
        .result-box { margin-top: 20px; padding: 15px; background: #e9ecef; border-radius: 6px; text-align: center; font-size: 1.2em; font-weight: bold; color: #007bff; }
        .explanation-box { margin-top: 15px; padding: 15px; background: #fff3cd; border-left: 5px solid #ffc107; border-radius: 6px; font-size: 0.95em; color: #856404; line-height: 1.5; }
        .history-box { margin-top: 25px; border-top: 1px solid #eee; padding-top: 15px; }
        .history-item { font-size: 0.9em; color: #666; margin-bottom: 5px; border-bottom: 1px dashed #eee; padding-bottom: 3px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
        <div class="container">
            <h2>Converter</h2>
            
            <!-- Navigation Bar -->
            <div class="nav-bar">
                <asp:LinkButton ID="btnUnitConverter" runat="server" CssClass="nav-btn active" OnClick="btnUnitConverter_Click" CausesValidation="false">
                    Unit Converter
                </asp:LinkButton>
                <asp:LinkButton ID="btnBinaryConverter" runat="server" CssClass="nav-btn" OnClick="btnBinaryConverter_Click" CausesValidation="false">
                    Binary Converter
                </asp:LinkButton>
            </div>
            
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-group">
                        <label>Category</label>
                        <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                            <asp:ListItem Value="Length">Length</asp:ListItem>
                            <asp:ListItem Value="Weight">Weight</asp:ListItem>
                            <asp:ListItem Value="Temperature">Temperature</asp:ListItem>
                            <asp:ListItem Value="Currency">Currency (Fixed)</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label>Value</label>
                        <asp:TextBox ID="txtInput" runat="server" placeholder="Enter value..."></asp:TextBox>
                    </div>

                    <div style="display:flex; gap:10px;">
                        <div class="form-group" style="flex:1;">
                            <label>From</label>
                            <asp:DropDownList ID="ddlFrom" runat="server"></asp:DropDownList>
                        </div>
                        <div class="form-group" style="flex:1;">
                            <label>To</label>
                            <asp:DropDownList ID="ddlTo" runat="server"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="btn-group">
                        <asp:Button ID="btnConvert" runat="server" Text="Convert" CssClass="btn-convert" OnClick="btnConvert_Click" />
                        <asp:Button ID="btnExplain" runat="server" Text="Explain" CssClass="btn-explain" OnClick="btnExplain_Click" Visible="false" />
                    </div>

                    <asp:Panel ID="pnlResult" runat="server" Visible="false" CssClass="result-box">
                        <asp:Label ID="lblResult" runat="server"></asp:Label>
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