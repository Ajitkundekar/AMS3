<%@ Page Title="" Language="C#" MasterPageFile="~/navfooter.Master" AutoEventWireup="true" CodeBehind="UPIPayment.aspx.cs" Inherits="AMS_3sem.UPIPayment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

     <div class="toaster-alert" id="toaster"></div>
    <div class="container mx-auto flex justify-center items-center h-screen">
        <form id="upiForm" runat="server" class="max-w-4xl mx-auto flex flex-col md:flex-row animate__animated animate__fadeIn animate__faster border rounded-md bg-white shadow-md p-4">
            <div class="md:w-1/2 p-4">
                <h2 class="text-3xl text-center text-[#68127f] font-semibold mb-2">UPI Payment</h2>
                <p class="text-gray-600 text-center mb-6">Enter your UPI ID and amount to proceed with the payment.</p>
                
                <div class="mb-4">
                    <label class="block text-gray-700 text-sm font-bold mb-2" for="txtUpiId">UPI ID</label>
                    <asp:TextBox ID="txtUpiId" runat="server" CssClass="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Enter your UPI ID" />
                    <asp:RequiredFieldValidator ID="rfvUpiId" runat="server" ControlToValidate="txtUpiId" Display="Dynamic" ErrorMessage="UPI ID is required." CssClass="text-red-500" />
                </div>

                <div class="mb-4">
                    <label class="block text-gray-700 text-sm font-bold mb-2" for="txtAmount">Amount</label>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Enter amount" />
                    <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount" Display="Dynamic" ErrorMessage="Amount is required." CssClass="text-red-500" />
                    <asp:RangeValidator ID="rvAmount" runat="server" ControlToValidate="txtAmount" Display="Dynamic" ErrorMessage="Amount must be between 1 and 100000." MinimumValue="1" MaximumValue="100000" CssClass="text-red-500" />
                </div>

                <asp:Button ID="btnSubmit" runat="server" CssClass="w-full bg-black text-white py-3 px-6 rounded-md transition duration-300 transform hover:scale-105 hover:bg-[#3e004f] focus:outline-none focus:ring focus:border-blue-300" Text="Pay Now"  />
            </div>
            <div class="md:w-1/2 p-4 flex justify-center items-center">
                <img id="form-image" src="img/upi.svg" class="mt-3" alt="UPI Payment" />
            </div>
        </form>
    </div>

</asp:Content>
