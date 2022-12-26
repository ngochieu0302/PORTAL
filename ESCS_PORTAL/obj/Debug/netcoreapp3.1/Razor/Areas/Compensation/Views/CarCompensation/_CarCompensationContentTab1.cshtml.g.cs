#pragma checksum "D:\project\ESCS_PORTAL\ESCS_PORTAL\Areas\Compensation\Views\CarCompensation\_CarCompensationContentTab1.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "07ace94b72058f88f6a0e8579bd6fea188b8d990"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Compensation_Views_CarCompensation__CarCompensationContentTab1), @"mvc.1.0.view", @"/Areas/Compensation/Views/CarCompensation/_CarCompensationContentTab1.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"07ace94b72058f88f6a0e8579bd6fea188b8d990", @"/Areas/Compensation/Views/CarCompensation/_CarCompensationContentTab1.cshtml")]
    public class Areas_Compensation_Views_CarCompensation__CarCompensationContentTab1 : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<div id=""divThongTinChungHoSoXe"" class=""card h-100 shadow d-none"">
    <div class=""card-body d-flex flex-column gap-2 overflow-auto p-0"">
        <div class=""d-flex flex-nowrap h-100"">
            <div class=""collapse collapse-horizontal"" id=""tab1LeftMenu"">
                <div class=""h-100 py-2 ps-2"" style=""width:250px;"">
                    <div class=""border rounded d-flex flex-column h-100 p-1"">
                        <div class=""bg-primary rounded text-white fs-5 p-2 mb-2 text-center"">Danh mục</div>
                        <div class=""list-group list-group-light flex-fill overflow-auto"" style=""height:0;"" id=""list-tab"" role=""tablist"">
                            <a class=""list-group-item list-group-item-action active px-3 border-0 mb-2 defaultFisrtTab""
                               data-mdb-toggle=""list"" href=""#tabThongTinChung"" role=""tab"" aria-controls=""list-home"" onclick=""onClickTitleTabMenu('Thông tin chung')"">Thông tin chung hồ sơ</a>
                            <a class=""list-group-item l");
            WriteLiteral(@"ist-group-item-action px-3 border-0 mb-2""
                               data-mdb-toggle=""list"" href=""#tabThongTinGCN"" role=""tab"" aria-controls=""list-profile"" onclick=""onClickTitleTabMenu('Thông tin giấy chứng nhận')"">Thông tin giấy chứng nhận</a>
                            <a class=""list-group-item list-group-item-action px-3 border-0 mb-2""
                               data-mdb-toggle=""list"" href=""#tabThongTinVuTT"" role=""tab"" aria-controls=""list-profile"" onclick=""onClickTitleTabMenu('Thông tin vụ tổn thất')"">Thông tin vụ tổn thất</a>
                            <a class=""list-group-item list-group-item-action px-3 border-0 mb-2""
                               data-mdb-toggle=""list"" href=""#tabThongTinLichGiamDinh"" role=""tab"" aria-controls=""list-profile"" onclick=""onClickTitleTabMenu('Thông tin lịch giám định')"">Thông tin lịch giám định</a>
                            <a class=""list-group-item list-group-item-action px-3 border-0 mb-2 d-none""
                               data-mdb-toggle=""list"" href=");
            WriteLiteral("\"#tabThongTinHoSoGiayTo\" role=\"tab\" aria-controls=\"list-messages\" onclick=\"onClickTitleTabMenu(\'Thông tin hồ sơ giấy tờ\')\">Thông tin hồ sơ giấy tờ</a>\r\n");
            WriteLiteral(@"                            <a class=""list-group-item list-group-item-action px-3 border-0 mb-2""
                               data-mdb-toggle=""list"" href=""#tabThongTinLichSuTonThat"" role=""tab"" aria-controls=""list-settings"" onclick=""onClickTitleTabMenu('Thông tin lịch sử bồi thường')"">Thông tin lịch sử bồi thường</a>
                            <a class=""list-group-item list-group-item-action px-3 border-0 mb-2""
                               data-mdb-toggle=""list"" href=""#tabQuaTrinhXuLy"" role=""tab"" aria-controls=""list-settings"" onclick=""onClickTitleTabMenu('Thông tin quá trình xử lý')"">Thông tin quá trình xử lý</a>
                        </div>
                        <div class=""row g-1 mt-1"">
                            <div class=""col-6"">
                                <button class=""btn btn-block btn-primary"" type=""button"" onclick=""_multitabService.showTabByIndex(1);""><i class=""fas fa-file""></i></button>
                            </div>
                            <div class=""col-6"">
     ");
            WriteLiteral(@"                           <button class=""btn btn-block btn-secondary"" type=""button"" onclick=""_multitabService.showTabByIndex(2);setCollapse(true);""><i class=""fas fa-images""></i></button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class=""flex-fill"" style=""width:0;"">
                <div class=""h-100 p-2"">
                    <div class=""border rounded d-flex flex-column h-100 p-1"">
                        <div class=""bg-primary rounded text-white fs-5 p-2 mb-2 d-flex flex-nowrap gap-2 text-nowrap overflow-hidden"">
                            <button class=""navbar-toggler text-white me-2""
                                    data-mdb-toggle=""collapse""
                                    data-mdb-target=""#tab1LeftMenu""
                                    aria-expanded=""false""
                                    aria-controls=""tab1LeftMenu""
                                    aria-haspopup=""");
            WriteLiteral(@"true"">
                                <svg class=""svg-inline--fa fa-bars"" aria-hidden=""true"" focusable=""false"" data-prefix=""fas"" data-icon=""bars"" role=""img"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 448 512"" data-fa-i2svg=""""><path fill=""currentColor"" d=""M0 96C0 78.33 14.33 64 32 64H416C433.7 64 448 78.33 448 96C448 113.7 433.7 128 416 128H32C14.33 128 0 113.7 0 96zM0 256C0 238.3 14.33 224 32 224H416C433.7 224 448 238.3 448 256C448 273.7 433.7 288 416 288H32C14.33 288 0 273.7 0 256zM416 448H32C14.33 448 0 433.7 0 416C0 398.3 14.33 384 32 384H416C433.7 384 448 398.3 448 416C448 433.7 433.7 448 416 448z""></path></svg><!-- <i class=""fas fa-bars""></i> Font Awesome fontawesome.com -->
                            </button>
                            <span id=""menuTitle"">Thông tin chung</span>
                            <button type=""button"" class=""btn-close btn-close-white ms-auto"" onclick=""_multitabService.showTabByIndex(0);UpdatePageTitle(PAGE_TITLE);""></button>
                        </div>
      ");
            WriteLiteral(@"                  <div class=""tab-content flex-fill overflow-auto"" style=""height:0;"">
                            <div class=""tab-pane fade show active"" id=""tabThongTinChung"" role=""tabpanel"">
                                <div class=""container-fluid""><div id=""tabThongTinChungContent""></div></div>
                            </div>
                            <div class=""tab-pane fade"" id=""tabThongTinGCN"" role=""tabpanel"">
                                <div class=""container-fluid"">
                                    <div id=""tabThongTinGCNContent"">
                                    </div>
                                </div>
                                <div class=""table-responsive"">
                                    <table class=""table table-bordered rounded text-nowrap mb-0 mt-2"">
                                        <thead class=""thead-dark bg-primary text-white sticky-top"" style=""top:0.5px;"">
                                            <tr>
                                     ");
            WriteLiteral(@"           <th scope=""col"">STT</th>
                                                <th scope=""col"">Sản phẩm tham gia</th>
                                                <th scope=""col"">Tiền bảo hiểm</th>
                                                <th scope=""col"">Khấu trừ</th>
                                                <th scope=""col"">Miễn thường</th>
                                                <th scope=""col"">Điều khoản bổ sung</th>
                                                <th scope=""col"">Phí bảo hiểm</th>
                                            </tr>
                                        </thead>

                                        <tbody id=""tabThongTinLoaiHinhBaoHiemContent"">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class=""tab-pane fade h-100"" id=""tabThongTinVuTT"" role=""tabpanel"">
                   ");
            WriteLiteral(@"             <div class=""container-fluid""><div id=""tabThongTinVuTTContent""></div></div>
                            </div>
                            <div class=""tab-pane fade h-100"" id=""tabThongTinLichGiamDinh"" role=""tabpanel"">
                                <div class=""container-fluid""><div id=""tabThongTinLichGiamDinhContent""></div></div>
                                <div class=""col-12 px-2"">
                                    <span class=""fs-5 text-muted text-uppercase"">Đại diện các bên tham gia giám định</span>
                                </div>
                                <div class=""table-responsive"">
                                    <table class=""table table-bordered rounded text-nowrap mb-0"">
                                        <thead class=""thead-dark bg-primary text-white sticky-top"" style=""top:0.5px;"">
                                            <tr>
                                                <th scope=""col"">Ngày giám định</th>
                                 ");
            WriteLiteral(@"               <th scope=""col"">Đại diện</th>
                                                <th scope=""col"">Họ và tên</th>
                                            </tr>
                                        </thead>
                                        <tbody id=""tabThongTinDaiDienBenTGGDContent"">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class=""tab-pane fade h-100 d-none"" id=""tabThongTinHoSoGiayTo"" role=""tabpanel"">
                                <table class=""table table-bordered rounded text-nowrap mb-0"">
                                    <thead class=""thead-dark bg-primary text-white sticky-top"" style=""top:0.5px;"">
                                        <tr>
                                            <th scope=""col"">Tên giấy tờ</th>
                                            <th scope=""col"">Ngày cung cấp</th>
          ");
            WriteLiteral(@"                                  <th scope=""col"">Trạng thái</th>
                                            <th scope=""col"">Hợp lệ</th>
                                            <th scope=""col"">Loại</th>
                                            <th scope=""col"">Yêu cầu BS</th>
                                            <th scope=""col"">Ghi chú</th>
                                            <th scope=""col"">Cán bộ yêu cầu</th>
                                        </tr>
                                    </thead>
                                    <tbody id=""tabThongTinHoSoGiayToContent"">
                                    </tbody>
                                </table>
                            </div>
                            <div class=""tab-pane fade h-100"" id=""tabThongTinLichSuTonThat"" role=""tabpanel"">
                                <table class=""table table-bordered rounded text-nowrap mb-0"">
                                    <thead class=""thead-dark bg-primary text-whit");
            WriteLiteral(@"e sticky-top"" style=""top:0.5px;"">
                                        <tr>
                                            <th scope=""col"">STT</th>
                                            <th scope=""col"">Ngày mở</th>
                                            <th scope=""col"">Trạng thái</th>
                                            <th scope=""col"">Số hồ sơ</th>
                                            <th scope=""col"">Cán bộ bồi thường</th>
                                            <th scope=""col"">Biển xe</th>
                                            <th scope=""col"">Tên khách hàng</th>
                                            <th scope=""col"">Số tiền duyệt</th>
                                            <th scope=""col"">Nghiệp vụ</th>
                                            <th scope=""col"">Số HĐ</th>
                                            <th scope=""col"">Số GCN</th>
                                            <th scope=""col"">Đơn vị xử lý</th>
                            ");
            WriteLiteral(@"            </tr>
                                    </thead>
                                    <tbody id=""tabThongTinLichSuTonThatContent"">
                                    </tbody>
                                </table>
                            </div>
                            <div class=""tab-pane fade"" id=""tabQuaTrinhXuLy"" role=""tabpanel"">
                                <div class=""container px-xl-5"">
                                    <div class=""row g-3 px-lg-5"" id=""tabQuaTrinhXuLyContent"">
                                    </div>
                                </div>
                            </div>
                            <div class=""tab-pane fade"" id=""tabThongTinBoiThuong"" role=""tabpanel"">
                           
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script>
            function onClickTitleTabMenu(title) {
                $('#menuTi");
            WriteLiteral("tle\').html(title);\r\n            }\r\n        </script>\r\n    </div>\r\n</div>");
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