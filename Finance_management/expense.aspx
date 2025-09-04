<%@ Page Title=""  EnableEventValidation="false" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="expense.aspx.cs" Inherits="Finance_management.expense" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="add_expense">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h2 style="text-align: center">Add your Expenses</h2>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="form-group mb-3">
                         <label>Enter Date:</label>
                         <asp:TextBox ID="ExDate" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>     
                    <div class="form-group mb-3">
                        <label for="pwd">Category: <span style="color: red">*</span></label>
                        <asp:DropDownList CssClass="form-control" runat="server" ID="ExCat">
                            <asp:ListItem Value="Select" Text="Select Category"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Bill"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Food"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Home"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Fun"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Investments"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Shares"></asp:ListItem>                   
                        </asp:DropDownList>
                    </div>
                    <div class="form-group mb-3">
                        <label for="email">Amount: </label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="ExAmt"></asp:TextBox>
                    </div>
                    <div class="form-group mb-3">
                        <label for="email">Remark: </label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="ExRemark"></asp:TextBox>
                    </div>
                    <asp:Button runat="server" ID="btnSave" OnClick="btnclick_save" Text="Save" CssClass="btn btn-primary" OnClientClick="return valid();" />
                </div>
            </div>
        </div>
        <div class="container" style="margin-top:100px">
            <h1 class="my-3" style="text-align: center">Expense List</h1>
            <div class="row">
                <div class="col-md-2"></div>
                <div class="col-md-12">
                    <div class="table-responsive">
                        <asp:GridView ID="gvdata" CssClass="table table-striped" runat="server" DataKeyNames="srno" OnRowDeleting="gvdata_RowDeleting" AutoGenerateColumns="false" EmptyDataText="No data found" ItemStyle-Width="800">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No" ItemStyle-Width="70">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsrno" runat="server" Text='<%# Eval("srno") %>'></asp:Label>
                                    </ItemTemplate>          
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("ex_date") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Category" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcat" runat="server" Text='<%# Eval("ex_category") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="lblamount" runat="server" Text='<%# Eval("ex_amount") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="lblremark" runat="server" Text='<%# Eval("ex_remark") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:Button ID="deletebtn" CssClass="btn btn-danger" runat="server" CommandName="Delete"
                                            Text="Delete" OnClientClick="return confirm('Are you sure?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function valid() {
            var Exdate = document.getElementById('<%= this.ExDate.ClientID %>').value;
            var ExCat = document.getElementById('<%= this.ExCat.ClientID %>').value;
            var ExAmt = document.getElementById('<%= this.ExAmt.ClientID %>').value;

            if (Exdate == "" || ExCat == "" || ExAmt == "") {
                swal("Please fill all details to proceed..!", "", "warning");
                return false;
            }
            return true;
    </script>
</asp:Content>
