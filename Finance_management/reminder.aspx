<%@ Page Title=""  EnableEventValidation="false" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="reminder.aspx.cs" Inherits="Finance_management.reminder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="Reminder">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h2 style="text-align: center">Set Reminder</h2>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="form-group">
                         <label>Enter Reminder Date:</label>
                         <asp:TextBox ID="ReDate" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group">
                         <label>Enter Reminder Title:</label>
                         <asp:TextBox ID="ReTittle" TextMode="SingleLine" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="email">Remark: </label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="ReRemark"></asp:TextBox>
                    </div>
                    <asp:Button runat="server" OnClick="btnclick_save" ID="btnSave" Text="Save" CssClass="btn btn-primary"  OnClientClick="return valid();"/>
                </div>
            </div>
        </div>
        <div class="container" style="margin-top:100px">
         <h3 class="my-3" style="text-align: center">Note List</h3>
         <div class="row">
             <div class="col-md-2"></div>
             <div class="col-md-8">
                 <div class="table-responsive">
                     <asp:GridView ID="gvdata" CssClass="table table-striped" runat="server" DataKeyNames="srno" OnRowDeleting="gvdata_RowDeleting" AutoGenerateColumns="false" EmptyDataText="No data found" ItemStyle-Width="800">
                     <Columns>
                         <asp:TemplateField HeaderText="Sr No" ItemStyle-Width="70">
                             <ItemTemplate>
                                 <asp:Label ID="srno" runat="server" Text='<%# Eval("srno") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText="Date" ItemStyle-Width="150">
                             <ItemTemplate>
                                 <asp:Label ID="remdate" runat="server" Text='<%# Eval("re_date") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText="title" ItemStyle-Width="150">
                             <ItemTemplate>
                                 <asp:Label ID="remtitle" runat="server" Text='<%# Eval("re_tittle") %>'></asp:Label>
                             </ItemTemplate>

                          </asp:TemplateField>
                         <asp:TemplateField HeaderText="Remark" ItemStyle-Width="150">
                             <ItemTemplate>
                                 <asp:Label ID="remremark" runat="server" Text='<%# Eval("re_remark") %>'></asp:Label>
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
        var ReDate = document.getElementById('<%= this.ReDate.ClientID %>').value;
        var ReTittle = document.getElementById('<%= this.ReTittle.ClientID %>').value;

        if (ReDate == "" || ReTittle == "" ) {
            swal("Please fill all details to proceed..!", "", "warning");
            return false;
        }
        return true;
    </script>
</asp:Content>
