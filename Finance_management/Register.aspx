<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Finance_management.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="register-box">
         <div class="row">
             <div class="col-md-12">
                 <h2 style="text-align: center">Register Now</h2>
             </div>
         </div>
         <div class="row ">
             <div class="col-md-3">
                 <div class="form-group mb-3">
                     <label for="txtName">Name: <span style="color: red">*</span></label>
                     <asp:TextBox runat="server" CssClass="form-control large-textbox" ID="txtName"></asp:TextBox>
                 </div>
                 <div class="form-group mb-3">
                     <label for="txtContact">Contact: <span style="color: red">*</span></label>
                     <asp:TextBox runat="server" TextMode="Number" CssClass="form-control large-textbox" ID="txtContact"></asp:TextBox>
                 </div>
                 <div class="form-group mb-3">
                     <label for="txtEmail">Email:</label>
                     <asp:TextBox runat="server" TextMode="Email" CssClass="form-control large-textbox" ID="txtEmail"></asp:TextBox>
                 </div>
                 <div class="form-group mb-3">
                     <label for="ddlcat">Category: <span style="color: red">*</span></label>
                     <asp:DropDownList CssClass="form-control large-textbox" runat="server" ID="ddlcat">
                         <asp:ListItem Value="Select" Text="Select Category"></asp:ListItem>
                         <asp:ListItem Value="1" Text="Salaried"></asp:ListItem>
                         <asp:ListItem Value="2" Text="Business"></asp:ListItem>                   
                     </asp:DropDownList>
                 </div>
                 <div class="form-group mb-3">
                     <label for="txtPassword">Set Password: <span style="color: red">*</span></label>
                     <asp:TextBox runat="server" TextMode="Password" CssClass="form-control large-textbox" ID="txtPassword"></asp:TextBox>
                 </div>
                 <asp:Button runat="server" OnClick="btnSave_Click" ID="btnSave" Text="Register" CssClass="btn btn-primary" OnClientClick="return valid();" />
             </div>
         </div>
     </div>
    <script>
        function valid() {
            var name = document.getElementById('<%= this.txtName.ClientID %>').value;
            var email = document.getElementById('<%= this.txtEmail.ClientID %>').value;
            var phone = document.getElementById('<%= this.txtContact.ClientID %>').value;
            var dept = document.getElementById('<%= this.ddlcat.ClientID %>').value;
            var password = document.getElementById('<%= this.txtPassword.ClientID %>').value;

            //regex for validation
            let mobilecon = /^\d{10}$/;
            let emailcon = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([com\co\.\in])+$/;
    
            if (name == "" || email == "" || phone == "" || dept == "Select") {
                swal("Please fill all details to proceed..!","","warning");
                return false;
            }
            if (phone != '') {
                if (!phone.match(mobilecon)) {
                    swal("Please Enter Valid Contact Number", "", "error");
                    return false;

                }
            }
            if (email != '') {
                if (!email.match(emailcon)) {
                    swal("Please Enter Valid Email-Id", "", "warning");
                    return false;
                }
            }

            return true;
        }
    </script></asp:Content>
