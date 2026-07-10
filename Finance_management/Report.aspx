<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Finance_management.Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.1/dist/chart.umd.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-container report-box">
        <h1 class="title" style="text-align:center">Profit / Loss Report</h1>

        <div class="row gutters">
            <div class="col-xl-4 col-md-4 col-sm-12">
                <div class="form-group">
                    <label>From <span style="color: red">*</span></label>
                    <asp:TextBox runat="server" TextMode="Date" CssClass="form-control" ID="txtfrom" />
                </div>
            </div>
            <div class="col-xl-4 col-md-4 col-sm-12">
                <div class="form-group">
                    <label>To <span style="color: red">*</span></label>
                    <asp:TextBox runat="server" TextMode="Date" CssClass="form-control" ID="txtto" />
                </div>
            </div>
            <div class="col-xl-4 col-md-4 col-sm-12">
                <div class="form-group">
                    <label>&nbsp;</label>
                    <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Generate Report" CssClass="btn btn-primary d-block" />
                </div>
            </div>
        </div>

        <asp:Panel ID="pnlResults" runat="server" Visible="false">
            <div class="dashboard-cards" style="margin-top: 24px;">
                <div class="dashboard-card income-card">
                    <p class="card-label">Total income</p>
                    <asp:Label ID="lblIncome" runat="server" CssClass="card-value text-success" />
                </div>
                <div class="dashboard-card expense-card">
                    <p class="card-label">Total expense</p>
                    <asp:Label ID="lblExpense" runat="server" CssClass="card-value text-danger" />
                </div>
                <div class="dashboard-card balance-card">
                    <p class="card-label">Net savings</p>
                    <asp:Label ID="lblSavings" runat="server" CssClass="card-value" />
                </div>
            </div>

            <h4 class="dashboard-heading">Income by category</h4>
            <asp:GridView runat="server" CssClass="table table-striped" ID="gvIncomeCat" AutoGenerateColumns="false" EmptyDataText="No income in this period">
                <Columns>
                    <asp:BoundField DataField="Category" HeaderText="Category" />
                    <asp:BoundField DataField="Total" HeaderText="Amount" DataFormatString="{0:N2}" />
                </Columns>
            </asp:GridView>

            <h4 class="dashboard-heading">Expense by category</h4>
            <asp:GridView runat="server" CssClass="table table-striped" ID="gvExpenseCat" AutoGenerateColumns="false" EmptyDataText="No expense in this period">
                <Columns>
                    <asp:BoundField DataField="Category" HeaderText="Category" />
                    <asp:BoundField DataField="Total" HeaderText="Amount" DataFormatString="{0:N2}" />
                </Columns>
            </asp:GridView>

            <div class="chart-row">
                <div class="chart-box">
                    <h5>Income vs Expense</h5>
                    <canvas id="plChartSummary" height="200"></canvas>
                </div>
                <div class="chart-box">
                    <h5>Expense breakdown</h5>
                    <canvas id="plChartExpense" height="200"></canvas>
                </div>
                <div class="chart-box">
                    <h5>Income breakdown</h5>
                    <canvas id="plChartIncome" height="200"></canvas>
                </div>
            </div>
            <asp:Literal ID="litCharts" runat="server" />
        </asp:Panel>
    </div>
</asp:Content>
