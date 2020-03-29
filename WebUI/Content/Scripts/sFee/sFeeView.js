
var StudentID = "0";
bindSelectFeeInfo = function () {
    var result = Easy.bindSelectInfo("../sFee/SelectsFeeView", sFeeID);
    var data = JSON.parse(result.Data)[0];
    $("#spDept").html(data.DeptName);
    $("#spVoucherNum").html(data.VoucherNum);
    $("#spNoteNum").html(data.NoteNum);
    $("#spName").html(data.StudName);
    $("#spIDCard").html(data.IDCard);
    $("#spFeeTime").html(data.FeeTime);
    $("#spFeeMode").html(data.FeeMode);
    $("#spShouldMoney").html(data.ShouldMoney);
    $("#spPaidMoney").html(parseFloat(parseFloat(data.PaidMoney) - parseFloat(data.DiscountMoney)).toFixed("2"));
    $("#spDiscountMoney").html(data.DiscountMoney);
    $("#spRefundMoney").html(data.RefundMoney);
    $("#spAffirmID").html(data.Affirm);
    $("#spAffirmTime").html(data.AffirmTime);
    $("#spStatus").html(data.Status);
    $("#spPlanName").html(data.PlanName);
    $("#spExplain").html(data.Explain);
    $("#spRemark").html(data.Remark);
    $("#spsFeeContent").html(data.FeeContent);
    $("#spOffset").html(data.OffsetMoney);
    $("#spByOffset").html(data.ByOffsetMoney); 
    $("#spsGive").html(data.GiveName);
    StudentID = data.StudentID;
}


tabsLoad = function (i) {
    $('#ViewInfo').tabs({
        border: false,
        selected: i,
        onSelect: function (title, index) {
            if (index === 0) {
                bindSelectFeeInfo();
            }
            else if (index === 1) {
                sfeeOrderList();  
            }
            else if(index===2) {
                Easy.bindSelectStudentInfo(StudentID);
            }
            else if(index===3) {
                sfeeRefund();
            }
        }
    });
}

sfeeRefund = function () {
    $("#refundGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        showFooter: true,
        queryParams: { sFeeID: sFeeID },//异步查询的参数
        columns: [[
            { field: "sRefundID", hidden: true },
            { field: "DeptName", title: "校区", width: 120, sortable: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "Name", title: "缴费人", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "Content", title: "缴费方案", width: 100, sortable: true },
            { field: "DetailName", title: "收费项", width: 100, sortable: true },
            { field: "Sort", title: "核销类别", width: 80, sortable: true },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' },
            { field: "RefundTime", title: "核销时间", width: 80, sortable: true },
            { field: "PayObject", title: "支付对象", width: 80, sortable: true },
            { field: "Creater", title: "核销员", sortable: true, width: 80 },
            { field: "Remark", title: "核销备注", sortable: true, width: 200 }

        ]],
        url: "../sRefund/GetViewsRefundList", sortName: "sRefundID", sortOrder: "desc",
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
}
tabsLoad(0);

sfeeOrderList = function () {
    $("#sfeeOrder").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        showFooter: true,
        queryParams: { sFeeID: sFeeID },//异步查询的参数
        columns: [[
            { field: "sFeesOrderID", hidden: true },
            { field: "DetailName", title: "收费类别", width: 120, sortable: true },
            { field: "ShouldMoney", title: "应收金额", halign: 'left', align: 'right', sortable: true },
            { field: "PaidMoney", title: "实收金额", halign: 'left', align: 'right', sortable: true },
            { field: "DiscountMoney", title: "优惠金额", halign: 'left', align: 'right', sortable: true },
            { field: "OffsetMoney", title: "充抵金额", halign: 'left', align: 'right', sortable: true },
            { field: "ByOffsetMoney", title: "被充抵金额", halign: 'left', align: 'right', sortable: true },
            { field: "RefundMoney", title: "核销金额", halign: 'left', align: 'right', sortable: true }
        ]],
        url: "../sFee/GetsFeeOrderList", sortName: "sFeesOrderID", sortOrder: "asc"
    });
};