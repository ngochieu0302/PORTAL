#pragma checksum "D:\project\ESCS_PORTAL\ESCS_PORTAL\Views\Home\Error.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "40ae1b04a80e799fdf9109a424403a678ef86c96"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Error), @"mvc.1.0.view", @"/Views/Home/Error.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\project\ESCS_PORTAL\ESCS_PORTAL\Views\_ViewImports.cshtml"
using ESCS_PORTAL;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"40ae1b04a80e799fdf9109a424403a678ef86c96", @"/Views/Home/Error.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"960ffb0cffc228cef030108c369f2a8f064ec5f2", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Error : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\project\ESCS_PORTAL\ESCS_PORTAL\Views\Home\Error.cshtml"
  
    ViewData["Title"] = "Home Page";
    Layout = "_LayoutLogin";

#line default
#line hidden
#nullable disable
            WriteLiteral("<input type=\"hidden\" name=\"escs_domain\"");
            BeginWriteAttribute("value", " value=\"", 114, "\"", 137, 1);
#nullable restore
#line 5 "D:\project\ESCS_PORTAL\ESCS_PORTAL\Views\Home\Error.cshtml"
WriteAttributeValue("", 122, ViewBag.domain, 122, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" />
<div class=""h-100"" style=""background-color: #eef5f9"">
    <div class=""container py-5 h-100"">
        <div class=""row d-flex justify-content-center align-items-center h-100"">
            <div class=""col-12 col-md-8 col-lg-6 col-xl-5"">
                <div class=""card shadow-2-strong"">
                    <div class=""card-body text-center"">
                        <h1 class=""fw-bold  text-danger""><i class='fas fa-exclamation-triangle'></i>&nbsp;Lỗi!</h1>
                        <p>Không tìm thấy trang</p>
                        <a");
            BeginWriteAttribute("class", " class=\"", 686, "\"", 694, 0);
            EndWriteAttribute();
            WriteLiteral(" href=\"javascript:history.back();\"><i class=\"fas fa-arrow-left\"></i>&nbsp;Quay lại</a>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591