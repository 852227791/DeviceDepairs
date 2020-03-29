var StudentID = "0";
bindSelectFeeInfo = function () {
    var result = Easy.bindSelectInfo("../Fee/SelectViewFee", FeeID);
    $("#spDept").html(result.Data[0].DeptName);
    $("#spVoucherNum").html(result.Data[0].VoucherNum);
    $("#spNoteNum").html(result.Data[0].NoteNum);
    $("#spName").html(result.Data[0].StudentName);
    $("#spIDCard").html(result.Data[0].IDCard);
    $("#spFeeTime").html(result.Data[0].FeeTime);
    $("#spFeeMode").html(result.Data[0].FeeMode);
    $("#spShouldMoney").html(result.Data[0].ShouldMoney);
    $("#spPaidMoney").html(result.Data[0].PaidMoney);
    $("#spOffset").html(result.Data[0].OffsetMoney);
    $("#spByOffset").html(result.Data[0].ByOffsetMoney);
    $("#spDiscountMoney").html(result.Data[0].DiscountMoney);
    $("#spRefundMoney").html(result.Data[0].RefundMoney);
    $("#spAffirmID").html(result.Data[0].AffirmName);
    $("#spAffirmTime").html(result.Data[0].AffirmTime);
    $("#spTeacher").html(result.Data[0].Teacher);
    $("#spStatus").html(result.Data[0].Status);
    $("#spItemName").html(result.Data[0].ItemName);
    $("#spClassName").html(result.Data[0].ClassName);
    $("#spFeeContent").html(result.Data[0].FeeContent);
    $("#spExplain").html(result.Data[0].Explain);
    $("#spRemark").html(result.Data[0].Remark);
    $("#spPersonSort").html(result.Data[0].PersonSort);
    StudentID = result.Data[0].StudentID;
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
             Easy.bindSelectStudentInfo(StudentID);
            }
            else if (index === 2) {
                feeRefundList();
            }
        }
    });
}
tabsLoad(0);

feeRefundList = function () {
    $("#refundGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        queryParams: { FeeID: FeeID },//异步查询的参数
        columns: [[
            { field: "RefundID", hidden: true },
             { field: "DeptName", title: "校区", width: 100, sortable: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
             { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "Name", title: "缴费人", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "ItemName", title: "证书名称", width: 100, sortable: true },
            { field: "ItemDetailName", title: "收费项", width: 100, sortable: true },
            { field: "Sort", title: "核销类别", width: 80, sortable: true },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' },
            { field: "RefundTime", title: "核销时间", width: 80, sortable: true },
            { field: "PayObject", title: "支付对象", width: 80, sortable: true },
            { field: "Creater", title: "核销员", sortable: true, width: 80 },
            { field: "Status", title: "状态", sortable: true, width: 80 },
            { field: "Remark", title: "核销备注", sortable: true, width: 200 }

        ]],
        url: "../Refund/GetViewRefundList", sortName: "RefundID", sortOrder: "desc",
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
}