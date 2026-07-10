<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Finance_management.Home" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.1/dist/chart.umd.min.js"></script>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="home-container">

        <img id="homeimg" src="Image/finance1.jpg" alt="Finance" />

        <div class="home-content">

            <p class="text-primary-emphasis" style="font-weight:800; font-size:50px">Welcome to Finance Management</p>

            <p>

                Manage your finances efficiently and effectively.<br />

                Track expenses, set budgets, and achieve your financial goals.

            </p>



            <asp:Panel ID="pnlDashboard" runat="server" Visible="false">

                <h4 class="dashboard-heading">Dashboard — <asp:Label ID="lblMonth" runat="server" /></h4>

                <div class="dashboard-cards">

                    <div class="dashboard-card income-card">

                        <p class="card-label">Income this month</p>

                        <asp:Label ID="lblIncome" runat="server" CssClass="card-value text-success" />

                    </div>

                    <div class="dashboard-card expense-card">

                        <p class="card-label">Expense this month</p>

                        <asp:Label ID="lblExpense" runat="server" CssClass="card-value text-danger" />

                    </div>

                    <div class="dashboard-card balance-card">

                        <p class="card-label">Net balance</p>

                        <asp:Label ID="lblBalance" runat="server" CssClass="card-value" />

                    </div>

                </div>
                <h4 runat="server" ID="headrem">Today's reminders</h4>

                <asp:GridView CssClass="table table-striped" ID="GridView1" runat="server" AutoGenerateColumns="False" EmptyDataText="No reminders for today">

                    <Columns>

                        <asp:BoundField DataField="re_tittle" HeaderText="Title" />

                        <asp:BoundField DataField="re_remark" HeaderText="Remark" />

                    </Columns>

                </asp:GridView>

    
                <h4 class="dashboard-heading">Budget Progress</h4>

                <asp:GridView CssClass="table table-striped" ID="gvBudget" runat="server" AutoGenerateColumns="False" EmptyDataText="No budgets defined">

                    <Columns>

                        <asp:BoundField DataField="ex_category" HeaderText="Category" />

                        <asp:TemplateField HeaderText="Budget">

                            <ItemTemplate><%# string.Format("{0:N2}", Eval("budget_amount")) %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Spent">

                            <ItemTemplate><%# string.Format("{0:N2}", Eval("spent_amount")) %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remaining">

                            <ItemTemplate><%# string.Format("{0:N2}", Eval("remaining")) %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Progress">
                            <ItemTemplate>
                                <div class="progress" style="height: 20px;">
                                    <div class='progress-bar <%# GetProgressClass(Eval("percent")) %>' role="progressbar"
                                        style='width: <%# Eval("percent") %>%'>
                                        <%# Eval("percent") %>%
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

                

                <h4 class="dashboard-heading">Recent transactions</h4>

                <asp:GridView CssClass="table table-striped" ID="gvRecent" runat="server" AutoGenerateColumns="False" EmptyDataText="No transactions yet">

                    <Columns>

                        <asp:TemplateField HeaderText="Date">

                            <ItemTemplate><%# Eval("TransDate", "{0:dd MMM yyyy}") %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Type">

                            <ItemTemplate>

                                <asp:Label runat="server" Text='<%# Eval("TransType") %>'

                                    CssClass='<%# Eval("TransType").ToString() == "Income" ? "badge bg-success" : "badge bg-danger" %>' />

                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:BoundField DataField="Category" HeaderText="Category" />

                        <asp:TemplateField HeaderText="Amount">

                            <ItemTemplate><%# string.Format("{0:N2}", Eval("Amount")) %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:BoundField DataField="Remark" HeaderText="Remark" />

                    </Columns>

                </asp:GridView>



                



                <asp:Literal ID="litCharts" runat="server" />

            </asp:Panel>

        </div>

    </div>

</asp:Content>

