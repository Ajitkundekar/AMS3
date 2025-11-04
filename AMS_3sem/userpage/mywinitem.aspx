<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/userpage/User.Master" AutoEventWireup="true" CodeBehind="mywinitem.aspx.cs" Inherits="AMS_3sem.userpage.mywinitem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="flex items-center justify-center">
        <br />
        <div class="bg-purple-200 border border-gray-200 rounded shadow p-6">
            <h1 class="text-4xl text-center font-semibold mb-6">Auction Items</h1>
            <div class="bg-purple-200 border border-gray-200 rounded shadow p-6">
                <div class="overflow-x-auto">
                    <table id="AuctionItemsDataTableR" class="min-w-full">
                        <thead class="bg-purple-700 text-white">
                            <tr>
                                <th class="py-2 px-4 border-b">Order Number</th>
                                <th class="py-2 px-4 border-b">Date</th>
                                <th class="py-2 px-4 border-b">Email / Phone</th>
                                <th class="py-2 px-4 border-b">Product</th>
                                <th class="py-2 px-4 border-b">Delivery Address</th>
                                <th class="py-2 px-4 border-b">Amount</th>
                                <td class="py-2 px-4 border-b">Transaction ID</td>
                                <th class="py-2 px-4 border-b">Status</th>
                                <th class="py-2 px-4 border-b">Payment Status</th>
                            </tr>
                        </thead>
                        <tbody class="text-center">
                            <asp:Repeater ID="AllorderTableRecord" runat="server">
                                <ItemTemplate>
                                    <tr class="hover:bg-purple-100 transition-all">
                                        <td class="py-2 px-4 border-b"><%# Eval("orderno")%></td>
                                        <td class="py-2 px-4 border-b"><%# Eval("date") %></td>
                                        <td data-label="Email / Phone">
                                            <a href="mailto:<%# Eval("EmailAddress") %>"><%# Eval("EmailAddress") %></a>
                                            <br />
                                            <a href="tel:<%# Eval("PhoneNumber") %>" class="phone"><%# Eval("PhoneNumber") %></a>
                                        </td>
                                        <td class="py-2 px-4 border-b"><%# Eval("Product") %></td>
                                        <td class="py-2 px-4 border-b"><%# Eval("Address") %></td>
                                        <td class="py-2 px-4 border-b"><%# Eval("Amount") %></td>
                                        <td class="py-2 px-4 border-b"><%# Eval("transactionid") %></td>
                                        <td class="py-2 px-4 border-b">
                                            <asp:Button   runat="server"   ID="StatusButton"
                                                    class= "btn bg-green-800 text-white p-1 rounded hover:bg-purple-600" 
                                                         Text='<%# Eval("Status").ToString() == "1" ? "Processing" : "Delivered" %>'
                                                Enabled='false' />
                                            </td>
                                        <td class="py-2 px-4 border-b">
                                            <asp:Button ID="Button2" runat="server"
                                                CssClass='<%# Eval("transactionid") == DBNull.Value ? "btn bg-green-800 text-white p-1 rounded hover:bg-purple-600" : "btn bg-green-800 text-white p-1 rounded hover:bg-green-600"'
                                                Text='<%# Eval("transactionid") == DBNull.Value ? "pay" : "payment complete" %>'
                                                Enabled='<%# Eval("transactionid") == DBNull.Value ? true : false %>'
                                                OnClick="payment" />
                                            <asp:HiddenField ID="HiddenField1" Value='<%# Eval("orderno") %>' runat="server" />
                                            <asp:HiddenField ID="HiddenField2" Value='<%# Eval("Amount") %>' runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <script>
        var AllorderTableRecord = false;

        $(document).ready(function () {
            $('#AllorderTable').DataTable({
                "pagingType": "full_numbers",
                "lengthMenu": [10, 25, 50, 75, 100],
                "pageLength": 10,
                "ordering": true,
                "searching": true,
                "info": true,
                "responsive": true,
                "columnDefs": [
                    { "orderable": false, "targets": [6, 7] }
                ],
                "language": {
                    "emptyTable": "No data available",
                    "infoEmpty": "No records found",
                    "search": "_INPUT_",
                    "searchPlaceholder": "Search...",
                    "lengthMenu": "Show _MENU_ entries",
                    "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                    "paginate": {
                        "first": "First",
                        "last": "Last",
                        "next": "Next",
                        "previous": "Previous"
                    }
                }
            });
            AllorderTableRecord = true;
        });
    </script>
</asp:Content>
