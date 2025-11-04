<%@ Page Title="AMS | Winner Reports" Language="C#" MasterPageFile="~/admin.Master" AutoEventWireup="true" CodeBehind="winnerreports.aspx.cs" Inherits="AMS_3sem.adminpage.winnerreports" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="toaster-alert" id="toaster"></div>
    <asp:Label ID="exequery" runat="server" Text="" CssClass="text-gray-800 mb-2"></asp:Label>

    <h1 class="text-4xl text-center font-semibold mb-6">Winner Reports</h1>
            <form id="mainForm" runat="server" class=" flex animate__animated animate__fadeIn animate__faster border rounded-md bg-white shadow-md">

    <div class="bg-purple-200 border border-gray-200 rounded shadow p-6">
                    <asp:Button ID="Button2" runat="server" CssClass="btn  text-black p-1 rounded hover:border hover:bg-red-600 btn-right" Text="Genrate Report" OnClick="btnDownloadReport_Click" />


         

            <table id="Winnerreport" class="display responsive" width="100%">
                            <thead>
                    <tr class="bg-purple-100 text-center">
                        <th class="py-2 px-4 border-b">No</th>
                        <th class="py-2 px-4 border-b">Product Image</th>
                        <th class="py-2 px-4 border-b">Winner Name</th>
                        <th class="py-2 px-4 border-b">Contact</th>
                        <th class="py-2 px-4 border-b">Product</th>
                        <th class="py-2 px-4 border-b">Min Price</th>
                        <th class="py-2 px-4 border-b">Winning Price</th>
                        <th class="py-2 px-4 border-b">Action</th>

                    </tr>
                </thead>

                <tbody class="text-center">
                    <asp:Repeater ID="WinnerreportDataTable" runat="server">
                        <ItemTemplate>
                            <tr class="bg-purple-300 hover:bg-purple-100 text-center transition-all">
                                <td class="py-2 px-4 border-b "><%# Container.ItemIndex + 1 %></td>
                                <td class="py-2 px-4 border-b">
                                    <div class="relative">
                                        <img src='<%# ResolveUrl("~/Uploads/product_img/") + Eval("FileName") %>' alt="Product Image" class="w-16 h-16 object-cover rounded-full hover:scale-150 transition-transform duration-300" />
                                    </div>
                                </td>
                                <td class="py-2 px-4 border-b"><%# Eval("WinnerName") %></td>
                                <td class="py-2 px-4 border-b"><a href="tel:<%# Eval("Contact") %>"><%# Eval("Contact") %></a></td>
                                <td class="py-2 px-4 border-b"><%# Eval("Product") %></td>
                                <td class="py-2 px-4 border-b"><%# Eval("MinPrice") %></td>
                                <td class="py-2 px-4 border-b"><%# Eval("WinningPrice") %></td>
                                <td class="py-2 px-4 border-b">
                                    <asp:Button ID="Button1" runat="server" Text="Place Order" OnClick="btnorder_Click" CssClass="btn btn-info" />
                                    <asp:HiddenField ID="OrderNoHiddenField" Value='<%# Eval("AuctionItemId") %>' runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </tbody>
            </table>
    </div>
        </form>

    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

    <script src="https://cdn.datatables.net/1.11.5/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.11.5/js/dataTables.bootstrap4.min.js"></script>

    <script>
        $(document).ready(function () {
            var table = $('#Winnerreport').DataTable({
                responsive: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                autoWidth: true,
                columnDefs: [
                    { orderable: false, targets: [0, 9] }
                ],
                language: {
                    search: "Filter: ",
                    info: "Showing _START_ to _END_ of _TOTAL_ items",
                    lengthMenu: "Show _MENU_ items",
                    paginate: {
                        first: 'First',
                        last: 'Last',
                        next: 'Next',
                        previous: 'Previous'
                    }
                }
            });
        });
    </script>
</asp:Content>
