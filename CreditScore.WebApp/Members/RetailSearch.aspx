<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="RetailSearch.aspx.cs"
    Inherits="CreditScore.WebApp.Members.RetailSearch" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="dgvSearch" runat="server" CssClass="rspnsvTbl" style="width:10px">
    </asp:GridView>
    <div class="col-md-3 col-sm-4 col-xs-6 form-group">
        <asp:Literal runat="server"
            Text="<%$ Resources:BuisResource, BirthDate%>" />
        <input type="text"
            placeholder="<%$ Resources:BuisResource, SelectDateMsg%>"
            class="prsnDate" id="cmbSchBirthDate" runat="server" />
    </div>
        <label>
            <asp:Literal runat="server" Text="<%$ Resources:BuisResource, NationalID%>" /></label>
        <input type="text" class="form-control" id="txtSchNational" value="123" runat="server" />
        <button id="btnSearch" type="button" runat="server">
            <span class="glyphicon glyphicon-search"></span>
            <asp:Literal runat="server" Text="<%$ Resources:localResource, Search%>" />
        </button>
   
    
</asp:Content>

