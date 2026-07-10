<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="budget.aspx.cs" Inherits="Finance_management.budget" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="add_expense">
        <div class="container">
            <h2 style="text-align:center">Monthly Budget</h2>
            <p style="text-align:center">Set spending limits per expense category and track progress.</p>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="form-group mb-3">
                        <label>Month</label>
                        <asp:TextBox ID="txtMonth" TextMode="Month" CssClass="form-control" runat="server" />
                    </div>
                    <div class="form-group mb-3">
                        <label>Category <span style="color:red">*</span></label>
                        <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server">
                            <asp:ListItem Value="Select" Text="Select Category" />
                            <asp:ListItem Value="Bill" Text="Bill" />
                            <asp:ListItem Value="Food" Text="Food" />
                            <asp:ListItem Value="Home" Text="Home" />
                            <asp:ListItem Value="Fun" Text="Fun" />
                            <asp:ListItem Value="Investments" Text="Investments" />
                            <asp:ListItem Value="Shares" Text="Shares" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-group mb-3">
                        <label>Budget amount <span style="color:red">*</span></label>
                        <asp:TextBox ID="txtAmount" CssClass="form-control" runat="server" />
                    </div>
                    <asp:Button ID="btnSave" runat="server" Text="Save Budget" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>

        <div class="container" style="margin-top:60px">
            <h3 style="text-align:center">Budget vs spent — <asp:Label ID="lblMonthDisplay" runat="server" /></h3>
            <asp:GridView ID="gvBudget" CssClass="table table-striped" runat="server" AutoGenerateColumns="false"
                DataKeyNames="srno" OnRowDeleting="gvBudget_RowDeleting" EmptyDataText="No budgets set for this month">
                <Columns>
                    <asp:BoundField DataField="ex_category" HeaderText="Category" />
                    <asp:BoundField DataField="budget_amount" HeaderText="Budget" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="spent_amount" HeaderText="Spent" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="remaining" HeaderText="Remaining" DataFormatString="{0:N2}" />
                    <asp:TemplateField HeaderText="Progress">
                        <ItemTemplate>
                            <div class="progress" style="height:22px;">
                                <div class="progress-bar <%# GetProgressClass(Eval("percent")) %>"
                                    style='<%# "width:" + Eval("percent") + "%;" %>'>
                                    <%# Eval("percent", "{0:0}%") %>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm"
                                CommandName="Delete" OnClientClick="return confirm('Delete this budget?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
