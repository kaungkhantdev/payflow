<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="PayPal_Payments_Advanced_Demo._Default" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="BodyContentDiv" runat="server">
       <h1>PayPal Payments Advanced - basic demo</h1>
       <p>Hosted checkout page (Layout C) embedded in your site as an iframe</p>
       <div id="AdvancedDemoContent" style="font-size:120%; margin-left:30px;" runat="server">
       </div>
       <br /><br />
       <hr />
          <h1>References</h1>
       <hr />
       <ul>
           <li><a href="https://cms.paypal.com/cms_content/US/en_US/files/developer/PayflowGateway_Guide.pdf">Payflow Gateway Developer Guide</a></li>
           <li><a href="https://cms.paypal.com/cms_content/US/en_US/files/developer/Embedded_Checkout_Design_Guide.pdf">Embedded Checkout Design Guide</a></li>
           <li><a href="https://www.x.com/developers/community/blogs/pp_integrations_preston/testing-paypal-payflow-gateway">Testing with the PayPal Payflow Gateway</a></li>
       </ul>
   </div>
</asp:Content>
