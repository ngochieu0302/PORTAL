#pragma checksum "D:\project\ESCS_PORTAL\ESCS_PORTAL\Areas\Contract\Views\Health\_HealthClaimContentTab3.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b2ac7c1041cb9a6c35dd4e5f714ac8c08c580502"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Contract_Views_Health__HealthClaimContentTab3), @"mvc.1.0.view", @"/Areas/Contract/Views/Health/_HealthClaimContentTab3.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b2ac7c1041cb9a6c35dd4e5f714ac8c08c580502", @"/Areas/Contract/Views/Health/_HealthClaimContentTab3.cshtml")]
    public class Areas_Contract_Views_Health__HealthClaimContentTab3 : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<style>
    input {
        background-color: #ffffff !important;
    }

    .divActived {
        background-color: #e6eaee;
    }
</style>

<div id=""divThemHoSoNguoi"" class=""card h-100 shadow d-none"">
    <div class=""card-header d-flex flex-nowrap gap-3 pe-3"">
        <a href=""javascript:_multitabService.showTabByIndex(2);""><i class=""fas fa-chevron-left me-2""></i>Quay lại</a>
        <button type=""button"" class=""btn-close ms-auto"" onclick=""_multitabService.showTabByIndex(0);UpdatePageTitle(PAGE_TITLE);""></button>
    </div>
    <div class=""card-body d-flex flex-column gap-2 overflow-auto pt-3"">
        <form name=""frmThongTinLienHe"" novalidate=""novalidate"" method=""post"">
            <input type=""hidden"" name=""so_hs""");
            BeginWriteAttribute("value", " value=\"", 744, "\"", 752, 0);
            EndWriteAttribute();
            WriteLiteral(@">
            <div class=""row"">
                <div class=""col-12 col-md-4 col-xxl-4"" style=""border-right:1px solid #00000014; padding:0px 20px 0px 20px;"" id=""khaiBaoHoSoBTTab1"">
                    <div class=""row card-header"">
                        <div class=""row"">
                            <div class=""col-12 text-center"">
                                <h4>Người thông báo/ liên hệ </h4>
                            </div>
                        </div>
                    </div>
                    <div class=""row card-body"" style=""height:55vh; overflow:auto"">
                        <div class=""row mt-3"">
                            <div class=""col-12"">
                                <h5 class=""m-0 pd-y-10"">Thông tin người liên hệ</h5>
                            </div>
                            <div class=""col-12 mt-3"">
                                <div class=""custom-control custom-checkbox"">
                                    <input class=""custom-control-input"" type=""checkb");
            WriteLiteral(@"ox"" id=""chkThamGiaLienHe"" value=""option1"">
                                    <label class=""custom-control-label"" for=""chkThamGiaLienHe"" style=""font-size:12px""><b style=""font-weight:bold"">Người được bảo hiểm là người liên hệ</b></label>
                                </div>
                            </div>
                        </div>
                        <div class=""row mt-3"">
                            <div class=""col-6"">
                                <div class=""form-group"">
                                    <label class=""_required"">Họ và tên</label>
                                    <input type=""text"" autocomplete=""off"" class=""form-control""");
            BeginWriteAttribute("required", " required=\"", 2454, "\"", 2465, 0);
            EndWriteAttribute();
            WriteLiteral(@" maxlength=""100"" name=""nguoi_lh"" placeholder=""VD: Nguyễn Văn A"">
                                </div>
                            </div>
                            <div class=""col-6"">
                                <div class=""form-group"">
                                    <label class=""_required"">Mối quan hệ</label>
                                    <select class=""select"" name=""nguoi_lhla"" id=""nguoi_lhla"">
                                        <option");
            BeginWriteAttribute("value", " value=\"", 2939, "\"", 2947, 0);
            EndWriteAttribute();
            WriteLiteral(@">Chọn mối quan hệ</option>
                                        <option value=""BO"">Bố</option>
                                        <option value=""ME"">Mẹ</option>
                                        <option value=""BAN_THAN"">Bản thân</option>
                                        <option value=""KHAC"">Khác (Anh/Chị/Em...)</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class=""row mt-3"">
                            <div class=""col-6"">
                                <div class=""form-group"">
                                    <label");
            BeginWriteAttribute("class", " class=\"", 3648, "\"", 3656, 0);
            EndWriteAttribute();
            WriteLiteral(@">Điện thoại</label>
                                    <input type=""text"" fn-validate=""validatePhoneControl"" autocomplete=""off"" class=""form-control phone"" name=""dthoai_nguoi_lh"" placeholder=""VD: 0972xxxxxx"" im-insert=""true"">
                                </div>
                            </div>
                            <div class=""col-6"">
                                <div class=""form-group"">
                                    <label>Email</label>
                                    <input type=""text"" fn-validate=""validateEmailControl"" autocomplete=""off"" class=""form-control email-inputmask"" maxlength=""100"" name=""email_nguoi_lh"" placeholder=""VD: mail@escs.vn"">
                                </div>
                            </div>
                        </div>
                        <div class=""row mt-3"">
                            <div class=""col-12"">
                                <h5 class=""m-0 pd-y-10"">Thông tin người thông báo</h5>
                            </div>
        ");
            WriteLiteral(@"                    <div class=""col-12 mt-3"">
                                <div class=""custom-control custom-checkbox"">
                                    <input class=""custom-control-input"" type=""checkbox"" id=""chkThamGiaThongBao"" value=""option1"">
                                    <label class=""custom-control-label"" for=""chkThamGiaThongBao"" style=""font-size:12px""><b style=""font-weight:bold"">Người được bảo hiểm là người thông báo</b></label>
                                </div>
                            </div>
                        </div>
                        <div class=""row mt-3"">
                            <div class=""col-6"">
                                <div class=""form-group"">
                                    <label class=""_required"">Họ và tên</label>
                                    <input type=""text"" autocomplete=""off"" class=""form-control""");
            BeginWriteAttribute("required", " required=\"", 5573, "\"", 5584, 0);
            EndWriteAttribute();
            WriteLiteral(@" maxlength=""100"" name=""nguoi_tb"" placeholder=""VD: Nguyễn Văn A"">
                                </div>
                            </div>
                            <div class=""col-6"">
                                <div class=""form-group"">
                                    <label");
            BeginWriteAttribute("class", " class=\"", 5876, "\"", 5884, 0);
            EndWriteAttribute();
            WriteLiteral(@">Điện thoại</label>
                                    <input type=""text"" autocomplete=""off"" fn-validate=""validatePhoneControl"" class=""form-control phone"" name=""dthoai_nguoi_tb"" placeholder=""VD: 0972xxxxxx"" im-insert=""true"">
                                </div>
                            </div>
                            <div class=""col-6"">
                                <div class=""form-group"">
                                    <label>Email</label>
                                    <input type=""text"" autocomplete=""off"" fn-validate=""validateEmailControl"" class=""form-control email-inputmask"" maxlength=""100"" name=""email_nguoi_tb"" placeholder=""VD: mail@escs.vn"">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class=""row card-footer mt-3"">
                        <div class=""col-12"">
                            <button type=""button"" class=""btn btn-md btn-primary btn-tab-1"" onclick=""");
            WriteLiteral(@"OnTiepTheo('tab_1')"" style=""float:right"">
                                Tiếp theo <i class=""fas fa-chevron-right ml-2""></i>
                            </button>
                        </div>
                    </div>
                </div>

                <div class=""col-12 col-md-4 col-xxl-4"" style=""border-right:1px solid #00000014; padding:0px 20px 0px 20px"" id=""khaiBaoHoSoBTTab2"">
                    <div class=""row card-header"">
                        <div class=""row"">
                            <div class=""col-12 text-center"">
                                <h4>Thông tin lần khám </h4>
                            </div>
                        </div>
                    </div>
                    <div class=""row card-body"" style=""height:55vh; overflow:auto"">
                        <div class=""col-12"">
                            <div class=""row"" style=""margin-top:5px;"">
                                <div class=""col-6"">
                                    <label class=""form-");
            WriteLiteral(@"label _required"">Nguyên nhân</label>
                                    <select class=""select"" id=""nhom_nguyen_nhan"" name=""nhom_nguyen_nhan""></select>
                                    <label class=""form-label select-label"">Chọn nhóm nguyên nhân</label>
                                </div>
                                <div class=""col-6"">
                                    <label class=""form-label _required"">Hình thức điều trị</label>
                                    <select class=""select"" id=""hinh_thuc"" name=""hinh_thuc"" required></select>
                                    <label class=""form-label select-label"">Chọn hình thức điều trị</label>
                                </div>
                            </div>
                            <div class=""row mt-3"">
                                <div class=""col-6"">
                                    <label class=""_required"">Ngày vào viện</label>
                                    <div class=""form-outline datepicker"" data-mdb-inlin");
            WriteLiteral(@"e=""true"" data-mdb-format=""dd/mm/yyyy"">
                                        <input type=""text"" required class=""form-control"" name=""ngay_vv"" value-format=""number"" placeholder=""ngày vào viện"" />
                                    </div>
                                </div>
                                <div class=""col-6"">
                                    <label class=""_required"">Ngày ra viện</label>
                                    <div class=""form-outline datepicker"" data-mdb-inline=""true"" data-mdb-format=""dd/mm/yyyy"">
                                        <input type=""text"" class=""form-control"" name=""ngay_rv"" required value-format=""number"" placeholder=""ngày ra viện"" />
                                    </div>
                                </div>
                            </div>
                            <div class=""row mt-3 d-none tai_nan"">
                                <div class=""col-6"">
                                    <label");
            BeginWriteAttribute("class", " class=\"", 9941, "\"", 9949, 0);
            EndWriteAttribute();
            WriteLiteral(@" style=""margin-bottom:7px"">Ngày xảy ra</label>
                                    <div class=""form-outline datepicker"" data-mdb-inline=""true"" data-mdb-format=""dd/mm/yyyy"">
                                        <input type=""text"" class=""form-control"" name=""ngay_xr"" value-format=""number"" placeholder=""ngày xảy ra"" />
                                    </div>
                                </div>
                                <div class=""col-6"">
                                    <label class=""form-label"">Nơi xảy ra</label>
                                    <input type=""text"" autocomplete=""off"" class=""form-control email-inputmask"" maxlength=""100"" name=""noi_xr"" placeholder=""Nơi xảy ra"">
                                </div>
                            </div>
                            <div class=""row mt-3 d-none tai_nan"">
                                <div class=""col-6"">
                                    <label class=""form-label"">Nguyên nhân tai nạn</label>
                            ");
            WriteLiteral(@"        <input type=""text"" autocomplete=""off"" class=""form-control email-inputmask"" maxlength=""100"" name=""nguyen_nhan_tnan"" placeholder=""Nguyên nhân"">
                                </div>
                                <div class=""col-6"">
                                    <label class=""form-label"">Hậu quả</label>
                                    <input type=""text"" autocomplete=""off"" class=""form-control email-inputmask"" maxlength=""100"" name=""hau)qua_ct"" placeholder=""Hậu quả"">
                                </div>
                            </div>
                            <div class=""row mt-3"">
                                <div class=""col-12"">
                                    <label class=""form-label"">Tỉnh thành</label>
                                    <select class=""select"" id=""tinh_thanh"" name=""tinh_thanh""></select>
                                    <label class=""form-label select-label"">Chọn tỉnh thành</label>
                                </div>
                        ");
            WriteLiteral(@"    </div>
                            <div class=""row mt-3"">
                                <div class=""col-12"">
                                    <label class=""form-label _required"">Cơ sở y tế</label>
                                    <select class=""select"" id=""benh_vien"" name=""benh_vien"" required></select>
                                    <label class=""form-label select-label"">Chọn cơ sở y tế</label>
                                </div>
                            </div>
                            <div class=""row mt-3"">
                                <div class=""col-12"">
                                    <label class=""form-label _required"">Chẩn đoán bệnh</label>
                                    <input type=""text"" autocomplete=""off"" required class=""form-control email-inputmask"" maxlength=""100"" name=""chan_doan"" placeholder=""Chẩn đoán"">
                                </div>
                            </div>
                        </div>
                    </div>
          ");
            WriteLiteral(@"          <div class=""row card-footer mt-3"">
                        <div class=""col-12"">
                            <button type=""button"" class=""btn btn-md btn-primary btn-tab-2"" onclick=""OnQuayVe('tab_2')"" style=""float: left"">
                                <i class=""fas fa-chevron-left""></i> Quay về
                            </button>
                            <button type=""button"" class=""btn btn-md btn-primary btn-tab-2"" onclick=""OnTiepTheo('tab_2')"" style=""float: right"">
                                Tiếp theo <i class=""fas fa-chevron-right ml-2""></i>
                            </button>
                        </div>
                    </div>
                </div>

                <div class=""col-12 col-md-4 col-xxl-4"" style=""padding:0px 20px 0px 20px"" id=""khaiBaoHoSoBTTab3"">
                    <div class=""row card-header"">
                        <div class=""row"">
                            <div class=""col-12 text-center"">
                                <h4>Thông tin thụ h");
            WriteLiteral(@"ưởng bảo hiểm </h4>
                            </div>
                        </div>
                    </div>
                    <div class=""row card-body"" style=""height:55vh; overflow:auto"">
                        <div class=""row mt-3"">
                            <div class=""col-12"">
                                <label class=""form-label _required"">Họ tên người thụ hưởng</label>
                                <input type=""text"" required autocomplete=""off"" class=""form-control email-inputmask"" maxlength=""100"" name=""thu_huong_ten"" placeholder=""Tên người thụ hưởng"">
                            </div>
                        </div>
                        <div class=""row mt-3"">
                            <div class=""col-12"">
                                <label class=""form-label _required"">Số tài khoản</label>
                                <input type=""text"" required autocomplete=""off"" class=""form-control email-inputmask"" maxlength=""100"" name=""thu_huong_tk"" placeholder=""Số tài khoản th");
            WriteLiteral(@"ụ hưởng"">
                            </div>
                        </div>
                        <div class=""row mt-3"">
                            <div class=""col-12"">
                                <label class=""form-label"">Ngân hàng thụ hưởng</label>
                                <select class=""select"" id=""thu_huong_ngan_hang"" name=""thu_huong_ngan_hang""></select>
                                <label class=""form-label select-label"">Chọn ngân hàng</label>
                            </div>
                        </div>
                        <div class=""row mt-3"">
                            <div class=""col-12"">
                                <label class=""form-label"">Chi nhánh ngân hàng thụ hưởng</label>
                                <select class=""select"" id=""thu_huong_cn_ngan_hang"" name=""thu_huong_cn_ngan_hang""></select>
                                <label class=""form-label select-label"">Chọn chi nhánh ngân hàng</label>
                            </div>
                   ");
            WriteLiteral(@"     </div>
                        <div class=""row mt-3"">
                            <div class=""col-12"">
                                <label class=""form-label _required"">Số tiền yêu cầu</label>
                                <input type=""text"" required autocomplete=""off"" class=""form-control email-inputmask number"" maxlength=""100"" name=""tien_yc"">
                            </div>
                        </div>
                    </div>
                    <div class=""row card-footer mt-3"">
                        <div class=""col-12"">
                            <button type=""button"" class=""btn btn-md btn-primary btn-tab-3"" onclick=""OnQuayVe('tab_3')"" style=""float: left"">
                                <i class=""fas fa-chevron-left""></i> Quay về
                            </button>
                            <button type=""button"" class=""btn btn-md btn-primary btn-tab-3"" onclick=""OnTiepTheo('tab_3')"" style=""float: right"">
                                Mở hồ sơ <i class=""fas fa-chevron");
            WriteLiteral("-right ml-2\"></i>\r\n                            </button>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </form>\r\n    </div>\r\n</div>");
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
