<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Finance_management.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="login-box">
         <div class="container" style="margin-top: 70px">
            <div class="row">
                <div class="col-md-12">
                    <h2 style="text-align: center">Login</h2>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="form-group mb-3">
                        <label for="pwd">Email: <span style="color: red">*</span></label>
                        <asp:TextBox runat="server" TextMode="Email" CssClass="form-control" ID="txtEmail"></asp:TextBox>
                    </div>            
                    <div class="form-group mb-3">
                        <label for="pwd">Password: <span style="color: red">*</span></label>
                        <asp:TextBox runat="server" TextMode="Password" CssClass="form-control" ID="txtPassword"></asp:TextBox>
                    </div>
                    <asp:Button runat="server" OnClick="btnLogin_Click" ID="btnLogin" Text="Login" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
