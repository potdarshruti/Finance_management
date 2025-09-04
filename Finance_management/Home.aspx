<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Finance_management.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="home-container">
    <img id="homeimg" src="Image/finance1.jpg" alt="Finance" />
    <div >
        <p class="text-primary-emphasis" style="font-weight:800; font-size:50px">Welcome to Finance Management</p>
        <p>
            Manage your finances efficiently and effectively.<br />
            Track expenses, set budgets, and achieve your financial goals.
        </p>
        <h4 runat="server" ID="headrem">Reminder</h4>
        <asp:GridView  CssClass="table table-striped" ID="GridView1" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="Title" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="ReTittle" runat="server" Text='<%# Eval("re_tittle") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Remark" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="ReRemark" runat="server" Text='<%# Eval("re_remark") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>
</asp:Content>
