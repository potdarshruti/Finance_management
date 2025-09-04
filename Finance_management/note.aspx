<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="note.aspx.cs" Inherits="Finance_management.note" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="note-box">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h2 style="text-align: center">Add Note</h2>
                </div>
            
            </div>
        <div class="row">
            <div class="col-md-3"></div>

            <div class="col-md-6">
            <div class="form-group">
                 <label> Title:</label>
                 <asp:TextBox ID="NTittle" TextMode="SingleLine" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
                    <div class="form-group">
            <label>Note Description:</label>
    <asp:TextBox ID="NDesc" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>

    </div>
            <asp:Button runat="server" OnClick="btnclick_save" ID="btnSave" Text="Save" CssClass="btn btn-primary" OnClientClick="return valid();" />

        </div>
    </div>
            <div class="container" style="margin-top:100px">
 <h1 class="my-3" style="text-align: center">Note List</h1>
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
                 <asp:TemplateField HeaderText="Title" ItemStyle-Width="150">
                     <ItemTemplate>
                         <asp:Label ID="ntittle" runat="server" Text='<%# Eval("note_tittle") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Remark" ItemStyle-Width="150">
                     <ItemTemplate>
                         <asp:Label ID="nremark" runat="server" Text='<%# Eval("note_remark") %>'></asp:Label>
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
</div></div>
    <script>
function valid() {
    var NTittle = document.getElementById('<%= this.NTittle.ClientID %>').value;
    if (NTittle == "") {
        swal("Please fill all details to proceed..!", "", "warning");
        return false;
    }
    return true;
    </script>
</asp:Content>
