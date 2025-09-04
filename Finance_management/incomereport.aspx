<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="incomereport.aspx.cs" Inherits="Finance_management.incomereport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="main-container report-box" style="text-align:center">
    <div class="page-title">
            <div class="row gutters">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <h1 class="title" style="text-align:center">Income Report</h1>
                </div>
            </div>
            <!-- Row start -->
            <div class="row gutters">

                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <div class="card">
                        <div class="card-header">
                            <div class="card-title">Income Report</div>
                        </div>
                        <div class="card-body">
                            <div class="row gutters">
                                <div class="col-xl-4 col-lglg-4 col-md-4 col-sm-4 col-12">
                                    <div class="form-group">
                                        <label for="txttitle">From <span style="color: red">*</span></label>
                                        <asp:TextBox runat="server" TextMode="Date" CssClass="form-control" ID="txtfrom"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xl-4 col-lglg-4 col-md-4 col-sm-4 col-12">
                                    <div class="form-group">
                                        <label for="txttitle">To <span style="color: red">*</span></label>
                                        <asp:TextBox runat="server" TextMode="Date" CssClass="form-control" ID="txtto"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xl-4 col-lglg-4 col-md-4 col-sm-4 col-12">
                                   <div class="form-group">
                                       <asp:Button runat="server"  ID="btnIncome" OnClick="btnIncome_Click" Text="Submit" CssClass="btn btn-primary my-1" />
                                   </div>
                                </div>

                                 <asp:GridView runat="server"   CssClass="display table table-striped table-bordered" Style="width: 100%" ID="gvRecords" EmptyDataText="No  Data Found"  DataKeyNames="Srno" AutoGenerateColumns="false">
                                <Columns>

                                    <asp:TemplateField HeaderText="Srno">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     
                                    <asp:BoundField DataField="in_date" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:TemplateField HeaderText="Income Date">
                                        <ItemTemplate>

                                            <asp:Label runat="server" Text='<%# Eval("in_date")%>'></asp:Label>

                                        </ItemTemplate>
                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Category">
                                        <ItemTemplate>

                                            <asp:Label runat="server" Text='<%# Eval("in_category")%>'></asp:Label>

                                        </ItemTemplate>
                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>

                                            <asp:Label runat="server" Text='<%# Eval("in_amount")%>'></asp:Label>

                                        </ItemTemplate>
                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>

                                            <asp:Label runat="server" Text='<%# Eval("in_remark")%>'></asp:Label>

                                        </ItemTemplate>
                                        
                                    </asp:TemplateField>
                                     
                                   


                                </Columns>

                            </asp:GridView>
                        <div style="margin-top: 20px">
                            <label>Total Amount : </label>
                            <asp:Label Style="font-size: 20px; font-weight: bold;" runat="server" ID="lbltotal"></asp:Label>
                            &#8377;
                        </div>

            </div></div></div>
</div>
		</div></div></div>
</asp:Content>
