<%@ Page Title="" Language="C#" MasterPageFile="~/navfooter.Master" AutoEventWireup="true" CodeBehind="payment.aspx.cs" Inherits="AMS_3sem.payment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <a href="index.aspx" class="nav-link">Home<span class="active-underline"></span></a>
    <a href="Login.aspx" class="nav-link">Login<span class="active-underline"></span></a>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="toaster-alert" id="toaster"></div>
    <div class="container mx-auto flex justify-center items-center h-screen">

                    

        <form id="upiForm" runat="server" class="max-w-4xl mx-auto flex flex-col md:flex-row animate__animated animate__fadeIn animate__faster border rounded-md bg-white shadow-md p-4">
            <div class="md:w-1/2 p-4">
                <h2 class="text-3xl text-center text-[#68127f] font-semibold mb-2">UPI Payment</h2>
                <p class="text-gray-600 text-center mb-6">after genrating the QR ,scan and pay .  fill the  information  after that click to confirm payment button    .</p>
                
                <div class="mb-4">
                    <label class="block text-gray-700 text-sm font-bold mb-2" for="txtUpiId">Transaction ID</label>
                    <asp:TextBox ID="txttId" runat="server" CssClass="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Enter your UPI ID" />
                </div>

                <div class="mb-4">
                    <label class="block text-gray-700 text-sm font-bold mb-2" for="txtAmount">Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Enter amount" />
                </div>


                
                <asp:Button ID="btnConfirmPayment" runat="server" CssClass="w-full bg-green-600 text-white py-3 px-6 rounded-md transition duration-300 transform hover:scale-105 hover:bg-green-800 focus:outline-none focus:ring focus:border-blue-300 mt-4" Text="Confirm Payment" OnClick="btnConfirmPayment_Click" Visible="false" />
            </div>
            <div class="md:w-1/2 p-4 flex justify-center items-center">
                <asp:Image ID="imgQrCode" runat="server" CssClass="mt-3" Alt="UPI QR Code" Visible="false" />
            </div>
        </form>
    </div>
</asp:Content>