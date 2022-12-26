#pragma checksum "D:\project\ESCS_PORTAL\ESCS_PORTAL\Areas\Compensation\Views\HealthCompensation\_Modal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "49edc5abfc73ae818880d3d6ce013302bbd32564"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Compensation_Views_HealthCompensation__Modal), @"mvc.1.0.view", @"/Areas/Compensation/Views/HealthCompensation/_Modal.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"49edc5abfc73ae818880d3d6ce013302bbd32564", @"/Areas/Compensation/Views/HealthCompensation/_Modal.cshtml")]
    public class Areas_Compensation_Views_HealthCompensation__Modal : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_HealthCompensationContentTab1", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_HealthCompensationContentTab2", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<!-- Modal -->\r\n");
            WriteLiteral(@"<div class=""modal top fade"" id=""modalXemThongTinHoSo"" tabindex=""-1"" aria-hidden=""true"" data-mdb-backdrop=""static"" data-mdb-keyboard=""false"" data-mdb-modal-non-invasive=""true"">
    <div class=""modal-dialog modal-fullscreen"">
        <div class=""modal-content"">
            <div class=""modal-header p-0 pe-md-3"">
                <ul class=""nav nav-tabs nav-fill"" role=""tablist"">
                    <li class=""nav-item"" role=""presentation"">
                        <a class=""nav-link py-3 mt-1 text-primary disabled"">
                            Số hồ sơ:&nbsp;<span id=""modalXemThongTinHoSoTitleSoHoSo""></span>
                        </a>
                    </li>
                    <li class=""nav-item"" role=""presentation"">
                        <a class=""nav-link py-3 mt-1 active defaultFisrtTab""
                           id=""tab-1""
                           data-mdb-toggle=""tab""
                           href=""#HealthCompensationContent1""
                           role=""tab""
                 ");
            WriteLiteral(@"          aria-controls=""tabs-1""
                           aria-selected=""true"">
                            <i class=""fa-regular fa-pen-to-square me-3""></i>Thông tin hồ sơ
                        </a>
                    </li>
                    <li class=""nav-item"" role=""presentation"">
                        <a class=""nav-link py-3 mt-1""
                           id=""tab-2""
                           data-mdb-toggle=""tab""
                           href=""#HealthCompensationContent2""
                           role=""tab""
                           aria-controls=""tabs-2""
                           aria-selected=""false"">
                            <i class=""fas fa-camera-retro me-3""></i>Tài liệu hình ảnh
                        </a>
                    </li>
                </ul>
                <button type=""button"" class=""btn-close d-none d-md-block"" data-mdb-dismiss=""modal"" aria-label=""Close""></button>
            </div>
            <div class=""modal-body p-0"">
                <div ");
            WriteLiteral(@"class=""tab-content h-100"" id=""HealthCompensationContent"">
                    <div class=""tab-pane fade h-100 show active""
                         id=""HealthCompensationContent1""
                         role=""tabpanel""
                         aria-labelledby=""tab-1"">
                        <div class=""container-fluid h-100 px-0"">
                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "49edc5abfc73ae818880d3d6ce013302bbd325645947", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                        </div>
                    </div>
                    <div class=""tab-pane fade h-100""
                         id=""HealthCompensationContent2""
                         role=""tabpanel""
                         aria-labelledby=""tab-2"">
                        <div class=""container-fluid h-100 px-0"">
                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "49edc5abfc73ae818880d3d6ce013302bbd325647431", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                        </div>
                    </div>
                </div>
            </div>
            <div class=""modal-footer p-2"">
                <button type=""button"" class=""btn btn-primary"" data-mdb-dismiss=""modal"">
                    <i class=""fa-solid fa-rectangle-xmark me-1""></i> Đóng
                </button>
            </div>
        </div>
    </div>
</div>


<div class=""modal fade"" id=""modalDanhMuc"" tabindex=""-1"" aria-hidden=""true"" data-mdb-backdrop=""false"" data-mdb-keyboard=""false"" data-mdb-modal-non-invasive=""true"">
    <div class=""modal-dialog modal-sm modal-side modal-bottom-left"" style=""width:240px;"">
        <div class=""modal-content overflow-hidden shadow-3-strong"">
            <div class=""modal-header border-bottom-0 py-2 bg-primary text-white"">
                <h5 class=""modal-title"">Danh mục</h5>
                <a class=""link-light float-right""
                    data-mdb-toggle=""collapse""
                    href=""#modalDanhMuc_body""
               ");
            WriteLiteral(@"     role=""button""
                    aria-expanded=""false""><i class=""fas fa-expand-alt""></i></a>
                <button type=""button"" class=""btn-close btn-close-white"" data-mdb-dismiss=""modal"" aria-label=""Close""></button>
            </div>
            <div class=""modal-body p-0 collapse show"" id=""modalDanhMuc_body"">
                <div class=""container-fluid py-2 overflow-auto"" style=""height:335px;"">
                    <div class=""list-group list-group-light"" id=""list-tab"" role=""tablist"">
                        <a class=""list-group-item list-group-item-action active px-3 border-0 mb-2 defaultFisrtTab""
                           data-mdb-toggle=""list"" href=""#tabThongTinChung"" role=""tab"" aria-controls=""list-home"" onclick=""onClickTitleTabMenu('Thông tin chung')"">Thông tin chung</a>
                        <a class=""list-group-item list-group-item-action px-3 border-0 mb-2""
                           data-mdb-toggle=""list"" href=""#tabThongTinGCN"" role=""tab"" aria-controls=""list-profile"" onclick=""on");
            WriteLiteral(@"ClickTitleTabMenu('Thông tin giấy chứng nhận')"">Thông tin giấy chứng nhận</a>
                        <a class=""list-group-item list-group-item-action px-3 border-0 mb-2""
                           data-mdb-toggle=""list"" href=""#tabThongTinHoSoGiayTo"" role=""tab"" aria-controls=""list-messages"" onclick=""onClickTitleTabMenu('Thông tin hồ sơ giấy tờ')"">Thông tin hồ sơ giấy tờ</a>
                        <a class=""list-group-item list-group-item-action px-3 border-0 mb-2""
                           data-mdb-toggle=""list"" href=""#tabThongTinYCBH"" role=""tab"" aria-controls=""list-settings"" onclick=""onClickTitleTabMenu('Thông tin yêu cầu bảo hiểm')"">Thông tin yêu cầu bảo hiểm</a>
                        <a class=""list-group-item list-group-item-action px-3 border-0 mb-2""
                           data-mdb-toggle=""list"" href=""#tabThongTinLichSuTonThat"" role=""tab"" aria-controls=""list-settings"" onclick=""onClickTitleTabMenu('Thông tin lịch sử tổn thất')"">Thông tin lịch sử tổn thất</a>
                        <a class=");
            WriteLiteral(@"""list-group-item list-group-item-action px-3 border-0 mb-2""
                           data-mdb-toggle=""list"" href=""#tabQuaTrinhXuLy"" role=""tab"" aria-controls=""list-settings"" onclick=""onClickTitleTabMenu('Thông tin quá trình xử lý')"">Thông tin quá trình xử lý</a>
                    </div>
                </div>
            </div>
            <div class=""modal-footer"">
                <button type=""button"" class=""btn btn-outline-primary"" data-mdb-dismiss=""modal"">Close</button>
            </div>
        </div>
    </div>
</div>");
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